using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Role
    {
        public string name { get; set; }

        [JsonConstructor]
        public Role(string Name)
        {
            name = Name;
        }
    }
}
