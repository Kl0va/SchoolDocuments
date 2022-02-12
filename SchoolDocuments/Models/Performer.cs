using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Performer
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int taskId { get; set; }
        public PerformerStatus status { get; set; }
        public string comment { get; set; }

        [JsonConstructor]
        public Performer(int UserId, int TaskId, PerformerStatus Status, string Comment)
        {
            userId = UserId;
            taskId = TaskId;
            status = Status;
            comment = Comment;
        }
    }
}
