using EcommerceWebApp.Data;
using EcommerceWebApp.Handler.Infrastructure;
using EcommerceWebApp.Handler.Pagination.Dtos;
using EcommerceWebApp.Handler.Products.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApp.Handler.Products.Queries
{
    public class GetAllProductByCategoryIdQuery : IQuery<GetAllProductByCategoryIdQueryResponse>
    {
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllProductByCategoryIdQueryHandler : IQueryHandler<GetAllProductByCategoryIdQuery, GetAllProductByCategoryIdQueryResponse>
    {
        private readonly EcommerceAppDbContext _db;

        public GetAllProductByCategoryIdQueryHandler(EcommerceAppDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllProductByCategoryIdQueryResponse> Handle(GetAllProductByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _db.ProductCategories on p.Id equals pic.ProductId
                        join c in _db.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId.Value >= 0)
            {
                query = query.Where(c => c.pic.CategoryId == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                })
                .ToListAsync();

            var response = new GetAllProductByCategoryIdQueryResponse(data, request.PageNumber, request.PageSize);
            return response;
        }
    }

    public class GetAllProductByCategoryIdQueryResponse : PaginationResponse<ICollection<ProductViewModel>>
    {
        public GetAllProductByCategoryIdQueryResponse(ICollection<ProductViewModel> data, int pageNumber, int pageSize)
            : base(data, pageNumber, pageSize)
        {
        }
    }
}
