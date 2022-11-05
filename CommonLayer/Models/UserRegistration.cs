using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class UserRegistration
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Createat { get; set; }
        public DateTime? Modifiedat { get; set; }
    }
}
