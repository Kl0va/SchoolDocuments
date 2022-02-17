using Newtonsoft.Json;
using SchoolDocuments.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Template
    {
        public string name { get; set; }

        [JsonConverter(typeof(BytesConverter))]
        public byte[] file { get; set; }

        [JsonConstructor]
        public Template(string Name,byte[] File)
        {
            name = Name;
            file = File;
        }
    }
}
