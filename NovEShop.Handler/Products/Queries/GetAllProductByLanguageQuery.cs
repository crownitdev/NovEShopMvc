using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Paginations.Dtos;
using NovEShop.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetAllProductByLanguageQuery : PaginationFilter, IQuery<GetAllProductByLanguageQueryResponse>
    {
        public string LanguageId { get; set; }
    }

    public class GetAllProductByLanguageQueryHandler : IQueryHandler<GetAllProductByLanguageQuery, GetAllProductByLanguageQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetAllProductByLanguageQueryHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllProductByLanguageQueryResponse> Handle(GetAllProductByLanguageQuery request, CancellationToken cancellationToken)
        {
            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _db.ProductCategories on p.Id equals pc.ProductId
                        join c in _db.Categories on pc.CategoryId equals c.Id
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pc };

            var data = await query.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    SeoAlias = x.pt.SeoAlias,
                    OriginalPrice = x.p.Price,
                    Price = x.p.Price,
                    ViewCount = x.p.ViewCount,
                    Stock = x.p.Stock,
                    DateCreated = x.p.CreatedAt,
                    LanguageId = x.pt.LanguageId
                })
                .ToListAsync();

            var response = new GetAllProductByLanguageQueryResponse(data);
            response.Message = $"Đã tìm thấy {data.Count} sản phẩm";
            response.TotalRecords = data.Count;
            response.TotalPages = data.Count / request.PageSize == 0 ? 1 : Convert.ToInt32(data.Count / request.PageSize);

            return response;
        }
    }

    public class GetAllProductByLanguageQueryResponse : PaginationResponse<ICollection<ProductViewModel>>
    {
        public GetAllProductByLanguageQueryResponse(ICollection<ProductViewModel> data)
            : base(data)
        {
        }
    }
}
