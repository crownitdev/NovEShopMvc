using NovEShop.Handler.Products.Queries;

namespace NovEShop.Handler.Commons
{
    public class HomeDataResponse
    {
        public GetProductMetasByCategoryNameQueryResponse NewArrivalProducts { get; set; }
        public GetProductMetasByCategoryNameQueryResponse BestSellerProducts { get; set; }
        public GetProductMetasByCategoryNameQueryResponse SaleProducts { get; set; }
    }
}
