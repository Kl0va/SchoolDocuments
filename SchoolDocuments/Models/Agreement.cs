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
        public Document document { get; set; }
        public DateTime deadline { get; set; }
        public AgreementStatus status { get; set; }
        public string comment { get; set; }
        public  DateTime created { get; set; } 

        [JsonConstructor]
        public Agreement(string UserId,Document DocumentId,DateTime Deadline,AgreementStatus Status,string Comment,DateTime Created)
        {
            userId = UserId;
            document = DocumentId;
            deadline = Deadline;
            status = Status;
            comment = Comment;
            created = Created;
            //statusChanged = StatusChanged;
        }
    }
}
