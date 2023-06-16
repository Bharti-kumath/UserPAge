
using BAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace DAL.Repository
{
    public class Repository : IRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        public void deleteUserDetails(long userID)
        {

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", userID, DbType.Int64);

                connection.Execute("sp_UserDeleteOpertaion", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<UserViewModel> GetUSerDetails()
        {


            List<BAL.Models.UserViewModel> UserList = new List<BAL.Models.UserViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", DBNull.Value, DbType.String);
                //parameters.Add("first_name", DBNull.Value, DbType.String);
                //parameters.Add("last_name", DBNull.Value, DbType.String);
                //parameters.Add("email", DBNull.Value, DbType.String);
                //parameters.Add("phone_number", DBNull.Value, DbType.String);
                //parameters.Add("country", DBNull.Value, DbType.String);
                //parameters.Add("city", DBNull.Value, DbType.String);
                //parameters.Add("pincode", DBNull.Value, DbType.Int32);
                //parameters.Add("dob", DBNull.Value, DbType.String);
                //parameters.Add("address", DBNull.Value, DbType.String);
                //parameters.Add("queryType", "Select", DbType.String);

                UserList = connection.Query<UserViewModel>("sp_UserGetOpertaion", parameters, commandType: CommandType.StoredProcedure).ToList();
            }


            return UserList;
        }

        private string Encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();

            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();

            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }

            return encryptdata.ToString();
        }
        public void SaveUserDetails(UserViewModel model)
        {

            var passwordHash = Encryption(model.Password);
            var passwordHashed = passwordHash + "As@";
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", model.ID, DbType.Int64);
                parameters.Add("first_name", model.FirstName, DbType.String);
                parameters.Add("last_name", model.LastName, DbType.String);
                parameters.Add("email", model.Email, DbType.String);
                parameters.Add("phone_number", model.PhoneNUmber, DbType.String);
                parameters.Add("country", model.Country, DbType.String);
                parameters.Add("city", model.City, DbType.String);
                parameters.Add("pincode", model.PinCode, DbType.Int32);
                parameters.Add("dob", model.DAteOfBirth, DbType.String);
                parameters.Add("address", model.Address, DbType.String);
                parameters.Add("password", passwordHashed, DbType.String);


                connection.Execute("sp_UserInsertUpdate", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<UserViewModel> GetUserDetails(FilterOptions filterOptions)
        {

            List<UserViewModel> userList = new List<UserViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", DBNull.Value, DbType.String);
                parameters.Add("@FirstName", filterOptions.FirstName, DbType.String);
                parameters.Add("@LastName", filterOptions.LastName, DbType.String);
                parameters.Add("@Country", filterOptions.Country, DbType.String);
                parameters.Add("@City", filterOptions.City, DbType.String);
                parameters.Add("@FromDate", filterOptions.FromDate, DbType.DateTime);
                parameters.Add("@ToDate", filterOptions.ToDate, DbType.DateTime);
                parameters.Add("@SortColumn", filterOptions.SortColumn, DbType.String);
                parameters.Add("@SortOrder", filterOptions.SortDirection, DbType.String);
                parameters.Add("@PageNumber", filterOptions.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", filterOptions.PageSize, DbType.Int32);
                parameters.Add("@export", filterOptions.export, DbType.Int32);

                userList = connection.Query<UserViewModel>("sp_UserFiltering", parameters, commandType: CommandType.StoredProcedure).ToList();
            }



            return userList;
        }

        public UserViewModel GetUserById(long id)
        {

            UserViewModel userByID = new UserViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int64);
                userByID = connection.QueryFirstOrDefault<UserViewModel>("GetUserById", parameters, commandType: CommandType.StoredProcedure);

            }

            return userByID;
        }

        public void GeCSVFile(FilterOptions filterOptions)
        {
            List<ExportViewModel> userList = new List<ExportViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", DBNull.Value, DbType.String);
                parameters.Add("@FirstName", filterOptions.FirstName, DbType.String);
                parameters.Add("@LastName", filterOptions.LastName, DbType.String);
                parameters.Add("@Country", filterOptions.Country, DbType.String);
                parameters.Add("@City", filterOptions.City, DbType.String);
                parameters.Add("@FromDate", filterOptions.FromDate, DbType.DateTime);
                parameters.Add("@ToDate", filterOptions.ToDate, DbType.DateTime);
                parameters.Add("@SortColumn", filterOptions.SortColumn, DbType.String);
                parameters.Add("@SortOrder", filterOptions.SortDirection, DbType.String);
                parameters.Add("@PageNumber", filterOptions.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", filterOptions.PageSize, DbType.Int32);
                parameters.Add("@export", filterOptions.export, DbType.Int32);

                userList = connection.Query<ExportViewModel>("sp_UserFiltering", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            StringBuilder csvData = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            foreach (ExportViewModel user in userList)
            {
                headers = new StringBuilder();
                var type = typeof(ExportViewModel);
                var properties = type.GetProperties();
                foreach (PropertyInfo prop in typeof(ExportViewModel).GetProperties())
                {
                    var props = prop;
                    var users = prop.GetValue(user)?.ToString();
                    csvData.Append(prop.GetValue(user)?.ToString() + ",");
                    headers.Append(prop.Name + ",");
                }

                csvData.Append("\r\n");
                headers.Append("\r\n");
            }

            string contentToExport = headers.Append(csvData.ToString()).ToString();
            string attachment = "attachment; filename=export.csv";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.Write(contentToExport);
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        public UserViewModel CheckUser(string email, string password)
        {
            UserViewModel userExist;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var modelPassword = Encryption(password);
                modelPassword = modelPassword + "As@";
                var parameters = new DynamicParameters();
                parameters.Add("@email", email, DbType.String);
                parameters.Add("@password", modelPassword, DbType.String);
                userExist = connection.QueryFirstOrDefault<UserViewModel>("sp_checkUser", parameters, commandType: CommandType.StoredProcedure);
            }
            return userExist;



        }

        public ProfileViewModel GetProfileById(long id)
        {
            ProfileViewModel profileByID = new ProfileViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int64);
                var reader = connection.QueryMultiple("sp_getUserProfile", parameters, commandType: CommandType.StoredProcedure);
                profileByID.UserDetail = reader.ReadFirst<UserViewModel>();
                profileByID.Suggestions = reader.Read<Suggestion>().ToList();
                profileByID.Friends = reader.Read<MutualFriendViewModel>().ToList();
                profileByID.Posts = reader.Read<PostViewModel>().ToList();

                List<Media> mediaPaths = connection.Query<Media>("SELECT postid, mediapath FROM media WHERE postid IN @PostIDs", new { PostIDs = profileByID.Posts.Select(post => post.ID) }, commandType: CommandType.Text).ToList();

                foreach (var post in profileByID.Posts)
                {
                    List<Media> mediaList = mediaPaths.Where(media => media.PostID == post.ID).ToList();
                    post.MediaPaths = mediaList.Select(media => media.MediaPath).ToList();
                }
            }
            return profileByID;
        }

        public ProfileViewModel GetFriendProfileById(long userId, long friendId)
        {
            ProfileViewModel profileByID = new ProfileViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", userId, DbType.Int64);
                parameters.Add("@FriendID", friendId, DbType.Int64);
                var reader = connection.QueryMultiple("sp_getFriendProfile", parameters, commandType: CommandType.StoredProcedure);
                profileByID.UserDetail = reader.ReadFirst<UserViewModel>();
                profileByID.MutualFriends = reader.Read<MutualFriendViewModel>().ToList();
                profileByID.Posts = reader.Read<PostViewModel>().ToList();

                List<Media> mediaPaths = connection.Query<Media>("SELECT postid, mediapath FROM media WHERE postid IN @PostIDs", new { PostIDs = profileByID.Posts.Select(post => post.ID) }, commandType: CommandType.Text).ToList();

                foreach (var post in profileByID.Posts)
                {
                    List<Media> mediaList = mediaPaths.Where(media => media.PostID == post.ID).ToList();
                    post.MediaPaths = mediaList.Select(media => media.MediaPath).ToList();
                }
            }

                return profileByID;
        }

        public void SavePost(PostViewModel model, long userID)
        {

            string path = HttpContext.Current.Server.MapPath("~/Content/PostImages");
            int status = (model.Created_At.HasValue && model.Created_At > DateTime.Now) ? 0 : 1;



            using (IDbConnection connection = new SqlConnection(connectionString))
            {
              
                DataTable imagesTable = new DataTable();
                imagesTable.Columns.Add("ImagePath", typeof(string));

                foreach (HttpPostedFileBase file in model.ImagePath)
                {
                  
                    var filename = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(path, filename);
                    file.SaveAs(filePath);
                    imagesTable.Rows.Add(filename);
                }

               
                var parameters = new DynamicParameters();
                parameters.Add("UserID", userID, DbType.Int32);
                parameters.Add("ID", model.ID, DbType.Int32);
                parameters.Add("Body", model.Body, DbType.String);
                parameters.Add("status", status, DbType.Boolean);
                parameters.Add("CreatedAt", model.Created_At, DbType.DateTime);
                parameters.Add("Images", imagesTable.AsTableValuedParameter("dbo.ImageTableType")); 

                connection.Execute("sp_InsertPost", parameters, commandType: CommandType.StoredProcedure);

                
            }


        }

        public PostViewModel EditPost(long postId)
        {
            PostViewModel postByID = new PostViewModel();

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ID", postId, DbType.Int64);
                postByID = connection.QueryFirstOrDefault<PostViewModel>("SELECT * FROM posts WHERE id = @PostID", new { PostID = postId }, commandType: CommandType.Text);

                postByID.MediaPaths = connection.Query<string>("SELECT mediapath FROM media WHERE postid = @PostID", new { PostID = postId }, commandType: CommandType.Text).ToList();

                    
                }
                return postByID;
            
        }
        public void DeletePost(long postID)
        {

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", postID, DbType.Int64);
                connection.Execute("sp_PostDeleteOpertaion", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public void SharePost(long postId, long toUserId, long fromUserID)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = @"
            INSERT INTO notification (fromUserId, toUserId, notificationType, postID)
            VALUES (@fromUserId, @toUserId, 5, @postId)";

                connection.Execute(insertQuery, new { fromUserID, toUserId, postId });
            }
        }
        public List<PostViewModel> GetAllPosts(long userID)
        {

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userID", userID, DbType.Int64);


                List<PostViewModel> posts = connection.Query<PostViewModel>("sp_GetAllPosts", parameters, commandType: CommandType.StoredProcedure).ToList();

                List<Media> mediaPaths = connection.Query<Media>("SELECT postid, mediapath FROM media", commandType: CommandType.Text).ToList();

                foreach (var post in posts)
                {
                    List<Media> mediaList = mediaPaths.Where(media => media.PostID == post.ID).ToList();
                    post.MediaPaths = mediaList.Select(media => media.MediaPath).ToList();
                }

                return posts;
            }

        }

        public PostViewModel LikePost(long postID, long userID, long postUserID)
        {
            PostViewModel totallikes;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("PostID", postID, DbType.Int64);
                parameters.Add("UserID", userID, DbType.Int64);
                parameters.Add("postUserID", postUserID, DbType.Int64);
                totallikes = connection.QueryFirstOrDefault<PostViewModel>("sp_LikeUnlikePost", parameters, commandType: CommandType.StoredProcedure);
            }
            return totallikes;
        }

        public List<Suggestion> GetLikeUserList(long postId)
        {
            List<Suggestion> likeUserList;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("PostID", postId, DbType.Int64);
                likeUserList = connection.Query<Suggestion>("sp_GetLikeUserList", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return likeUserList;
        }

        public List<CommentViewModel> GetCommentsByPostId(long id)
        {
            List<CommentViewModel> totalComments;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("PostID", id, DbType.Int64);
                totalComments = connection.Query<CommentViewModel>("sp_GetCommentsByPost", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return totalComments;
        }

        public CommentViewModel SaveComment(long userID, long postID, string commentText, long postUserID , long? toUserID)
        {
            CommentViewModel comment;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("PostID", postID, DbType.Int64);
                parameters.Add("UserID", userID, DbType.Int64);
                parameters.Add("comment", commentText, DbType.String);
                parameters.Add("postUserID", postUserID, DbType.Int64);
                parameters.Add("toUserID", toUserID, DbType.Int64);
                comment = connection.QueryFirstOrDefault<CommentViewModel>("sp_SaveComment", parameters, commandType: CommandType.StoredProcedure);
            }
            return comment;
        }

        public CommentViewModel SaveCommentReply(long commentId, long UserID,string replyText,long toUserId)
        {
            CommentViewModel comment;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("commentId", commentId, DbType.Int64);
                parameters.Add("UserID", UserID, DbType.Int64);
                parameters.Add("toUserId", toUserId, DbType.Int64);
                parameters.Add("replyText", replyText, DbType.String);
                comment = connection.QueryFirstOrDefault<CommentViewModel>("sp_SaveCommentReply", parameters, commandType: CommandType.StoredProcedure);
            }
            return comment;
        }

        public List<ReplyViewModel> GetReplyByCommentID(long commenId)
        {
            List<ReplyViewModel> totalReplies;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("commentId", commenId, DbType.Int64);
                totalReplies = connection.Query<ReplyViewModel>("sp_GetReplyByCommentID", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return totalReplies;
        }

        public CommentViewModel DeleteComment(long commentID, long postID)
        {
            CommentViewModel comment;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("CommentID", commentID, DbType.Int64);
                parameters.Add("PostID", postID, DbType.Int64);
                comment = connection.QueryFirstOrDefault<CommentViewModel>("sp_DeleteComment", parameters, commandType: CommandType.StoredProcedure);
            }
            return comment;
        }

        public void FollowRequest(long userID, long toUserId)
        {

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("FollowerID", userID, DbType.Int64);
                parameters.Add("FollowingID", toUserId, DbType.Int64);
                connection.Execute("sp_FollowRequest", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<NotificationViewModel> GetNotificationByID(long userID)
        {
            List<NotificationViewModel> totalNotifications;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserID", userID, DbType.Int64);
                totalNotifications = connection.Query<NotificationViewModel>("sp_GetNotificationByID", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return totalNotifications;
        }

        public void changeNotificationStatus(long id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", id, DbType.Int64);
                connection.Execute("sp_ReadNotificationByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int GetNotificationCount(long userId)
        {
            int count = 0;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("userId", userId, DbType.Int64);
                var result = connection.QueryFirstOrDefault<int>("sp_notificationCount", parameters, commandType: CommandType.StoredProcedure);
                count = (int)result;
            }
            return count;
        }

        public void UpdateFollowRequest(long followerId, long followingID, byte action)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("followerId", followerId, DbType.Int64);
                parameters.Add("followingID", followingID, DbType.Int64);
                parameters.Add("action", action, DbType.Byte);
                connection.Execute("sp_UpdateFollowRequest", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Suggestion> SearchUser(string userName, long userID)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userID", userID, DbType.Int64);
                parameters.Add("@userName", userName, DbType.String);


                List<Suggestion> UserList = connection.Query<Suggestion>("sp_SearchFollowedUser", parameters, commandType: CommandType.StoredProcedure).ToList();

                return UserList;
            }
        }

        public List<PostViewModel> GetScheduledPost(DateTime currentTime)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT id,user_id as userId FROM posts WHERE created_at <= @currentTime and status = 0";
                List<PostViewModel> postIds = connection.Query<PostViewModel>(query, new { currentTime }).ToList();
                return postIds;
               
            }
        }
        public void PublishPost(long postId)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {

                string query = "UPDATE posts SET [status] = 1 WHERE ID = @postId ";
                connection.Execute(query, new { postId });

            }
           
        }
    }
}

