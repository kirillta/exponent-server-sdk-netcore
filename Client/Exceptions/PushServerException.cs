using System;
using System.Collections.Generic;
using System.Net.Http;
using Floxdc.Exponent.Response;

namespace Floxdc.Exponent.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Raised when the push token server is not behaving as expected.
    /// For example, invalid push notification arguments result in a different
    /// style of error. Instead of a "data" array containing errors per
    /// notification, an "error" array is returned.
    /// </summary>
    /// <code>
    /// {"errors": [
    ///   {"code": "API_ERROR",
    ///    "message": "child \"to\" fails because [\"to\" must be a string]. \"value\" must be an array."
    ///   }
    /// ]}
    /// </code>
    public class PushServerException : Exception
    {
        public PushServerException(string message, HttpResponseMessage response, ResponseData responseData = null, IReadOnlyCollection<ResponseError> errors = null) : base(message)
        {
            Response = response;
            ResponseData = responseData;
            Errors = errors;
        }


        public IReadOnlyCollection<ResponseError> Errors { get; }
        public HttpResponseMessage Response { get; }
        public ResponseData ResponseData { get; }
    }
}
