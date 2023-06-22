using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class ReplyViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string ToUserName { get; set; }
        public DateTime Created_At { get; set; }
        public string ReplyText { get; set; }

    }
}