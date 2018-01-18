using Newtonsoft.Json;

namespace Floxdc.ExponentServerSdk.Enums
{
    /// <summary>
    /// A sound to play when the recipient receives this 
    /// notification. Specify "default" to play the device's default 
    /// notification sound, or omit this field to play no sound.
    /// </summary>
    [JsonConverter(typeof(PushSoundsConverter))]
    public enum PushSounds
    {
        None = 0,
        Default = 1
    }
}
