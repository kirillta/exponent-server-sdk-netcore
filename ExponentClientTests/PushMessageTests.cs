using Floxdc.ExponentServerSdk;
using Floxdc.ExponentServerSdk.Enums;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Xunit;

namespace ExponentClientTests
{
    public class PushMessageTests
    {
        [Fact]
        public void GetPayload_ShouldReturnPayload()
        {
            var message = new PushMessage(Token);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);


            Assert.Equal(Token, payload.to.ToString());
            Assert.Null((PushSounds?) payload.sound);
            Assert.Equal(PushPriorities.Default, (PushPriorities) payload.priority);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithBadgeWhenBadgeIsSpecified(int badge)
        {
            var message = new PushMessage(Token, badge: badge);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(badge, (int) payload.badge);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithBodyWhenBodyIsSpecified()
        {
            const string body = "body";
            var message = new PushMessage(Token, body: body);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(body, (string) payload.body);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithDataWhenDataIsSpecified()
        {
            var data = new { id = 1};
            var jdata = JsonConvert.SerializeObject(data);
            var message = new PushMessage(Token, data: jdata);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(jdata, payload.data.ToString());
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithExpirationWhenExpirationIsSpecified(int expiration)
        {
            var message = new PushMessage(Token, expiration: expiration);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(expiration, (int) payload.expiration);
        }


        [Theory]
        [InlineData(PushPriorities.Default)]
        [InlineData(PushPriorities.High)]
        [InlineData(PushPriorities.Normal)]
        public void GetPayload_ShouldReturnPayloadWithSpecifiedPriority(PushPriorities priority)
        {
            var message = new PushMessage(Token, priority: priority);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(priority, (PushPriorities)payload.priority);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithTitleWhenTitleIsSpecified()
        {
            const string title = "title";
            var message = new PushMessage(Token, title: title);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(title, (string) payload.title);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPayload_ShouldReturnPayloadWithTtlWhenTtlIsSpecified(int ttl)
        {
            var message = new PushMessage(Token, ttl: ttl);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(ttl, (int) payload.ttl);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithSoundWhenSoundIsDefault()
        {
            const PushSounds sound = PushSounds.Default;
            var message = new PushMessage(Token, sound: sound);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Equal(PushSounds.Default.ToString().ToLower(), (string) payload.sound);
        }


        [Fact]
        public void GetPayload_ShouldReturnPayloadWithoutSoundWhenSoundIsNone()
        {
            var message = new PushMessage(Token, sound: PushSounds.None);

            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var payload = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.Throws<RuntimeBinderException>(() => (PushSounds) payload.sound);
        }


        private const string Token = "ExponentPushToken[xxxxxxxxxxxxxxxxxxxxxx]";
    }
}
