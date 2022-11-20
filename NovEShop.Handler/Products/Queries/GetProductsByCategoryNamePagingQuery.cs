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
    public class GetProductsByCategoryNamePagingQuery : PaginationFilter, IQuery<GetProductsByCategoryNamePagingQueryResponse>
    {
        public string CategoryName { get; set; }
    }

    public class GetProductsByCategoryNamePagingQueryHandler : IQueryHandler<
        GetProductsByCategoryNamePagingQuery,
        GetProductsByCategoryNamePagingQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductsByCategoryNamePagingQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductsByCategoryNamePagingQueryResponse> Handle(GetProductsByCategoryNamePagingQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                return null;
            }

            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _db.ProductCategories on p.Id equals pc.ProductId
                        join ct in _db.CategoryTranslations on pc.CategoryId equals ct.CategoryId
                        select new { p, pt, ct };


            var dataResponse = await query.Where(c => c.ct.Name.Contains(request.CategoryName))
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductMetaViewModel()
                {
                    ProductId = p.p.Id,
                    Name = p.pt.Name,
                    Price = p.p.Price.Value
                })
                .ToListAsync(cancellationToken);

            //var queryResponse = await query.ToListAsync();

            var response = new GetProductsByCategoryNamePagingQueryResponse(
                dataResponse,
                request.PageNumber,
                request.PageSize)
            {
                IsSucceed = true,
                Message = "Lấy dữ liệu thành công",
                TotalRecords = dataResponse.Count,
                TotalPages = Convert.ToInt32(dataResponse.Count / request.PageSize)
            };

            return response;
        }
    }

    public class GetProductsByCategoryNamePagingQueryResponse : PaginationResponse<ICollection<ProductMetaViewModel>>
    {
        public GetProductsByCategoryNamePagingQueryResponse(ICollection<ProductMetaViewModel> data,
            int pageNumber,
            int pageSize)
            : base(data, pageNumber, pageSize)
        {
        }
    }
}
