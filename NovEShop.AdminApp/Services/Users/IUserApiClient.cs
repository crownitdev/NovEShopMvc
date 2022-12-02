using NovEShop.Handler.Users.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Users
{
    public interface IUserApiClient
    {
        Task<GetAllUsersPagingQueryResponse> GetAllUsersPaging(GetAllUsersPagingQuery request);
    }
}
