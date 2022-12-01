using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NovEShop.AdminApp.Services;
using NovEShop.Handler.Accounts.Commands;
using NovEShop.Handler.Accounts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IAccountApiClient _accountApiClient;

        public AccountController(IValidator<LoginRequestDto> loginValidator,
            IAccountApiClient accountApiClient
            )
        {
            _loginValidator = loginValidator;
            _accountApiClient = accountApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
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


            return View();
        }
    }
}
