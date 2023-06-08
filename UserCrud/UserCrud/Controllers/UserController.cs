﻿using Azure;
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

        #endregion

        #region UserDetail

        [HttpGet]

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

        public ActionResult deletePost(long postID)
        {
            _repository.DeletePost(postID);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LikePost(long postID)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var post = _repository.LikePost(postID, userID);
            if (post != null)
            {
                return Json(post.TotalLikes);
            }
            else
            {
                return Json("0");
            }

        }
        public ActionResult GetCommentsByPostId(long id)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            ViewBag.userID = userID;
            List<CommentViewModel> comments = _repository.GetCommentsByPostId(id);
            return PartialView("Comment", comments);
        }
        [HttpPost]
        public ActionResult SaveComment(long postID, string commentText)
        {
            var userID = Convert.ToInt64(Session["id"].ToString());
            var comment = _repository.SaveComment(userID, postID, commentText);
            if (comment != null)
            {
                return Json(comment.TotalComments);
            }
            else
            {
                return Json("0");
            }

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
        #endregion

    }
}
