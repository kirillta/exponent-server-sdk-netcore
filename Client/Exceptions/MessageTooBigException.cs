namespace Floxdc.Exponent.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Raised when the notification was too large. 
    /// On Android and iOS, the total payload must be at most 4096 bytes.
    /// </summary>
    public class MessageTooBigException : PushResponseException
    {
        protected internal MessageTooBigException(string pushResponse) : base(pushResponse)
        {
        }
    }
}
