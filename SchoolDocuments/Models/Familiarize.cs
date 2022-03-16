using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Familiarize
    {
        public int id { get; set; }
        public User user { get; set; }
        public Document document { get; set; }
        public bool familiarized { get; set; }
        public DateTime created { get; set; }

        [JsonConstructor]
        public Familiarize(User UserId,Document DocumentId,bool Familiarized,DateTime Created)
        {
            user = UserId;
            document = DocumentId;
            familiarized = Familiarized;
            created = Created;
        }
    }
}
