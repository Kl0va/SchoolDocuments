using Newtonsoft.Json;
using SchoolDocuments.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class File
    {
        public int id { get; set; }
        public string name { get; set; }
        public float size { get; set; }
        [JsonConverter(typeof(BytesConverter))]
        public byte[] file { get; set; }
        public string extension { get; set; }

        [JsonConstructor]
        public File(string Name,byte[] File)
        {
            name = Name;
            file = File;
            extension = "rtf";
        }
    }
}
