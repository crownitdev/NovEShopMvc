using NovEShop.Data.Models.Commons;
using NovEShop.Share.Enums;

namespace NovEShop.Data.Models
{
    public class Contact : EntityId
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public Status Status { get; set; }
    }
}
