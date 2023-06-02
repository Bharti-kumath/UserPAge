using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace UserCrud
{

    public class AuthorizationAttribute : AuthorizeAttribute
    {
       
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = httpContext.Session["Token"];

            if (token != null)
            {
                var tokenString = token.ToString();
                var validationResult = JWTAuthorization.ValidateToken(tokenString);

                if (validationResult.IsValid)
                {
                    return true;
                }
                else if (validationResult.IsExpired)
                {
                    // Handle expired token case
                    httpContext.Response.Redirect("~/Home/ExpiredToken"); // Redirect to an expired token view
                    return false;
                }
            }

            // Handle invalid token case
            httpContext.Response.Redirect("~/Home/ExpiredToken"); // Redirect to an invalid token view
            return false;
        }
    }




}











