using NovEShop.Handler.Users.Commands;
using NovEShop.Handler.Users.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Users
{
    public interface IUserApiClient
    {
        Task<GetAllUsersPagingQueryResponse> GetAllUsersPaging(GetAllUsersPagingQuery request);
        Task<CreateUserCommandResponse> CreateUser(CreateUserCommand request);
    }
}
