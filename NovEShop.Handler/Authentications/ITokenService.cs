using NovEShop.Handler.Accounts.Dtos;
using System.Threading.Tasks;

namespace NovEShop.Handler.Authentications
{
    public interface ITokenService
    {
        Task<bool> ValidateUserAsync(LoginRequestDto request);
        Task<string> CreateTokenAsync();

        bool IsTokenValid(string token);
    }
}
