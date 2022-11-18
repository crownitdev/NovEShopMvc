using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IBroker _broker;

        public IActionResult Index()
        {
            //var response = _broker.Query(new GetAllProductPagingQuery());
            return View();
        }public IActionResult Details()
        {
            return View();
        }
    }
}
