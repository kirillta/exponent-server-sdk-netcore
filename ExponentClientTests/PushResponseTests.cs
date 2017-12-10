using Floxdc.ExponentServerSdk;
using Floxdc.ExponentServerSdk.Exceptions;
using Floxdc.ExponentServerSdk.Infrastructure;
using Xunit;

namespace ExponentClientTests
{
    public class PushResponseTests
    {
        [Fact]
        public void ValidateResponse_ShouldThrowDeviceNotRegisteredException()
        {
            var response = new PushResponse(PushResponseStatuses.Error,
                details: new PushResponse.ContentDetails("DeviceNotRegistered"));

            Assert.Throws<DeviceNotRegisteredException>(() => response.ValidateResponse());
        }


        [Fact]
        public void ValidateResponse_ShouldThrowMessageTooBigException()
        {
            var response = new PushResponse(PushResponseStatuses.Error,
                details: new PushResponse.ContentDetails("MessageTooBig"));

            Assert.Throws<MessageTooBigException>(() => response.ValidateResponse());
        }


        [Fact]
        public void ValidateResponse_ShouldThrowMessageRateExceededException()
        {
            var response = new PushResponse(PushResponseStatuses.Error,
                details: new PushResponse.ContentDetails("MessageRateExceeded"));

            Assert.Throws<MessageRateExceededException>(() => response.ValidateResponse());
        }


        [Fact]
        public void ValidateResponse_ShouldThrowPushResponseException()
        {
            var response = new PushResponse(PushResponseStatuses.Error,
                details: new PushResponse.ContentDetails("Other"));

            Assert.Throws<PushResponseException>(() => response.ValidateResponse());
        }


        [Fact]
        public void ValidateResponse_ShouldPassWhenStatusIsSuccess()
        {
            var response = new PushResponse(PushResponseStatuses.Ok,
                details: new PushResponse.ContentDetails("Other"));

            var ex = Record.Exception(() => response.ValidateResponse());
            Assert.Null(ex);
        }
    }
}
