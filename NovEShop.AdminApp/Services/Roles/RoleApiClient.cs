using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NovEShop.Handler.Roles.Dtos;
using NovEShop.Handler.Roles.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Roles
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAllRolesQueryResponse> GetAllRoles()
        {
            var tokenAuth = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAuth);
            var response = await client.GetAsync($"/api/roles/getall");

            var body = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<GetAllRolesQueryResponse>(body);

            return responseData;
        }
    }
}
