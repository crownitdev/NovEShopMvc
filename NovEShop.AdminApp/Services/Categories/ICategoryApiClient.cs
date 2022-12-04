using NovEShop.Handler.Categories.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Categories
{
    public interface ICategoryApiClient
    {
        Task<GetAllCategoriesQueryResponse> GetAllCategories();
    }
}
