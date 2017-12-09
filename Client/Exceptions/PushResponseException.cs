using System;

namespace Floxdc.Exponent.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all push reponse errors.
    /// </summary>
    public class PushResponseException : Exception
    {
        protected internal PushResponseException(string pushResponse) : base(string.IsNullOrWhiteSpace(pushResponse)
            ? "Unknown push response error"
            : pushResponse)
        {
        }
    }
}
