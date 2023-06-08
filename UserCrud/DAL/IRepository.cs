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

        UserViewModel CheckUser(string email, string password);
        void GeCSVFile(FilterOptions filterOptions);

        ProfileViewModel GetProfileById(long id);
        ProfileViewModel GetFriendProfileById(long userId, long friendId);
        void SavePost(PostViewModel model,long userID);
        void DeletePost(long postID);
        List<PostViewModel> GetAllPosts(long userID);
        PostViewModel LikePost(long postID, long userID);
        List<CommentViewModel> GetCommentsByPostId(long id);
        CommentViewModel SaveComment(long userID, long postID, string commentText);
        CommentViewModel DeleteComment(long commentID, long postId);
        void FollowRequest(long userID, long toUserId);
    }
}
