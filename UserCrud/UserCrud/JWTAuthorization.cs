using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;

namespace UserCrud
{
    public class JWTAuthorization
    {
        public static TokenValidationResult ValidateToken(string token)
        {
            var jwtKey = ConfigurationManager.AppSettings["JwtKey"];
            var jwtIssuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var expiryMinutes = Convert.ToDouble(ConfigurationManager.AppSettings["JwtExpireMinutes"]);
            var expirationTime = DateTime.UtcNow;
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateLifetime = true,
                    ValidateAudience = false // Adjust as needed
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if(validatedToken.ValidTo < expirationTime)
                {
                    return new TokenValidationResult { IsValid = false, IsExpired = true };
                }
                return new TokenValidationResult { IsValid = true, IsExpired = false };
            }
            catch
            {
                return new TokenValidationResult { IsValid = false, IsExpired = false };
            }
        }
        public class TokenValidationResult
        {
            public bool IsValid { get; set; }
            public bool IsExpired { get; set; }
        }
    }
}


