using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Queries;
using NovEShop.Models;
using System.Diagnostics;

namespace NovEShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBroker _broker;

        public HomeController(ILogger<HomeController> logger,
            IBroker broker)
        {
            _logger = logger;
            _broker = broker;
        }

        public IActionResult Index()
        {
            //var response = _broker.Query(new GetProductsHomePageQuery());
            var bestSellerProducts = _broker.Query(new GetProductMetasByCategoryNameQuery() 
            {
                CategoryName = "Bán chạy",
                PageNumber = 1,
                PageSize = 8
            });

            var newArrivalProducts = _broker.Query(new GetProductMetasByCategoryNameQuery()
            {
                CategoryName = "Sản phẩm mới",
                PageNumber = 1,
                PageSize = 8
            });

            var saleProducts = _broker.Query(new GetProductMetasByCategoryNameQuery()
            {
                CategoryName = "Khuyến mãi",
                PageNumber = 1,
                PageSize = 8
            });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
