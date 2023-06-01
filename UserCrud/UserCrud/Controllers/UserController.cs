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
        private readonly IRepository _repository ;
       
        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: User
        public ActionResult Login()
        {
        
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email , string Password)
        {
            bool isUserExist = _repository.checkUser(Email, Password);
            if (isUserExist)
            {
                Claim claims =
                        new Claim("Email", Email);

       
                var jwtkey = ConfigurationManager.ConnectionStrings["JwtKey"].ConnectionString;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var issuer = ConfigurationManager.ConnectionStrings["JwtIssuer"].ConnectionString;
                var expiry = ConfigurationManager.ConnectionStrings["JwtExpireDays"].ConnectionString;
                var expiryhour = Convert.ToDouble(expiry);
                var token = new JwtSecurityToken(
                   issuer,
                    claims.ToString(),
                   expires: DateTime.UtcNow.AddMinutes(expiryhour),
                    signingCredentials: creds
                    );

                return Content (new JwtSecurityTokenHandler().WriteToken(token));
            }
            return View();
        }
        [HttpGet]
        [Authorize]
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
