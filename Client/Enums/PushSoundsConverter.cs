using System;
using Newtonsoft.Json;

namespace Floxdc.ExponentServerSdk.Enums
{
    internal class PushSoundsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(PushSounds);


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (((string)reader.Value).ToLower())
            {
                case "default":
                    return PushSounds.Default;
                default:
                    return PushSounds.None;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((PushSounds)value)
            {
                case PushSounds.None:
                    writer.WriteNull();
                    break;
                case PushSounds.Default:
                    writer.WriteValue(PushSounds.Default.ToString().ToLower());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
