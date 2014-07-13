using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{

    public class Matrix4Converter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(Matrix4).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            // Read values:
            float m11 = float.Parse(reader.ReadAsString());
            float m12 = float.Parse(reader.ReadAsString());
            float m13 = float.Parse(reader.ReadAsString());
            float m14 = float.Parse(reader.ReadAsString());
            float m21 = float.Parse(reader.ReadAsString());
            float m22 = float.Parse(reader.ReadAsString());
            float m23 = float.Parse(reader.ReadAsString());
            float m24 = float.Parse(reader.ReadAsString());
            float m31 = float.Parse(reader.ReadAsString());
            float m32 = float.Parse(reader.ReadAsString());
            float m33 = float.Parse(reader.ReadAsString());
            float m34 = float.Parse(reader.ReadAsString());
            float m41 = float.Parse(reader.ReadAsString());
            float m42 = float.Parse(reader.ReadAsString());
            float m43 = float.Parse(reader.ReadAsString());
            float m44 = float.Parse(reader.ReadAsString());

            // Read end array token:
            reader.Read();

            return new Matrix4(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44);

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Matrix4 m = (Matrix4)value;

            writer.WriteStartArray();
            writer.WriteRaw(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}",
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44));
            writer.WriteEndArray();
        }

    }

}
