namespace Floxdc.ExponentServerSdk.Response
{
    public class ResponseError
    {
        public ResponseError(string code, string message)
        {
            Code = code;
            Message = message;
        }


        public string Code { get; }
        public string Message { get; }
    }
}
