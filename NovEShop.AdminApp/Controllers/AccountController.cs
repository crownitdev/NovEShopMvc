using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class AccountController : BaseController
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
    }
}
