using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class UserViewModel
    {
        public int ID { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Email { get; set; }
        public string PhoneNUmber { get; set; }
        public DateTime DAteOfBirth { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public int PinCode { get; set; }
        public string Address { get; set; }





    }
}