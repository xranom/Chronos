using Chronos.Core.Component;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{

    public class Vector4Converter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector3).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            // Read values:
            float x = float.Parse(reader.ReadAsString());
            float y = float.Parse(reader.ReadAsString());
            float z = float.Parse(reader.ReadAsString());
            float w = float.Parse(reader.ReadAsString());

            // Read end array token:
            reader.Read();

            return new Vector4(x, y, z, w);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector4 v = (Vector4)value;
            writer.WriteStartArray();
            writer.WriteRaw(string.Format("{0}, {1}, {2}, {3}", v.X, v.Y, v.Z, v.W));
            writer.WriteEndArray();
        }

    }

}
