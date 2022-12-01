using Newtonsoft.Json;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Share.Constants;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountApiClient(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Login(LoginRequestDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            var response = await client.PostAsync($"/Login", httpContent);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }
}
