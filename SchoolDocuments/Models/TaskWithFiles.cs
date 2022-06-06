using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class TaskWithFiles
    {
        public Models.Task task { get; set; }
        public List<Models.File> files { get; set; }

        [JsonConstructor]
        public TaskWithFiles(Models.Task Task, List<Models.File> Files)
        {
            task = Task;
            files = Files;
        }
    }
}
