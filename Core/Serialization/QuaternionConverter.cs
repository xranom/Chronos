using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{
    public class QuaternionConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(Quaternion).IsAssignableFrom(objectType);
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

            return new Quaternion(x, y, z, w);

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            Quaternion q = (Quaternion)value;
            writer.WriteStartArray();
            writer.WriteRaw(string.Format("{0}, {1}, {2}, {3}", q.X, q.Y, q.Z, q.W));
            writer.WriteEndArray();

        }

    }

}
