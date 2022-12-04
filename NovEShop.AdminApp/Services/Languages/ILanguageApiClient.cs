using NovEShop.Handler.Languages.Queries;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Languages
{
    public interface ILanguageApiClient
    {
        Task<GetAllLanguagesQueryResponse> GetAll();
    }
}
