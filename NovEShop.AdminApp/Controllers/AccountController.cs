using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NovEShop.AdminApp.Services;
using NovEShop.Handler.Accounts.Commands;
using NovEShop.Handler.Accounts.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IAccountApiClient _accountApiClient;
        private readonly IConfiguration _configuration;

        public AccountController(IValidator<LoginRequestDto> loginValidator,
            IAccountApiClient accountApiClient,
            IConfiguration configuration
            )
        {
            _loginValidator = loginValidator;
            _accountApiClient = accountApiClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var validateResult = await _loginValidator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                validateResult.AddToModelState(ModelState);
                return View(ModelState);
            }

            var response = await _accountApiClient.Login(request);

            var userPrincipal = this.ValidateToken(response, _configuration);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            HttpContext.Session.SetString("Token", response);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Account");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken, IConfiguration _configuration)
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
