﻿using Microsoft.AspNetCore.Http;

namespace NovEShop.Handler.Products.Dtos
{
    public class ProductCreateRequest
    {
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
