using System;

namespace EcommerceWebApp.Handler.Categories.Dtos
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string SeoDescription { get; set; }
        public string SeoAlias { get; set; }
        public string SeoTitle { get; set; }
        public int SortOrder { get; set; }
        public string LanguageId { get; set; }
        public bool IsShowOnHome { get; set; }
        public int? ParentId { get; set; }
    }
}
