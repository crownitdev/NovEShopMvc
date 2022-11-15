﻿using NovEShop.Data.Models.Commons;
using System;

namespace NovEShop.Data.Models
{
    public class OrderDetail : EntityId
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
