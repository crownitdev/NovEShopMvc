using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.Data.Models.Commons
{
    public class AuditEntity : EntityId, IEntityActivable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedById { get; set; }
        public int? CreatedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
