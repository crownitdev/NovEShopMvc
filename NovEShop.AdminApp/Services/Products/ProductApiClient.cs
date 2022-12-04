using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NovEShop.Handler.Products.Commands;
using NovEShop.Handler.Products.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Products
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            :base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AssignProductToCategoriesCommandResponse> AssignProductToCategories(AssignProductToCategoriesCommand request)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var serverResponse = await client.PutAsync($"/api/product/AssignToCategories/{request.Id}/", httpContent);

            var body = await serverResponse.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<AssignProductToCategoriesCommandResponse>(body);
            return responseData;
        }

        public async Task<CreateProductCommandResponse> CreateProduct(CreateProductCommand request)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var currentLanguageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // preparing data serialize
            var requestContent = new MultipartFormDataContent();
            
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "Price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "OriginalPrice");
            requestContent.Add(new StringContent(request.Name), "Name");
            requestContent.Add(new StringContent(request.Description), "Description");
            requestContent.Add(new StringContent(request.Details), "Details");
            requestContent.Add(new StringContent(request.SeoAlias), "SeoAlias");
            requestContent.Add(new StringContent(request.SeoTitle), "SeoTitle");
            requestContent.Add(new StringContent(request.SeoDescription), "SeoDescription");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(currentLanguageId), "LanguageId");


            var response = await client.PostAsync($"/api/product/create", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonConvert.DeserializeObject<CreateProductCommandResponse>(body);

            return dataResponse;
        }

        public async Task<GetAllProductsPagingQueryResponse> GetAllProductsPaging(GetAllProductsPagingQuery request)
        {
            var response = await GetAsync<GetAllProductsPagingQueryResponse>($"/api/product/GetAll?languageid={request.LanguageId}" +
                $"&keyword={request.Keyword}" +
                $"&pageNumber={request.PageNumber}" +
                $"&pageSize={request.PageSize}" +
                $"&categoryId={request.CategoryId}");

            return response;
        }

        public async Task<GetProductByIdQueryResponse> GetProductById(int id)
        {
            var currentLanguageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var response = await GetAsync<GetProductByIdQueryResponse>($"/api/product/GetProductById?id={id}" +
                $"&languageid={currentLanguageId}");

            return response;
        }
    }
}
