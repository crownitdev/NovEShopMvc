using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Accounts.Commands
{
    public class LoginCommand : LoginRequestDto, ICommand<LoginCommandResponse>
    {
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new LoginCommandResponse();

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                response.Message = "Tên đăng nhập và mật khẩu không được để trống";
                response.IsSucceed = false;

                return response;
            }

            // Check user is existed
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.Message = "Tên đăng nhập không tồn tại";
                response.IsSucceed = false;

                return response;
                //throw new NovEShopException("Username không tồn tại");
            }

            if (user.IsActive != true)
            {
                response.Message = "Tài khoản đã bị khoá";
                response.IsSucceed = false;

                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(
                user: user,
                password: request.Password,
                isPersistent: request.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                response.Message = "Đăng nhập thành công";
                response.IsSucceed = true;
                await SetLoginSuccessResult(user, response);
            }
            else if (result.IsLockedOut)
            {
                response.Message = "UserIsLockedOut";
                response.IsSucceed = false;
            }
            else
            {
                response.Message = "Thông tin người dùng không đúng";
                response.IsSucceed = false;
            }

            return response;
        }

        private async Task SetLoginSuccessResult(ApplicationUser user, LoginCommandResponse response)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber?? ""),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, string.Join(",", roles)?? "")
            };

            var token = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecurityKey"]));
            var creds = new SigningCredentials(token, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.TokenAuth = jwtToken;
        }
    }
    public class LoginCommandResponse : Response
    {
        public string TokenAuth { get; set; }
    }
}
