using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{
    [Table("LinkFileModel",Schema="public")]
    public class LinkFileModel
    {
        [Key]
        public string ID { get; set; }
        public string UnitFileName { get; set; }
        public string MainModel { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }

    }

    [Table("LinkFileUrl", Schema = "public")]
    public class LinkFileUrl
    {
        [Key]
        public string ID { get; set; }
        public string LinksUnit { get; set; }
        public string Tag { get; set; }
        public string FileFolder { get; set; }
        public string ProjectName { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }

    }
    [Table("SVModelControl", Schema = "public")]
    public class SVModelControl
    {
        [Key]
        public string ID { get; set; }
        public string Author { get; set; }
        public string ModelName { get; set; }
        public string Permit { get; set; }
        public string ProjectName { get; set; }
        public string ImageUrl { get; set; }
        public string ModelType { get; set; }

    }

}