using Microsoft.AspNetCore.Mvc;

namespace NovEShop.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
