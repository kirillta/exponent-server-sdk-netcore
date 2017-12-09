namespace Floxdc.Exponent.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Raised when you are sending messages too frequently to a device. 
    /// You should implement exponential backoff and slowly retry sending messages.
    /// </summary>
    public class MessageRateExceededException : PushResponseException
    {
        protected internal MessageRateExceededException(string pushResponse) : base(pushResponse)
        {
        }
    }
}
