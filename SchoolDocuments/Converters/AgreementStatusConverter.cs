using Newtonsoft.Json;
using SchoolDocuments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Converters
{
    class AgreementStatusConverter : JsonConverter<AgreementStatus>
    {
        public override AgreementStatus ReadJson(JsonReader reader, Type objectType, AgreementStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (AgreementStatus)Enum.Parse(typeof(AgreementStatus), reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, AgreementStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
