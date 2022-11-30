using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Accounts.Commands
{
    public class RegisterAccountCommand : RegisterRequestDto, ICommand<RegisterAccountCommandResponse>
    {
    }

    public class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand, RegisterAccountCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterAccountCommandHandler(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<RegisterAccountCommandResponse> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new RegisterAccountCommandResponse();

            var user = new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                response.Message = "Đăng ký tài khoản thành công";
                response.IsSucceed = true;
            }
            else
            {
                response.Message = "Đăng ký tài khoản thất bại";
                response.IsSucceed = false;
                response.Errors = result.Errors.Select(x => x.Description).ToList();
            }

            return response;
        }
    }

    public class RegisterAccountCommandResponse : Response
    {
        public string TokenAuth { get; set; }
    }
}
