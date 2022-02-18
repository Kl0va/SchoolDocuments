using Newtonsoft.Json;
using SchoolDocuments.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Agreement
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int documentId { get; set; }
        public DateTime deadline { get; set; }
        [JsonConverter(typeof(AgreementStatusConverter))]
        public AgreementStatus status { get; set; }
        public string comment { get; set; }
        public  DateTime created { get; set; } 
        //public DateTime? statusChanged { get; set; } 

        [JsonConstructor]
        public Agreement(string UserId,int DocumentId,DateTime Deadline,AgreementStatus Status,string Comment,DateTime Created)
        {
            userId = UserId;
            documentId = DocumentId;
            deadline = Deadline;
            status = Status;
            comment = Comment;
            created = Created;
            //statusChanged = StatusChanged;
        }
    }
}
