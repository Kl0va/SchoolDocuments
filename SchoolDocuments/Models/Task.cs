using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Task
    {
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public DateTime created { get; set; }
        public DateTime deadline { get; set; }
        public User author { get; set; }
        public List<Performer> performs { get; set; }

        [JsonConstructor]
        public Task(string Title,string Desc,DateTime Created,DateTime Deadline,User AuthorId,List<Performer> Performers)
        {
            title = Title;
            desc = Desc;
            created = Created;
            deadline = Deadline;
            author = AuthorId;
            performs = Performers;
        }
    }
}
