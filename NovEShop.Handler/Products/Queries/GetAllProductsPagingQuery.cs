using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using NovEShop.Handler.Infrastructure;
using NovEShop.Data;
using NovEShop.Handler.Paginations.Dtos;
using NovEShop.Handler.Products.Dtos;

namespace NovEShop.Handler.Products.Queries
{
    public class GetAllProductsPagingQuery : PaginationFilter, IQuery<GetAllProductsPagingQueryResponse>
    {
        public string TokenAuth { get; set; }
        public string Keyword { get; set; }
        public string CategoryIds { get; set; }
        public string LanguageId { get; set; }
        public GetAllProductsPagingQuery()
            : base(pageSize: 10, pageNumber: 1)
        {
        }
    }

    public class GetAllProductPagingQueryHandler : IQueryHandler<GetAllProductsPagingQuery, GetAllProductsPagingQueryResponse>
    {
        private readonly NovEShopDbContext _dbContext;

        public GetAllProductPagingQueryHandler(
            NovEShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAllProductsPagingQueryResponse> Handle(GetAllProductsPagingQuery request, CancellationToken cancellationToken)
        {
            var query = from p in _dbContext.Products
                        join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt };

            // Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword) ||
                                    x.pt.Description.Contains(request.Keyword));
            }

            if (!string.IsNullOrEmpty(request.CategoryIds))
            {
                //query = query.Where(x => request.CategoryIds.Contains(x.pc.CategoryId.ToString()));
            }

            var queryResponse = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                Description = x.pt.Description,
                Details = x.pt.Details,
                DateCreated = x.p.CreatedAt,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price.Value,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                IsActive = x.p.IsActive
            })
                .ToListAsync();


            //var products = await query.ToListAsync();

            var rowCount = queryResponse.Count;

            var response = new GetAllProductsPagingQueryResponse(queryResponse, request.PageNumber, request.PageSize)
            {
                TotalRecords = rowCount,
                IsSucceed = true,
                Message = $"Đã tìm thấy {rowCount} sản phẩm",
                TotalPages = Convert.ToInt32(rowCount / request.PageSize)
            };

            return response;
        }
    }

    public class GetAllProductsPagingQueryResponse : PaginationResponse<ICollection<ProductViewModel>>
    {
        public GetAllProductsPagingQueryResponse(ICollection<ProductViewModel> products, int pageNumber, int pageSize)
            : base(products, pageNumber, pageSize)
        {
        }
    }
}
