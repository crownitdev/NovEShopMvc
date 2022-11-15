using Microsoft.AspNetCore.Http;

namespace EcommerceWebApp.Handler.Products.Dtos
{
    public class SaveProductImageRequest
    {
        public int ProductId { get; set; }
        public IFormFile ThumbnailFile { get; set; }
    }
}
