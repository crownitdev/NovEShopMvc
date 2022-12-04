using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovEShop.AdminApp.Services.Products;
using NovEShop.Handler.Products.Commands;
using NovEShop.Handler.Products.Queries;
using NovEShop.Handler.Users.Queries;
using NovEShop.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovEShop.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;

        public ProductController(
            IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageNumber = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetAllProductsPagingQuery()
            {
                Keyword = keyword,
                PageNumber = pageNumber,
                PageSize = pageSize,
                LanguageId = languageId
            };

            var response = await _productApiClient.GetAllProductsPaging(request);
            if (!response.IsSucceed)
            {
                ModelState.AddModelError("", response.Message);
            }

            if (TempData["Result"] != null)
            {
                ViewData["Result"] = TempData["Result"];
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]CreateProductCommand request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _productApiClient.CreateProduct(request);
            if (!response.IsSucceed)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }

            TempData["Result"] = response.Message;
            return RedirectToAction("Index", "Product");
        }
    }
}
