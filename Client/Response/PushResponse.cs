﻿using Floxdc.ExponentServerSdk.Enums;
using Floxdc.ExponentServerSdk.Exceptions;

namespace Floxdc.ExponentServerSdk.Response
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

            switch (Details?.Error)
            {
                case "DeviceNotRegistered":
                    throw new DeviceNotRegisteredException(ToString());
                case "MessageTooBig":
                    throw new MessageTooBigException(ToString());
                case "MessageRateExceeded":
                    throw new MessageRateExceededException(ToString());
                default:
                    throw new PushResponseException(ToString());
            }
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
