using NovEShop.Data.Models.Commons;
using System;
using System.Collections.Generic;

namespace NovEShop.Data.Models
{
    public class Product : AuditEntity
    {
        public decimal? OriginalPrice { get; set; }
        public decimal? Price { get; set; }
        public int ViewCount { get; set; }
        public int Stock { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ProductTranslation> ProductTranslations { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public int ProductId { get; set; }
    }
}
