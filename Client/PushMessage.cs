using Floxdc.ExponentServerSdk.Enums;
using Newtonsoft.Json;

namespace Floxdc.ExponentServerSdk
{
    /// <summary>
    /// An object that describes a push notification request.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PushMessage
    {
        public PushMessage(string to, object data = null, string title = null, string body = null, PushSounds sound = PushSounds.None, int? ttl = null,
            int? expiration = null, PushPriotities priority = PushPriotities.Default, int? badge = null, string category = null, bool displayInForeground = false,
            string channelId = null)
        {
            Badge = badge;
            Body = body;
            Category = category;
            ChannelId = channelId;
            Data = data;
            DisplayInForeground = displayInForeground;
            Expiration = expiration;
            Priority = priority;
            Sound = sound;
            Title = title;
            To = to;
            Ttl = ttl;
        }


        /// <summary>
        /// An integer representing the unread notification count. This 
        /// currently only affects iOS. Specify 0 to clear the badge count.
        /// </summary>
        [JsonProperty("badge")]
        public int? Badge { get; private set; }

        /// <summary>
        /// The message to display in the notification.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; private set; }

        /// <summary>
        /// ID of the Notification Category through which to display this notification.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; private set; }

        /// <summary>
        /// ID of the Notification Channel through which to display this notification
        /// on Android devices.If an ID is specified but the corresponding channel
        /// does not exist on the device(i.e.has not yet been created by your app),
        /// the notification will not be displayed to the user.
        /// </summary>
        [JsonProperty("channelId")]
        public string ChannelId { get; private set; }

        /// <summary>
        /// A dict of extra data to pass inside of the push notification. 
        /// The total notification payload must be at most 4096 bytes.
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; private set; }

        /// <summary>
        /// Displays the notification when the app is foregrounded. Defaults to `false`.
        /// </summary>
        [JsonProperty("display_in_foreground")]
        public bool DisplayInForeground { get; private set; }

        /// <summary>
        /// UNIX timestamp for when this message expires. It has 
        /// the same effect as ttl, and is just an absolute timestamp 
        /// instead of a relative one.
        /// </summary>
        [JsonProperty("expiration")]
        public int? Expiration { get; private set; }

        /// <summary>
        /// Delivery priority of the message. 'default', 'normal', 
        /// and 'high' are the only valid values.
        /// </summary>
        [JsonProperty("priority")]
        public PushPriotities Priority { get; private set; }

        /// <summary>
        /// A sound to play when the recipient receives this 
        /// notification. Specify "default" to play the device's default 
        /// notification sound, or omit this field to play no sound.
        /// </summary>
        [JsonProperty("sound")]
        public PushSounds Sound { get; private set; }

        /// <summary>
        /// The title to display in the notification. On iOS, this is 
        /// displayed only on Apple Watch.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// The number of seconds for which the message may be kept around 
        /// for redelivery if it hasn't been delivered yet. Defaults to 0.
        /// </summary>
        [JsonProperty("ttl")]
        public int? Ttl { get; private set; }

        /// <summary>
        /// A token of the form ExponentPushToken[xxxxxxx].
        /// </summary>
        [JsonProperty("to")]
        public string To { get; private set; }
    }
}
