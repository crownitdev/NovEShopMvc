using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetProductByIdQuery : IQuery<GetProductByIdQueryResponse>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductByIdQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetProductByIdQueryResponse();

            if(string.IsNullOrEmpty(request.ProductId.ToString()))
            {
                throw new NullReferenceException("Product Id không hợp lệ");
            }

            var productJoinQuery = from p in _db.Products
                          join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                          join pc in _db.ProductCategories on p.Id equals pc.ProductId
                          join ct in _db.CategoryTranslations on pc.CategoryId equals ct.CategoryId
                          select new { p, pt, pc, ct };

            var productResponse = productJoinQuery.Where(q => q.p.Id == request.ProductId)
                          .Select(p => new ProductViewModel()
                          {
                              Id = p.p.Id,
                              Name = p.pt.Name,
                              Description = p.pt.Description,
                              Details = p.pt.Details,
                              Stock = p.p.Stock,
                              ViewCount = p.p.ViewCount,
                              Price = p.p.Price,
                              DateCreated = p.p.CreatedAt,
                              SeoTitle = p.pt.SeoTitle,
                              SeoDescription = p.pt.SeoDescription,
                              SeoAlias = p.pt.SeoAlias,
                              LanguageId = p.pt.LanguageId,
                              OriginalPrice = p.p.OriginalPrice
                          }).FirstOrDefault();

            response.Data = productResponse;
            response.Message = "Lấy sản phẩm thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class GetProductByIdQueryResponse : Response<ProductViewModel>
    {
    }
}
