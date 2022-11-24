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

        public ProductController(IBroker broker)
        {
            _broker = broker;
        }

        public IActionResult Index()
        {
            var response = _broker.Query(new GetAllProductPagingQuery());
            return View();
        }

        public async Task<IActionResult> Search(GetProductsByNameQuery request)
        {
            var response = await _broker.Query(request);
            return View(response);
        }
        
        public async Task<IActionResult> Detail([FromQuery] GetProductByIdQuery request)
        {
            var response = await _broker.Query(request);
            if (response.IsSucceed)
            {
                return View(response.Data);
            }

            return RedirectToAction("NotFound", controllerName: "Home");
        }
    }
}
