using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class ProfileViewModel
    {
        public UserViewModel UserDetail { get; set; }
        public Suggestion Suggestions { get; set; }

    }
}