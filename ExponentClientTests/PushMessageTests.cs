using System;
using Floxdc.ExponentServerSdk;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

namespace ExponentClientTests
{
    public class PushMessageTests
    {
        [Fact]
        public void GetPayload_ShouldThrowArgumentExceptionIfTokenIsInvalid()
        {
            var message = new PushMessage("invalid string");

            Assert.Throws<ArgumentException>(() => message.GetPayload());
        }


        [Fact]
        public void GetPayload_ShouldReturnPayload()
        {
            var message = new PushMessage(Token);

            var payload = message.GetPayload();

            Assert.Equal(Token, payload.to);
            Assert.Equal("default", payload.priority);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithBadgeWhenBadgeIsSpecified(int badge)
        {
            var message = new PushMessage(Token, badge: badge);

            var payload = message.GetPayload();

            Assert.Equal(badge, payload.badge);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithBodyWhenBodyIsSpecified()
        {
            const string body = "body";
            var message = new PushMessage(Token, body: body);

            var payload = message.GetPayload();

            Assert.Equal(body, payload.body);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithDataWhenDataIsSpecified()
        {
            var data = new object();
            var message = new PushMessage(Token, data: data);

            var payload = message.GetPayload();

            Assert.Equal(data, payload.data);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithExpirationWhenExpirationIsSpecified(int expiration)
        {
            var message = new PushMessage(Token, expiration: expiration);

            var payload = message.GetPayload();

            Assert.Equal(expiration, payload.expiration);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithTitleWhenTitleIsSpecified()
        {
            const string title = "title";
            var message = new PushMessage(Token, title: title);

            var payload = message.GetPayload();

            Assert.Equal(title, payload.title);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithTtlWhenTtlIsSpecified(int ttl)
        {
            var message = new PushMessage(Token, ttl: ttl);

            var payload = message.GetPayload();

            Assert.Equal(ttl, payload.ttl);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithSoundWhenSoundIsDefault()
        {
            const PushSounds sound = PushSounds.Default;
            var message = new PushMessage(Token, sound: sound);

            var payload = message.GetPayload();

            Assert.Equal(PushSounds.Default.ToString().ToLower(), payload.sound);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithoutSoundWhenSoundIsNone()
        {
            const PushSounds sound = PushSounds.None;
            var message = new PushMessage(Token, sound: sound);

            var payload = message.GetPayload();

            Assert.Throws<RuntimeBinderException>(() => payload.sound);
        }


        private const string Token = "ExponentPushToken[xxxxxxxxxxxxxxxxxxxxxx]";
    }
}
