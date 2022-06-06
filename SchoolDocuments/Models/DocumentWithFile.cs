using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class DocumentWithFile
    {
        public Document document { get; set; }
        public File file { get; set; }

        public DocumentWithFile(Document Document, File File)
        {
            document = Document;
            file = File;
        }
    }
}
