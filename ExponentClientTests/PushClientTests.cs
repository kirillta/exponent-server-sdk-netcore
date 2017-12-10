using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Floxdc.ExponentServerSdk;
using Floxdc.ExponentServerSdk.Exceptions;
using Floxdc.ExponentServerSdk.Infrastructure;
using Xunit;

namespace ExponentClientTests
{
    public class PushClientTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\r\n")]
        [InlineData("token")]
        public void IsExponentPushToken_ShouldReturnFalseWhenTokenIsEmptyOrWrong(string token)
        {
            var isValid = PushClient.IsExponentPushToken(token);

            Assert.False(isValid);
        }


        [Fact]
        public void IsExponentPushToken_ShouldReturnTrue()
        {
            var isValid = PushClient.IsExponentPushToken(Token);

            Assert.True(isValid);
        }

        [Fact]
        public async Task Publish_ShouldThrowAggregateExceptionWhenPushMessageIsEmpty()
        {
            var pushClient = new PushClient();

            await Assert.ThrowsAsync<AggregateException>(async () => await pushClient.Publish(null));
        }


        [Fact]
        public async Task Publish_ShouldThrowHttpRequestExceptionWhenStatusCodeIsNot200()
        {
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden)));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            await Assert.ThrowsAsync<HttpRequestException>(async () => await pushClient.Publish(new PushMessage(Token)));
        }


        [Fact]
        public async Task Publish_ShouldThrowPushServerExceptionWhenHttpClientReturnsAnError()
        {
            var handler = new TestHandler((message, token) => throw new InvalidOperationException());
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            await Assert.ThrowsAsync<PushServerException>(async () => await pushClient.Publish(new PushMessage(Token)));
        }


        [Fact]
        public async Task Publish_ShouldThrowPushServerExceptionWhenResponseHasErrors()
        {
            const string code = "INTERNAL_SERVER_ERROR";
            const string errorMessage = "An unknown error occurred.";
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\r\n  \"errors\": [{\r\n    \"code\": \"" + code + "\",\r\n    \"message\": \"" + errorMessage + "\"\r\n  }]\r\n}",
                    Encoding.UTF8, "application/json")
            }));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            var ex = await Record.ExceptionAsync(async () => await pushClient.Publish(new PushMessage(Token)));
            var pushEx = ex as PushServerException;

            Assert.Equal("Request failed.", ex.Message);
            Assert.Equal(code, pushEx.Errors.First().Code);
            Assert.Equal(errorMessage, pushEx.Errors.First().Message);
        }


        [Fact]
        public async Task Publish_ShouldThrowPushServerExceptionWhenResponseHasNoData()
        {
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\r\n  \"data\": null\r\n}",
                    Encoding.UTF8, "application/json")
            }));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            var ex = await Record.ExceptionAsync(async () => await pushClient.Publish(new PushMessage(Token)));

            Assert.Equal("Invalid server response.", ex.Message);
        }


        [Fact]
        public async Task Publish_ShouldThrowPushServerExceptionWhenResponseHasAMismatchedAmoundOfData()
        {
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\r\n  \"data\": [\r\n    {\"status\": \"ok\"},\r\n    {\"status\": \"ok\"}\r\n  ]\r\n}",
                    Encoding.UTF8, "application/json")
            }));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            var ex = await Record.ExceptionAsync(async () => await pushClient.Publish(new PushMessage(Token)));

            Assert.Equal("Mismatched response length. Expected 1, but only 2 received.", ex.Message);
        }


        [Fact]
        public async Task Publish_ShouldReturnErrorResponse()
        {
            const string error = "DeviceNotRegistered";
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\r\n  \"data\": [{\r\n    \"status\": \"error\",\r\n    \"message\": \"\\\"ExponentPushToken[xxxxxxxxxxxxxxxxxxxxxx]\\\" is not a registered push notification recipient\",\r\n    \"details\": {\r\n      \"error\": \"" + error + "\"\r\n    }\r\n  }]\r\n}",
                    Encoding.UTF8, "application/json")
            }));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            var response = await pushClient.Publish(new PushMessage(Token));

            Assert.Equal(PushResponseStatuses.Error, response.Status);
            Assert.Equal(error, response.Details.Error);
            Assert.Equal("\"ExponentPushToken[xxxxxxxxxxxxxxxxxxxxxx]\" is not a registered push notification recipient", response.Message);
        }


        [Fact]
        public async Task Publish_ShouldReturnOkResponse()
        {
            var handler = new TestHandler((message, token) => Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\r\n  \"data\": [\r\n    {\"status\": \"ok\"}\r\n  ]\r\n}",
                    Encoding.UTF8, "application/json")
            }));
            var httpClient = new HttpClient(handler);
            var pushClient = new PushClient(httpClient: httpClient);

            var response = await pushClient.Publish(new PushMessage(Token));

            Assert.Equal(PushResponseStatuses.Ok, response.Status);
        }


        private const string Token = "ExponentPushToken[xxxxxxxxxxxxxxxxxxxxxx]";
    }
}
