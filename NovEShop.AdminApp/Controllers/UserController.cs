using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NovEShop.AdminApp.Services.Users;
using NovEShop.Handler.Users.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;

        public UserController(IConfiguration configuration,
            IUserApiClient userApiClient)
        {
            _configuration = configuration;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageNumber = 1, int pageSize = 10)
        {
            
            
            var token = HttpContext.Session.GetString("Token");
            var request = new GetAllUsersPagingQuery()
            {
                BearerToken = token,
                Keyword = keyword,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetAllUsersPaging(request);
            return View(data.Data);
        }
    }
}
