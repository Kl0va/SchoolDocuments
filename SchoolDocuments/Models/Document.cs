using Newtonsoft.Json;
using SchoolDocuments.Converters;
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
        public string templateId { get; set; }
        public User author { get; set; }
        public string title { get; set; }
        [JsonConverter(typeof(BytesConverter))]
        public byte[] file { get; set; }
        public string desc { get; set; }
        public List<Familiarize> familiarize { get; set; }
        public List<Agreement> agreement { get; set; }

        [JsonConstructor]
        public Document(string TemplateId,User AuthorId,string Title,byte[] File,string Desc,List<Familiarize> Familiarize,List<Agreement> Agreement)
        {
            templateId = TemplateId;
            author = AuthorId;
            title = Title;
            file = File;
            desc = Desc;
            familiarize = Familiarize;
            agreement = Agreement;
        }
    }
}
