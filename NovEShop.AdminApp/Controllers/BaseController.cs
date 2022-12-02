using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NovEShop.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        protected ClaimsPrincipal ValidateToken(string jwtToken, IConfiguration _configuration)
        {

            //var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(jwtToken));
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtOptions:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtOptions:Audience"],
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SigningKey"]))
            };

            try
            {
                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken,
                    validationParameters,
                    out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
