using System.Collections.Generic;

namespace NovEShop.Handler.Products.Dtos
{
    public class CategoryAssignRequestDto
    {
        public int Id { get; set; }
        public List<CategorySelectItemDto> Categories { get; set; } = new List<CategorySelectItemDto>();
    }
}
