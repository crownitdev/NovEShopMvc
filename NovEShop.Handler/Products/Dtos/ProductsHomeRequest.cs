using System.Collections.Generic;

namespace NovEShop.Handler.Products.Dtos
{
    public class ProductsHomeRequest
    {
        public IEnumerable<ProductMetaViewModel> TShirts { get; set; }
        public IEnumerable<ProductMetaViewModel> Hats { get; set; }
        public IEnumerable<ProductMetaViewModel> Clocks { get; set; }
    }
}
