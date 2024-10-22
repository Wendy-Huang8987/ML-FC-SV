using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{
    [Table("user_info",Schema="public")]
    public class USERCLASS
    {
        [Key]
        public string uid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string CompanyOrOrganization { get; set;}
        public string Country { get; set; }
        public string Password { get; set;}


    }
}