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
    public class GetProductMetasByCategoryNameQuery : PaginationFilter, IQuery<GetProductMetasByCategoryNameQueryResponse>
    {
        public GetProductMetasByCategoryNameQuery()
            :base(pageNumber: 1, pageSize: 10)
        {
        }

        public GetProductMetasByCategoryNameQuery(string categoryName)
            : base(pageNumber: 1, pageSize: 10)
        {
            this.CategoryName = categoryName;
        }

        public GetProductMetasByCategoryNameQuery(int pageNumber, int pageSize)
            :base(pageSize, pageNumber)
        {
        }

        public GetProductMetasByCategoryNameQuery(string categoryName, int pageNumber, int pageSize)
            : this(pageSize, pageNumber)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }

    public class GetProductMetasByCategoryNameQueryHandler : IQueryHandler<
        GetProductMetasByCategoryNameQuery, 
        GetProductMetasByCategoryNameQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductMetasByCategoryNameQueryHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductMetasByCategoryNameQueryResponse> Handle(GetProductMetasByCategoryNameQuery request, CancellationToken cancellationToken)
        {
            //var query = _db.ProductTranslations.AsNoTracking()
            //    .Include(pdt => pdt.Product)
            //    .ThenInclude(p => p.ProductCategories)
            //    .ThenInclude(productCategory => productCategory.Category)
            //    .ThenInclude(category => category.CategoryTranslations)
            //    .Where(c => c.Product.ProductCategories.)
            //    .Skip((request.PageNumber - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //    .Select(x => new ProductMetaViewModel()
            //    {
            //        ProductId = x.ProductId,
            //        Name = x.Name,
            //        Price = x.Product.Price.Value
            //    });

            var query = from p in _db.Products
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _db.ProductCategories on p.Id equals pc.ProductId
                        join ct in _db.CategoryTranslations on pc.CategoryId equals ct.CategoryId
                        select new { p, pt, ct }
                        into productJoinTable
                        where productJoinTable.ct.Name.Contains(request.CategoryName)
                        select new ProductMetaViewModel()
                        {
                            ProductId = productJoinTable.p.Id,
                            Name = productJoinTable.pt.Name,
                            Price = productJoinTable.p.Price.Value
                        };

            var queryResponse = await query.ToListAsync();
            var response = new GetProductMetasByCategoryNameQueryResponse(queryResponse,
                request.PageNumber,
                request.PageSize)
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                IsSucceed = true,
                Message = $"Đã tìm thấy {queryResponse.Count} sản phẩm"
            };

            return response;
        }
    }

    public class GetProductMetasByCategoryNameQueryResponse : PaginationResponse<ICollection<ProductMetaViewModel>>
    {
        public GetProductMetasByCategoryNameQueryResponse(ICollection<ProductMetaViewModel> data,
            int pageNumber,
            int pageSize)
            :base(data, pageNumber, pageSize)
        {
        }
    }
}
