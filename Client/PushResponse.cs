using Floxdc.Exponent.Exceptions;
using Floxdc.Exponent.Infrastructure;

namespace Floxdc.Exponent
{
    /// <summary>
    /// Wrapper class for a push notification response. 
    /// A successful single push notification: 
    ///   {'status': 'ok'}
    /// An invalid push token 
    ///   {'status': 'error', 'message': '"adsf" is not a registered push notification recipient'}
    /// </summary>
    /// <exception cref="PushResponseException"></exception>
    /// <exception cref="DeviceNotRegisteredException"></exception>
    /// <exception cref="MessageTooBigException"></exception>
    /// <exception cref="MessageRateExceededException"></exception>
    public class PushResponse
    {
        public PushResponse(PushResponseStatuses status, string message = null, ContentDetails details = null)
        {
            Status = status;
            Message = message;
            Details = details;
        }


        /// <summary>
        /// Returns True if this push notification successfully sent.
        /// </summary>
        public bool IsSuccess() 
            => Status == PushResponseStatuses.Ok;


        /// <summary>
        /// Raises an exception if there was an error. Otherwise, do nothing. 
        /// Clients should handle these errors, since these require custom handling 
        /// to properly resolve.
        /// </summary>
        public void ValidateResponse()
        {
            if (IsSuccess())
                return;

            var error = Details?.Error;
            switch (error)
            {
                case "DeviceNotRegistered":
                    throw new DeviceNotRegisteredException(ToString());
                case "MessageTooBig":
                    throw new MessageTooBigException(ToString());
                case "MessageRateExceeded":
                    throw new MessageRateExceededException(ToString());
            }
            
            throw new PushResponseException(ToString());
        }


        public ContentDetails Details { get; }
        public string Message { get; }
        public PushResponseStatuses Status { get; }


        public class ContentDetails
        {
            public ContentDetails(string error)
            {
                Error = error;
            }

            public string Error { get; }
        }
    }
}
