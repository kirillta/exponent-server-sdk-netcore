using System;
using System.Dynamic;
using Floxdc.ExponentServerSdk.Enums;

namespace Floxdc.ExponentServerSdk
{
    /// <summary>
    /// An object that describes a push notification request.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public class PushMessage
    {
        public PushMessage(string to, object data = null, string title = null, string body = null,
            PushSounds sound = PushSounds.None, int? ttl = null,
            int? expiration = null, PushPriotities priority = PushPriotities.Default, int? badge = null)
        {
            To = to;
            Data = data;
            Title = title;
            Body = body;
            Sound = sound;
            Ttl = ttl;
            Expiration = expiration;
            Priority = priority;
            Badge = badge;
        }


        public virtual dynamic GetPayload()
        {
            if (!PushClient.IsExponentPushToken(To))
                throw new ArgumentException("Invalid push token");

            dynamic payload = new ExpandoObject();
            payload.to = To;
            payload.priority = Priority.ToString().ToLower();

            if (Badge != null)
                payload.badge = Badge.Value;

            if (!string.IsNullOrEmpty(Body))
                payload.body = Body;

            if (Data != null)
                payload.data = Data;

            if (Expiration != null)
                payload.expiration = Expiration.Value;

            if (!string.IsNullOrEmpty(Title))
                payload.title = Title;

            if (Ttl != null)
                payload.ttl = Ttl.Value;

            switch (Sound)
            {
                case PushSounds.None:
                    break;
                case PushSounds.Default:
                    payload.sound = PushSounds.Default.ToString().ToLower();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return payload;
        }


        /// <summary>
        /// An integer representing the unread notification count. This 
        /// currently only affects iOS. Specify 0 to clear the badge count.
        /// </summary>
        public int? Badge { get; private set; }

        /// <summary>
        /// The message to display in the notification.
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// A dict of extra data to pass inside of the push notification. 
        /// The total notification payload must be at most 4096 bytes.
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// UNIX timestamp for when this message expires. It has 
        /// the same effect as ttl, and is just an absolute timestamp 
        /// instead of a relative one.
        /// </summary>
        public int? Expiration { get; private set; }

        /// <summary>
        /// Delivery priority of the message. 'default', 'normal', 
        /// and 'high' are the only valid values.
        /// </summary>
        public PushPriotities Priority { get; private set; }

        /// <summary>
        /// A sound to play when the recipient receives this 
        /// notification. Specify "default" to play the device's default 
        /// notification sound, or omit this field to play no sound.
        /// </summary>
        public PushSounds Sound { get; private set; }

        /// <summary>
        /// The title to display in the notification. On iOS, this is 
        /// displayed only on Apple Watch.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The number of seconds for which the message may be kept around 
        /// for redelivery if it hasn't been delivered yet. Defaults to 0.
        /// </summary>
        public int? Ttl { get; private set; }

        /// <summary>
        /// A token of the form ExponentPushToken[xxxxxxx].
        /// </summary>
        public string To { get; private set; }
    }
}
