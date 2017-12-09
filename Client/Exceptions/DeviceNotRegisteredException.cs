namespace Floxdc.Exponent.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Raised when the push token is invalid. 
    /// To handle this error, you should stop sending messages to this token.
    /// </summary>
    public class DeviceNotRegisteredException : PushResponseException
    {
        protected internal DeviceNotRegisteredException(string pushResponse) : base(pushResponse)
        {
        }
    }
}
