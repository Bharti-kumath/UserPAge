using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Models
{
    public class ProfileViewModel
    {
        public UserViewModel UserDetail { get; set; }
        public List<Suggestion> Suggestions { get; set; }
        public List<MutualFriendViewModel> Friends { get; set; }
        public List<MutualFriendViewModel> MutualFriends { get; set; }

        public PostViewModel Post { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}