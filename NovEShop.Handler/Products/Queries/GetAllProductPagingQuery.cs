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
using NovEShop.Data.Models;

namespace NovEShop.Handler.Products.Queries
{
    public class GetAllProductPagingQuery : PaginationFilter, IQuery<GetAllProductPagingQueryResposne>
    {
        public GetAllProductPagingQuery()
            : base(pageSize: 10, pageNumber: 1)
        {
        }
    }

    public class GetAllProductPagingQueryHandler : IQueryHandler<GetAllProductPagingQuery, GetAllProductPagingQueryResposne>
    {
        private readonly NovEShopDbContext _dbContext;

        public GetAllProductPagingQueryHandler(
            NovEShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAllProductPagingQueryResposne> Handle(GetAllProductPagingQuery request, CancellationToken cancellationToken)
        {
            //var query = from p in _dbContext.Products
            //            join pt in _dbContext.ProductTranslations on p.Id equals pt.ProductId
            //            join pc in _dbContext.ProductCategories on p.Id equals pc.ProductId
            //            join c in _dbContext.Categories on pc.CategoryId equals c.Id
            //            select new { p, pt, pc };

            var query = _dbContext.ProductTranslations.AsNoTracking()
                .Include(pdt => pdt.Product)
                .ThenInclude(p => p.ProductCategories)
                .ThenInclude(productCategory => productCategory.Category)
                .ThenInclude(category => category.CategoryTranslations)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductMetaViewModel()
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Price = x.Product.Price.Value
                });

            var products = await query.ToListAsync();

            //var products = await query
            //    .Skip((request.PageNumber - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //    .Select(x => new ProductViewModel()
            //    {
            //        Id = x.p.Id,
            //        Name = x.pt.Name,
            //        Description = x.pt.Description,
            //        Details = x.pt.Details,
            //        Price = x.p.Price,
            //        OriginalPrice = x.p.OriginalPrice,
            //        SeoAlias = x.pt.SeoAlias,
            //        SeoDescription = x.pt.SeoDescription,
            //        SeoTitle = x.pt.SeoTitle,
            //        Stock = x.p.Stock,
            //        ViewCount = x.p.ViewCount,
            //        DateCreated = x.p.CreatedAt,
            //        LanguageId = x.pt.LanguageId
            //    })
            //    .ToListAsync();

            var rowCount = products.Count;

            var response = new GetAllProductPagingQueryResposne(products, request.PageNumber, request.PageSize)
            {
                TotalRecords = rowCount,
                IsSucceed = true,
                Message = $"Đã tìm thấy {rowCount} sản phẩm",
                TotalPages = Convert.ToInt32(rowCount / request.PageSize)
            };

            return response;
        }
    }

    public class GetAllProductPagingQueryResposne : PaginationResponse<ICollection<ProductMetaViewModel>>
    {
        public GetAllProductPagingQueryResposne(ICollection<ProductMetaViewModel> products, int pageNumber, int pageSize)
            : base(products, pageNumber, pageSize)
        {
        }
    }
}
