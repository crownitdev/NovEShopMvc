using Newtonsoft.Json;
using NovEShop.Handler.Users.Commands;
using NovEShop.Handler.Users.Queries;
using NovEShop.Share.Constants;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Users
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserApiClient(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CreateUserCommandResponse> CreateUser(CreateUserCommand request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
            var response = await client.PostAsync($"/api/user/create", httpContent);

            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<CreateUserCommandResponse>(body);

            return users;
        }

        public async Task<DeleteUserCommandResponse> DeleteUser(DeleteUserCommand request)
        {
            //var json = JsonConvert.SerializeObject(request);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.TokenAuth);
            var response = await client.DeleteAsync($"/api/user/delete/{request.Id}/{request.TokenAuth}");

            var body = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<DeleteUserCommandResponse>(body);

            return responseData;
        }

        public async Task<GetAllUsersPagingQueryResponse> GetAllUsersPaging(GetAllUsersPagingQuery request)
        {
            var json = JsonConvert.SerializeObject(request);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);
            var response = await client.GetAsync($"/api/user/GetAllUsersPaging?" +
                $"pageNumber={request.PageNumber}&pageSize={request.PageSize}&keyword={request.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<GetAllUsersPagingQueryResponse>(body);
            return users;
        }

        public async Task<GetUserByIdQueryResponse> GetUserById(GetUserByIdQuery request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.TokenAuth);
            var serverResponse = await client.GetAsync($"/api/user/getuserbyid/{request.Id}");

            var body = await serverResponse.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<GetUserByIdQueryResponse>(body);
            return responseData;
        }

        public async Task<UpdateUserCommandResponse> UpdateUser(int id, UpdateUserCommand request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiUrlConstants.ServeApiUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.TokenAuth);
            var serverResponse = await client.PutAsync($"/api/user/update/{id}", httpContent);

            var body = await serverResponse.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<UpdateUserCommandResponse>(body);
            return responseData;
        }
    }
}
