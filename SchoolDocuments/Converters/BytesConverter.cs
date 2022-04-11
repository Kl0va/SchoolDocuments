using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Converters
{
    class BytesConverter : JsonConverter<byte[]>
    {
        public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (byte valueByte in value)
            {
                if (valueByte > 127)
                    writer.WriteValue(valueByte - 256);
                else
                    writer.WriteValue(valueByte);
            }
            writer.WriteEndArray();
        }

        public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<long> longs = new List<long>();

            reader.Read();
            while (reader.TokenType != JsonToken.EndArray)
            {
                longs.Add((long)reader.Value);
                reader.Read();
            }
            int[] ints = (from value in longs select int.Parse(value.ToString())).ToArray();

            byte[] bytes = new byte[ints.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                bytes[i] = (byte)(ints[i] & 0xFF);
            }

            return bytes;
        }

    }
}
