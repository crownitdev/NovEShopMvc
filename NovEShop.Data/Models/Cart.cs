using NovEShop.Data.Models.Commons;
using System;

namespace NovEShop.Data.Models
{
    public class Cart : EntityId
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
