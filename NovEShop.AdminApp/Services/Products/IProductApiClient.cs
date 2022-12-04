using NovEShop.Handler.Products.Queries;
using NovEShop.Handler.Products.Commands;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Products
{
    public interface IProductApiClient
    {
        Task<GetAllProductsPagingQueryResponse> GetAllProductsPaging(GetAllProductsPagingQuery request);
        Task<CreateProductCommandResponse> CreateProduct(CreateProductCommand request);
    }
}
