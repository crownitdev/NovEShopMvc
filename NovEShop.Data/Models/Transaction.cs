using NovEShop.Data.Models.Commons;
using NovEShop.Share.Enums;
using System;

namespace NovEShop.Data.Models
{
    public class Transaction : EntityId
    {
        public DateTime TransactionDate { get; set; }
        public string ExternalTransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public TransactionStatus Status { get; set; }
        public string Provider { get; set; }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
