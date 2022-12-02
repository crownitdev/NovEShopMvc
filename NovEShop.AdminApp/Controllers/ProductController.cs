using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Users.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        public ProductController()
        {

        }

        public async Task<IActionResult> Index(string keyword, int pageNumber, int pageSize)
        {
            var token = HttpContext.Session.GetString("Token");
            var request = new GetAllUsersPagingQuery()
            {
                BearerToken = token,
                Keyword = keyword,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View();
        }
    }
}
