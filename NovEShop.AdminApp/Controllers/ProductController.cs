using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NovEShop.AdminApp.Services.Categories;
using NovEShop.AdminApp.Services.Products;
using NovEShop.Handler.Products.Commands;
using NovEShop.Handler.Products.Dtos;
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
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(
            IProductApiClient productApiClient,
            ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetAllProductsPagingQuery request)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            request.LanguageId = languageId;
            //var request = new GetAllProductsPagingQuery()
            //{
            //    Keyword = keyword,
            //    PageNumber = pageNumber,
            //    PageSize = pageSize,
            //    LanguageId = languageId,
            //    CategoryId = categoryId
            //};

            var response = await _productApiClient.GetAllProductsPaging(request);
            if (!response.IsSucceed)
            {
                ModelState.AddModelError("", response.Message);
            }
            ViewData["keyword"] = request.Keyword;

            var categories = await _categoryApiClient.GetAllCategories();
            ViewBag.Categories = categories.Data.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = request.CategoryId.HasValue && request.CategoryId.Value == x.Id
            });

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

        [HttpGet]
        public async Task<IActionResult> AssignToCategory(int id)
        {
            var productCategoriesResponse = await GetCategoriesOfProduct(id);
            return View(productCategoriesResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AssignToCategory(AssignProductToCategoriesCommand request)
        {
            var result = await _productApiClient.AssignProductToCategories(request);
            if (result.IsSucceed)
            {
                TempData["Result"] = $"Cập nhật danh mục cho sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var productCategoriesResponse = GetCategoriesOfProduct(request.Id);

            return View(productCategoriesResponse);
        }

        private async Task<CategoryAssignRequestDto> GetCategoriesOfProduct(int id)
        {
            var productResponse = await _productApiClient.GetProductById(id);
            var categoriesResponse = await _categoryApiClient.GetAllCategories();

            var categoryAssignRequest = new CategoryAssignRequestDto();
            foreach (var role in categoriesResponse.Data)
            {
                categoryAssignRequest.Categories.Add(new CategorySelectItemDto
                {
                    Name = role.Name,
                    CategoryId = role.Id,
                    Selected = productResponse.Data.CategoryIds.Contains(role.Id)
                });
            }

            return categoryAssignRequest;
        }

    }
}
