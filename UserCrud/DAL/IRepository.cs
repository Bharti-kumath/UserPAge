using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public interface IRepository
    {
        List<BAL.Models.UserViewModel> GetUSerDetails();
        void SaveUserDetails(UserViewModel model);
        void deleteUserDetails(long userID);
        List<BAL.Models.UserViewModel> GetUserDetails(FilterOptions filterOptions);
        UserViewModel GetUserById(long id);

        UserViewModel checkUser(string email, string password);
        void GeCSVFile(FilterOptions filterOptions);

        ProfileViewModel GetProfileById(long id);
        ProfileViewModel GetFriendProfileById(long userId, long friendId);
        void SavePost(PostViewModel model,long userID);
        void deletePost(long postID);
        List<PostViewModel> GetAllPosts();
    }
}
