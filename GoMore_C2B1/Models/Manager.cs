using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{
    [Table("Manager", Schema = "public")]
    public class Manager
    {
        [Key]
        public string Email { get; set; }
        public string AccountType { get; set; }
    }
}