using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Json
{
    public class DataValueAttribute : JsonConverterAttribute
    {
        private bool isSvalue = false;
        public DataValueAttribute() : base(null) { }
        public DataValueAttribute(bool sValue) : base(null) { isSvalue = sValue; }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            return new DataValueConverter(isSvalue);
        }
    }

    public class DataValueConverter : JsonConverter<object>
    {
        private bool isSvalue = false;

        public DataValueConverter(bool sValue) : base() { isSvalue = sValue; }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (isSvalue == false)
            {
                writer.WriteStringValue(JsonSerializer.Serialize(value));
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteString("$", JsonSerializer.Serialize(value));
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type typeToConvert) => true;
    }
}
