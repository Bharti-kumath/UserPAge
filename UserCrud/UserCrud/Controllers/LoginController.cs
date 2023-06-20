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
using System.Web;
using System.Web.Mvc;

namespace UserCrud.Controllers
{
    public class LoginController : Controller
    {

        private readonly IRepository _repository;

        public LoginController(IRepository repository)
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
                var time = HttpContext.Timestamp;
                Session["Time"] = time;


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
            return RedirectToAction("Login", "Login");
        }

        #endregion
    }
}