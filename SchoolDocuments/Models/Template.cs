using Newtonsoft.Json;
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
        public string file { get; set; }

        [JsonConstructor]
        public Template(string Name,string File)
        {
            name = Name;
            file = File;
        }
    }
}
