using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace UserCrud
{
    public class Authorization :AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                return false;

            var jwtkey = ConfigurationManager.ConnectionStrings["JwtKey"].ConnectionString;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = ConfigurationManager.ConnectionStrings["JwtIssuer"].ConnectionString;
            var expiry = ConfigurationManager.ConnectionStrings["JwtExpireDays"].ConnectionString;
            var jwtToken = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "issuer",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jwtkey")),
               
            };

            try
            {
                var principal = handler.ValidateToken(jwtToken, jwtTokenValidationParameters, out _);
                httpContext.User = principal;
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}

