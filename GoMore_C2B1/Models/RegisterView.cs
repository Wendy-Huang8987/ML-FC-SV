using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoMore_C2B1.Models
{
    public class RegisterView
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string CompanyOrOrganization { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Confitmpassword { get; set; }
    }
}