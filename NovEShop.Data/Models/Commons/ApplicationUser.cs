using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NovEShop.Data.Models.Commons
{
    public class ApplicationUser : IdentityUser<int>, IEntityActivable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        [ProtectedPersonalData]
        [MaxLength(50)]
        public override string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
