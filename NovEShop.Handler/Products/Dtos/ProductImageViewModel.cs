using System;

namespace NovEShop.Handler.Products.Dtos
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }
        public bool IsDefault { get; set; }
        public long FileSize { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
