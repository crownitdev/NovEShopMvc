using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Accounts.Commands;
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

        public AccountsController(
            IBroker broker)
        {
            _broker = broker;
        }

        [HttpPost("/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginCommand request)
        {
            if (!ModelState.IsValid)
            {
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
            if (!ModelState.IsValid)
            {
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
