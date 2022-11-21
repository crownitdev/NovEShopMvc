using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Queries;
using NovEShop.Models;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            //var response = _broker.Query(new GetProductsHomePageQuery());
            var bestSellerProducts = await _broker.Query(new GetProductMetasByCategoryNameQuery() 
            {
                CategoryName = "Bán chạy",
                PageNumber = 1,
                PageSize = 8
            });

            var newArrivalProducts = await _broker.Query(new GetProductMetasByCategoryNameQuery()
            {
                CategoryName = "Sản phẩm mới",
                PageNumber = 1,
                PageSize = 8
            });

            var saleProducts = await _broker.Query(new GetProductMetasByCategoryNameQuery()
            {
                CategoryName = "Khuyến mãi",
                PageNumber = 1,
                PageSize = 8
            });

            var homeDataResponse = new HomeDataResponse()
            {
                NewArrivalProducts = newArrivalProducts,
                BestSellerProducts = bestSellerProducts,
                SaleProducts = saleProducts
            };

            return View(homeDataResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
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
