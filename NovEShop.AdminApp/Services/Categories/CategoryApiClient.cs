using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NovEShop.Handler.Categories.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Categories
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryApiClient(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            :base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAllCategoriesQueryResponse> GetAllCategories()
        {
            var currentLanguageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var response = await GetAsync<GetAllCategoriesQueryResponse>($"/api/categories/GetAll?languageid={currentLanguageId}");

            return response;
        }
    }
}
