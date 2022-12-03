using NovEShop.Handler.Roles.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Roles
{
    public interface IRoleApiClient
    {
        Task<GetAllRolesQueryResponse> GetAllRoles();
    }
}
