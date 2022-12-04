using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NovEShop.Handler.Products.Dtos
{
    public class ProductCreateRequest
    {
        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { get; set; }
        [Display(Name = "Số lượng")]
        public int Stock { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        public string Details { get; set; }
        [Display(Name = "SEO Description")]
        public string SeoDescription { get; set; }
        [Display(Name = "SEO Title")]
        public string SeoTitle { get; set; }
        [Display(Name = "SEO Alias")]
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFile ThumbnailImage { get; set; }
    }
}
