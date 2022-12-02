using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Accounts.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.Handler.Authentications
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser? _applicationUser;

        public TokenService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUserAsync(LoginRequestDto request)
        {
            _applicationUser = await _userManager.FindByNameAsync(request.UserName);
            var result = _applicationUser != null && await _userManager.CheckPasswordAsync(_applicationUser, request.Password);
            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("JwtOptions");
            var signingKey = Encoding.UTF8.GetBytes(jwtConfig["SigningKey"]);
            var secret = new SymmetricSecurityKey(signingKey);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var roles = await _userManager.GetRolesAsync(_applicationUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _applicationUser.Email),
                new Claim(ClaimTypes.MobilePhone, _applicationUser.PhoneNumber?? ""),
                new Claim(ClaimTypes.GivenName, _applicationUser.FullName),
                new Claim(ClaimTypes.Role, string.Join(",", roles)?? "")
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtOptions");
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(jwtSettings["ExpiresIn"])),
                signingCredentials: signingCredentials
                );

            return tokenOptions;
        }

        public bool IsTokenValid(string token)
        {
            var jwtOptions = _configuration.GetSection("JwtOptions");
            var signingKey = Encoding.UTF8.GetBytes(jwtOptions["SigningKey"]);
            var securityKey = new SymmetricSecurityKey(signingKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtOptions["Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = securityKey
                    }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
