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
        public string userId { get; set; }
        public int documentId { get; set; }
        public bool familiarized { get; set; }
        public DateTime created { get; set; }

        [JsonConstructor]
        public Familiarize(string UserId,int DocumentId,bool Familiarized,DateTime Created)
        {
            userId = UserId;
            documentId = DocumentId;
            familiarized = Familiarized;
            created = Created;
        }
    }
}
