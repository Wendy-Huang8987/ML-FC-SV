using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{
    [Table("MainModelDB",Schema="public")]
    public class ModelNameDB
    {
        [Key]
        public string ID { get; set; }
        public string ProjectName { get; set; }
        public string Author { get; set; }
        public string Permit { get; set; }
    }
}