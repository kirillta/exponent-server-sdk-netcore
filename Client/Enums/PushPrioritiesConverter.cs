using Newtonsoft.Json;
using System;

namespace Floxdc.ExponentServerSdk.Enums
{
    internal class PushPrioritiesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(PushPriorities);


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (((string) reader.Value).ToLower())
            {
                case "default":
                    return PushPriorities.Default;
                case "high":
                    return PushPriorities.High;
                case "normal":
                    return PushPriorities.Normal;
                default:
                    return PushPriorities.None;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((PushPriorities) value)
            {
                case PushPriorities.None:
                    break;
                case PushPriorities.Default:
                    writer.WriteValue("default");
                    break;
                case PushPriorities.High:
                    writer.WriteValue("high");
                    break;
                case PushPriorities.Normal:
                    writer.WriteValue("normal");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
