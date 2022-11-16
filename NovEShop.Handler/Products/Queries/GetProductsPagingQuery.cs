using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Paginations.Dtos;
using NovEShop.Handler.Products.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetProductsPagingQuery : PaginationFilter, IQuery<GetProductsPagingQueryResponse>
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }

    public class GetProductsPagingQueryHandler : IQueryHandler<GetProductsPagingQuery, GetProductsPagingQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductsPagingQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductsPagingQueryResponse> Handle(GetProductsPagingQuery request, CancellationToken cancellationToken)
        {
            // 1. Select join
            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _db.ProductCategories on p.Id equals pic.ProductId
                        join c in _db.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }

            // 3. Paging
            int totalRows = await query.CountAsync();
            var data = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.CreatedAt,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            // 4. Select and projection

            var response = new GetProductsPagingQueryResponse(data, request.PageNumber, request.PageSize)
            {
                TotalRecords = totalRows
            };

            return response;
        }
    }

    public class GetProductsPagingQueryResponse : PaginationResponse<ICollection<ProductViewModel>>
    {
        public GetProductsPagingQueryResponse(ICollection<ProductViewModel> products, int pageNumber, int pageSize)
            :base(products, pageNumber, pageSize)
        {
        }
    }
}
