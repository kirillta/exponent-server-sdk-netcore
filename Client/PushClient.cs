using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Floxdc.ExponentServerSdk.Exceptions;
using Floxdc.ExponentServerSdk.Response;
using Newtonsoft.Json;

namespace Floxdc.ExponentServerSdk
{
    public class PushClient : IDisposable
    {
        /// <summary>
        /// Exponent push client.      
        /// See full API docs at https://docs.getexponent.com/versions/v13.0.0/guides/push-notifications.html#http-2-api
        /// </summary>
        /// <param name="host">The server protocol, hostname, and port.</param>
        /// <param name="apiUrl">The api url at the host.</param>
        /// <param name="httpClient">The http/2 client with a decompression support.</param>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="PushServerException"></exception>
        public PushClient(string host = null, string apiUrl = null, HttpClient httpClient = null)
        {
            var internalHost = string.IsNullOrWhiteSpace(host) ? DefaultHost : host;
            var internalApiUrl = string.IsNullOrWhiteSpace(apiUrl) ? DefaultBaseApiUrl : apiUrl;
            PushUrl = new Uri(internalHost + internalApiUrl + "/push/send", UriKind.Absolute);

            _client = httpClient;
            if (_client is null)
            {
                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                _client = new HttpClient(handler);
                _isClientLocal = true;
            }

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public void Dispose()
        {
            if (_isClientLocal)
                _client?.Dispose();
        }


        /// <summary>
        /// Returns `True` if the token is an Exponent push token.
        /// </summary>
        /// <param name="token"></param>
        public static bool IsExponentPushToken(string token) 
            => !string.IsNullOrWhiteSpace(token) && token.StartsWith("ExponentPushToken");


        /// <summary>
        /// Sends a single push notification.
        /// </summary>
        /// <param name="pushMessage">A single PushMessage object.</param>
        /// <returns>A PushResponse object which contains the results.</returns>
        public async Task<PushResponse> Publish(PushMessage pushMessage)
            => (await PublishMultiple(new[] {pushMessage})).FirstOrDefault();


        /// <summary>
        /// Sends multiple push notifications at once.
        /// </summary>
        /// <param name="pushMessages">An array of PushMessage objects.</param>
        /// <returns>A read-only collection of PushResponse objects which contains the results.</returns>
        public async Task<IReadOnlyCollection<PushResponse>> PublishMultiple(IEnumerable<PushMessage> pushMessages) 
            => await PublishInternal(pushMessages);


        private HttpRequestMessage BuildRequest(string json)
            => new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = PushUrl,
                Version = new Version(2, 0)
            };


        private async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.SendAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"{response.StatusCode}: {response.ReasonPhrase}");

                return response;
            }
            catch(HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PushServerException(ex.Message, response);
            }
        }


        private async Task<IReadOnlyCollection<PushResponse>> PublishInternal(IEnumerable<PushMessage> pushMessages)
        {
            var messages = pushMessages as PushMessage[] ?? pushMessages.ToArray();
            var payloads = messages.AsParallel().Select(m => m.GetPayload()).ToArray();
            var json = JsonConvert.SerializeObject(payloads);
            var request = BuildRequest(json);

            var response = await GetResponse(request);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData>(content);

            if (data.Errors != null && data.Errors.Any())
                throw new PushServerException("Request failed.", response, data, data.Errors);

            if (data.Data == null)
                throw new PushServerException("Invalid server response.", response, data);

            if (messages.Length != data.Data.Count)
                throw new PushServerException(
                    $"Mismatched response length. Expected {messages.Length}, but only {data.Data.Count} received.",
                    response, data);

            return data.Data.AsReadOnly();
        }


        private Uri PushUrl { get; }


        private readonly HttpClient _client;
        private const string DefaultBaseApiUrl = "/--/api/v2";
        private const string DefaultHost = "https://exp.host";
        private readonly bool _isClientLocal;
    }
}
