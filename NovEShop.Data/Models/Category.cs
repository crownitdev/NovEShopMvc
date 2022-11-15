using NovEShop.Data.Models.Commons;
using System.Collections.Generic;

namespace NovEShop.Data.Models
{
    public class Category : AuditEntity
    {
        public int SortOrder { get; set; }
        public bool IsShowOnHome { get; set; }
        public int? ParentId { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
