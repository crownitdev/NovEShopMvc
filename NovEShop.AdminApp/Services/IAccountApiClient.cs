using NovEShop.Handler.Accounts.Dtos;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services
{
    public interface IAccountApiClient
    {
        Task<string> Login(LoginRequestDto request);
    }
}
