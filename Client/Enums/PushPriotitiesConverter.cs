using System;
using Newtonsoft.Json;

namespace Floxdc.ExponentServerSdk.Enums
{
    internal class PushPriotitiesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(PushPriotities);


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (((string) reader.Value).ToLower())
            {
                case "default":
                    return PushPriotities.Default;
                case "high":
                    return PushPriotities.High;
                case "normal":
                    return PushPriotities.Normal;
                default:
                    return PushPriotities.None;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((PushPriotities) value)
            {
                case PushPriotities.None:
                    break;
                case PushPriotities.Default:
                    writer.WriteValue("default");
                    break;
                case PushPriotities.High:
                    writer.WriteValue("high");
                    break;
                case PushPriotities.Normal:
                    writer.WriteValue("normal");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
