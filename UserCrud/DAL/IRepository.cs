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
        PostViewModel EditPost(long postId);
        void DeletePost(long postID);
        List<PostViewModel> GetAllPosts(long userID);
        PostViewModel LikePost(long postID, long userID, long postUserID);
        List<PostViewModel> GetScheduledPost(DateTime currentTime);
        List<CommentViewModel> GetCommentsByPostId(long id);
        void PublishPost(long postId);
        CommentViewModel SaveComment(long userID, long postID, string commentText,long postUserID, long? toUserID);
        CommentViewModel DeleteComment(long commentID, long postId);
        void FollowRequest(long userID, long toUserId);
        List<NotificationViewModel> GetNotificationByID(long userID);
        void changeNotificationStatus(long id);
        int GetNotificationCount(long userId);
        void UpdateFollowRequest(long followerId, long followingID, byte action);
        CommentViewModel SaveCommentReply(long commentId, long userID, string replyText, long toUserId);
        List<ReplyViewModel> GetReplyByCommentID(long commenId);
        List<Suggestion> GetLikeUserList(long postId);
        List<Suggestion> SearchUser(string userName, long userID);
        void SharePost(long postId, long toUserId, long fromUserID);
    }
}
