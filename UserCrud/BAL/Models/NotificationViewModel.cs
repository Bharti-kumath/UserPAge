using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class NotificationViewModel
    {
        public int id { get; set; }
        public byte NotificationType { get; set; }
        public string UserName { get; set; }
        public int PostID { get; set; }

        public DateTime Created_At { get; set; }

        public bool IsRead { get; set; }

        public int FromUserID { get; set; }

    }
}