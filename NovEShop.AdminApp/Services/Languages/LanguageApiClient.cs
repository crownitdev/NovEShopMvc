using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NovEShop.Handler.Languages.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Services.Languages
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        public LanguageApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            :base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<GetAllLanguagesQueryResponse> GetAll()
        {
            return await GetAsync<GetAllLanguagesQueryResponse>("/api/languages/GetAllLanguages");
        }
    }
}
