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
    public class GetProductsByNameQuery : IQuery<GetProductsByNameQueryResponse>
    {
        public string Keyword { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetProductsByNameQueryHandler : IQueryHandler<GetProductsByNameQuery, GetProductsByNameQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductsByNameQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductsByNameQueryResponse> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _db.ProductCategories on p.Id equals pc.ProductId
                        join c in _db.Categories on pc.CategoryId equals c.Id
                        select new { p, pt, pc };

            if (request.Keyword.Length > 0)
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            var products = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.CreatedAt,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    LanguageId = x.pt.LanguageId
                })
                .ToListAsync();

            var rowCount = products.Count;

            var response = new GetProductsByNameQueryResponse(data: products, request.PageNumber, request.PageSize)
            {
                TotalRecords = rowCount,
                TotalPages = Convert.ToInt32(rowCount / request.PageSize),
                IsSucceed = true
            };


            return response;
        }
    }

    public class GetProductsByNameQueryResponse : PaginationResponse<ICollection<ProductViewModel>>
    {
        public GetProductsByNameQueryResponse(ICollection<ProductViewModel> data, int pageNumber, int pageSize)
            :base(data, pageNumber, pageSize)
        {
        }
    }
}
