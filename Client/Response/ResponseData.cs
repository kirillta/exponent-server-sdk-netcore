using System.Collections.Generic;

namespace Floxdc.ExponentServerSdk.Response
{
    public class ResponseData
    {
        public ResponseData(List<PushResponse> data, List<ResponseError> errors)
        {
            Data = data;
            Errors = errors;
        }


        public List<PushResponse> Data { get; }
        public List<ResponseError> Errors { get; }
    }
}