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
        public int authorId { get; set; }
        public List<Performer> performers { get; set; }

        [JsonConstructor]
        public Task(string Title,string Desc,DateTime Created,DateTime Deadline,int AuthorId,List<Performer> Performers)
        {
            title = Title;
            desc = Desc;
            created = Created;
            deadline = Deadline;
            authorId = AuthorId;
            performers = Performers;
        }
    }
}
