using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{

    public class Vector3Converter : JsonConverter
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

            // Read end array token:
            reader.Read();

            return new Vector3(x, y, z);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 v = (Vector3)value;
            writer.WriteStartArray();
            writer.WriteRaw(string.Format("{0}, {1}, {2}", v.X, v.Y, v.Z));
            writer.WriteEndArray();
        }

    }

}
