using Azure;
using Azure.Communication.Sms;
using BAL.Models;
using DAL;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace UserCrud.Controllers
{

    public class UserController : Controller

    {
        private readonly IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
        }


        #region Login
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            UserViewModel userExist = _repository.CheckUser(Email, Password);
            if (userExist != null)
            {
                Session["id"] = userExist.ID;
                Session["userName"] = userExist.FirstName + " " + userExist.LastName;
                var claims = new[]
            {
                new Claim("Email", Email)
            };

                var jwtKey = ConfigurationManager.AppSettings["JwtKey"];
                var jwtIssuer = ConfigurationManager.AppSettings["JwtIssuer"];
                var expiryMinutes = Convert.ToDouble(ConfigurationManager.AppSettings["JwtExpireMinutes"]);
                var expirationTime = DateTime.UtcNow.AddMinutes(expiryMinutes);


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: null,
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: creds
                );

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                string expirationTimeString = token.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");

                Session["Token"] = encodedToken;

                return RedirectToAction("UserDetail", "User"); // Redirect to a protected page
            }

            else
            {
                TempData["ErrorMessage"] = "Invalid Email OR Password";
                return View();
            }

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        #endregion

        #region UserDetail

        [HttpGet]
        [Authorization]
        public ActionResult UserDetail()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetDetails(FilterOptions filterOptions)
        {

            var userList = _repository.GetUserDetails(filterOptions);


            return Json(new
            {
                data = userList,
                recordsTotal = userList.FirstOrDefault()?.TotalCount,
                recordsFiltered = userList.FirstOrDefault()?.TotalCount
            });

        }

        [HttpPost]
        public ActionResult UserDetail(UserViewModel model)
        {
            if (model.ID != 0)
            {
                model.ConfirmPassword = model.Password;
                ModelState.Clear();
            }
            if (ModelState.IsValid)
            {
                _repository.SaveUserDetails(model);
                TempData["SuccessMessage"] = "User added successfully.";
                return RedirectToAction("UserDetail", "User");
            }
            else
            {
                return View(model);
            }

        }

        public ActionResult GetUserById(long id)
        {
            var userById = _repository.GetUserById(id);
            return PartialView("UserPartial", userById);
        }

        public ActionResult deleteUserDetail(long userID)
        {
            _repository.deleteUserDetails(userID);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportCSV(FilterOptions filterOptions)
        {

            filterOptions.export = 1;
            _repository.GeCSVFile(filterOptions);

            return Json("exported");
        }

        #endregion


        [Authorization]
        public ActionResult UserProfile()
        {
            var userId = Convert.ToInt64(Session["id"].ToString());
            ProfileViewModel profileData = _repository.GetProfileById(userId);
            return View(profileData);
        }
        public ActionResult FriendProfile(long friendId)
        {
            var userId = Convert.ToInt64(Session["id"].ToString());
            ProfileViewModel profileData = _repository.GetFriendProfileById(userId, friendId);
            return View(profileData);
        }

        #region Posts
        [Authorization]
        public ActionResult Landing()
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var allPost = _repository.GetAllPosts(userID);
            return View(allPost);
        }

        [HttpPost]
        public ActionResult SavePost(PostViewModel model)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            _repository.SavePost(model, userID);
            return Json(new { success = true });
        }

        public  ActionResult EditPost (long postId)
        {
            var postbyId = _repository.EditPost(postId);
            return Json(postbyId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult deletePost(long postID)
        {
            _repository.DeletePost(postID);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LikePost(long postID, long postUserID)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var post = _repository.LikePost(postID, userID , postUserID);
            if (post != null)
            {
                return Json(post.TotalLikes);
            }
            else
            {
                return Json("0");
            }

        }

        [HttpPost]
        public ActionResult GetLikeUserList(long postId)
        {
            List<Suggestion> userList = _repository.GetLikeUserList(postId);
            return Json(userList);
        }
        public ActionResult GetCommentsByPostId(long id)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            ViewBag.userID = userID;
            ViewBag.userName = Session["userName"].ToString();
            List<CommentViewModel> comments = _repository.GetCommentsByPostId(id);
            return PartialView("Comment", comments);
        }
        [HttpPost]
        public ActionResult SaveComment(long postID, string commentText,long postUserID,long? toUserID = null)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var comment = _repository.SaveComment(userID, postID, commentText, postUserID, toUserID);
            if (comment != null)
            {
                return Json(comment.TotalComments);
            }
            else
            {
                return Json("0");
            }

        }
        [HttpPost]
        public ActionResult SaveCommentReply(long commentId,string replyText,long toUserId )
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var reply = _repository.SaveCommentReply(commentId, userID, replyText, toUserId);
            if (reply != null)
            {
                return Json(reply);
            }
            else
            {
                return Json("0");
            }
        }

        public ActionResult GetReplyByCommentID(long commentId)
        {
            List<ReplyViewModel> Replies = _repository.GetReplyByCommentID(commentId);
            return PartialView("Reply", Replies);
        }
        public ActionResult DeleteComment(long commentID, long postId)
        {
            CommentViewModel comment = _repository.DeleteComment(commentID, postId);

            if (comment != null)
            {
                return Json(comment.TotalComments);
            }
            else
            {
                return Json("0");
            }
        }

        public ActionResult FollowRequest(long toUserId)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            _repository.FollowRequest(userID, toUserId);

            return Json("requested");

        }

        [Authorization]
        public ActionResult Notification()
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            List<NotificationViewModel> notificationList = _repository.GetNotificationByID(userID);
            return View(notificationList);
        }

        public ActionResult ReadNotification(long id)
        {
            _repository.changeNotificationStatus(id);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorization]
        public ActionResult GetNotificationCount()
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            int count = _repository.GetNotificationCount(userID);

            return Json(count , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateFollowRequest(long followerId, byte action)
        {
            var followingID = Convert.ToInt64(Session["id"].ToString());
            _repository.UpdateFollowRequest(followerId, followingID,action);
            return Json(new { success = true });
        }
        #endregion


        public ActionResult SearchUser(string userName)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            List<Suggestion> userList = _repository.SearchUser(userName , userID);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}
