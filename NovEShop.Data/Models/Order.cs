using NovEShop.Data.Models.Commons;
using NovEShop.Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.Data.Models
{
    public class Order : EntityId
    {
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipEmail { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhoneNumber { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
