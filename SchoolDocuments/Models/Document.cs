using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class Document
    {
        public int id { get; set; }
        public int templateId { get; set; }
        public int authorId { get; set; }
        public string title { get; set; }
        public string file { get; set; }
        public string desc { get; set; }
        public List<Familiarize> familiarize { get; set; }
        public List<Agreement> agreement { get; set; }

        [JsonConstructor]
        public Document(int TemplateId,int AuthorId,string Title,string File,string Desc,List<Familiarize> Familiarize,List<Agreement> Agreement)
        {
            templateId = TemplateId;
            authorId = AuthorId;
            title = Title;
            file = File;
            desc = Desc;
            familiarize = Familiarize;
            agreement = Agreement;
        }
    }
}
