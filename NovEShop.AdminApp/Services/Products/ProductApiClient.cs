using Newtonsoft.Json;
using NovEShop.Handler.Products.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Products
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductApiClient(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetAllProductsPagingQueryResponse> GetAllProductsPaging(GetAllProductsPagingQuery request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

            var response = await client.GetAsync($"/api/product?keyword={request.Keyword}" +
                $"&pageNumber={request.PageNumber}&pageSize={request.PageSize}");

            var resultString = await response.Content.ReadAsStringAsync();
            var resultData = JsonConvert.DeserializeObject<GetAllProductsPagingQueryResponse>(resultString);

            return resultData;
        }
    }
}
