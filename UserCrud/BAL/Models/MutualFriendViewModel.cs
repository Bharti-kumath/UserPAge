using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class MutualFriendViewModel
    {
        public int Id { get; set; }
        public int TotalMutual { get; set; } = 0;
        public string MutualFriendName { get; set; }
    }
}