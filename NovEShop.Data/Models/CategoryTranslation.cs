using NovEShop.Data.Models.Commons;

namespace NovEShop.Data.Models
{
    public class CategoryTranslation : EntityId
    {
        public string Name { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoAlias { get; set; }
        public int CategoryId { get; set; }
        public string LanguageId { get; set; }
        public Category Category { get; set; }
        public Language Language { get; set; }
    }
}
