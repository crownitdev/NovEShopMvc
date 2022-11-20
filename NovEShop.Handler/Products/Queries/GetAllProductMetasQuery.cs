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
    public class GetAllProductMetasQuery : PaginationFilter, IQuery<GetAllAsProductMetasQueryResponse>
    {
        public GetAllProductMetasQuery()
            : base(pageSize: 10, pageNumber: 1)
        {
        }

        public GetAllProductMetasQuery(int pageNumber, int pageSize)
            : base(pageSize, pageNumber)
        { }
    }

    public class GetAllAsProductMetasQueryHandler : IQueryHandler<GetAllProductMetasQuery, GetAllAsProductMetasQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetAllAsProductMetasQueryHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllAsProductMetasQueryResponse> Handle(GetAllProductMetasQuery request, CancellationToken cancellationToken)
        {
            var query = _db.ProductTranslations.AsNoTracking()
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

            var rowCount = products.Count;

            var response = new GetAllAsProductMetasQueryResponse(products, request.PageNumber, request.PageSize)
            {
                TotalRecords = rowCount,
                IsSucceed = true,
                Message = $"Đã tìm thấy {rowCount} sản phẩm",
                TotalPages = Convert.ToInt32(rowCount / request.PageSize)
            };

            return response;
        }
    }

    public class GetAllAsProductMetasQueryResponse : PaginationResponse<ICollection<ProductMetaViewModel>>
    {
        public GetAllAsProductMetasQueryResponse(ICollection<ProductMetaViewModel> data,
            int pageNumber,
            int pageSize)
            : base(data, pageNumber, pageSize)
        {
        }
    }
}
