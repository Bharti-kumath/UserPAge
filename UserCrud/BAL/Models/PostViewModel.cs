using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class PostViewModel
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }
        public int TotalViews { get; set; }
        public List<string> MediaPaths { get; set; }
        public string Path { get; set; }
        public List<HttpPostedFileBase> ImagePath { get; set; }

        public int TotalPost { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComments { get; set; }
        public int Liked { get; set; }
        public DateTime? Created_At { get; set; } = DateTime.Now;
      
    }
}