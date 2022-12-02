using NovEShop.Handler.Products.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Products
{
    public interface IProductApiClient
    {
        Task<GetAllProductsPagingQueryResponse> GetAllProductsPaging(GetAllProductsPagingQuery request);
    }
}
