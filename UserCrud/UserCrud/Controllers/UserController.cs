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
        private readonly IRepository _repository ;
       
        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        
        public ActionResult Login()
        {
            HttpContext.Response.Cookies.Remove("AuthToken");
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            bool isUserExist = _repository.checkUser(Email, Password);
            if (isUserExist)
            {
                var claims = new[]
            {
                new Claim("Email", Email)
            };

                var jwtKey = ConfigurationManager.AppSettings["JwtKey"];
                var jwtIssuer = ConfigurationManager.AppSettings["JwtIssuer"];
                var expiryMinutes = Convert.ToDouble(ConfigurationManager.AppSettings["JwtExpireMinutes"]);
                var expirationTime = DateTime.UtcNow.AddMinutes(1);
              

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

            return View();
        
        }

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

            return Json(new {success = true} , JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportCSV(FilterOptions filterOptions)
        {
          
            filterOptions.export = 1;
            _repository.GeCSVFile(filterOptions);
           
            return Content("exported");
        }

    }
}
