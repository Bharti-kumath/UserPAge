using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comment_Text { get; set; }
        public string UserName { get; set; }
        public DateTime Created_At { get; set; }
        public int TotalComments  { get; set; }
        public int User_id { get; set; }
        public int Post_id { get; set; }


    }
}