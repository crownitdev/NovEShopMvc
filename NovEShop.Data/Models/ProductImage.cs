using NovEShop.Data.Models.Commons;
using System;

namespace NovEShop.Data.Models
{
    public class ProductImage : EntityId
    {
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
