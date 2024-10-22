using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{

    public class DrawingsFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string Tag { get; set; }

    }
    public class DocumentsFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string Tag { get; set; }

    }

    public class ModelsFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string Tag { get; set; }

    }

    public class OthersFile
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string Tag { get; set; }

    }

    public class totalFile
    { 
        public string DrawingsFile { get; set; }
        public string DocumentsFile { get; set; }
        public string ModelsFile { get; set; }
        public string OthersFile { get; set; }

    }

}