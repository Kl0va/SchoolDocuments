using Newtonsoft.Json;
using SchoolDocuments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Converters
{
    class PerformStatusConv : JsonConverter<PerformerStatus>
    {
        public override PerformerStatus ReadJson(JsonReader reader, Type objectType, PerformerStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (PerformerStatus)Enum.Parse(typeof(PerformerStatus), reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, PerformerStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
