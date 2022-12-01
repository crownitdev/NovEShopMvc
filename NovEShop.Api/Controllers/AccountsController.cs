using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Accounts.Commands;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IBroker _broker;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<RegisterRequestDto> _registerValidator;

        public AccountsController(
            IValidator<LoginRequestDto> loginValidator,
            IValidator<RegisterRequestDto> registerValidator,
            IBroker broker)
        {
            _broker = broker;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var validateResult = await _loginValidator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                // Copy the validation results into ModelState.
                // ASP.NET uses the ModelState collection to populate 
                // error messages in the View.
                validateResult.AddToModelState(ModelState);
                return BadRequest(ModelState);

            }

            var result = await _broker.Command(request);
            if (!result.IsSucceed)
            {
                return BadRequest("Username hoặc mật khẩu không đúng");
            }

            return Ok(result);
        }

        [HttpPost("/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterAccountCommand request)
        {
            var validateResult = await _registerValidator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                validateResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }


            var result = await _broker.Command(request);
            if (!result.IsSucceed)
            {
                return BadRequest("Đăng ký tài khoản thất bại");
            }

            return Ok(result);
        }
    }
}
