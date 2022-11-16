using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Products.Queries
{
    public class GetProductsHomePageQuery : IQuery<GetProductsHomePageQueryResponse>
    {
    }

    public class GetProductsHomePageQueryHandler : IQueryHandler<GetProductsHomePageQuery, GetProductsHomePageQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetProductsHomePageQueryHandler(NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetProductsHomePageQueryResponse> Handle(GetProductsHomePageQuery request, CancellationToken cancellationToken)
        {
            var query = from p in _db.Products
                        join pc in _db.ProductCategories on p.Id equals pc.ProductId
                        join pt in _db.ProductTranslations on p.Id equals pt.ProductId
                        join c in _db.Categories on pc.CategoryId equals c.Id
                        join ct in _db.CategoryTranslations on c.Id equals ct.CategoryId
                        select new { p, pc, pt, ct };

            GetProductsHomePageQueryResponse response = new GetProductsHomePageQueryResponse();
            response.Data.TShirts = await query.Where(x => x.ct.Name.Contains("TShirt"))
                .Select(d => new ProductMetaViewModel()
                {
                    ProductId = d.p.Id,
                    Name = d.pt.Name,
                    Price = d.p.Price.Value
                })
                .ToListAsync();

            response.Data.Hats = await query.Where(x => string.Compare(x.ct.Name, "Hat") >= 0)
                .Select(d => new ProductMetaViewModel()
                {
                    ProductId = d.p.Id,
                    Name = d.pt.Name,
                    Price = d.p.Price.Value
                })
                .ToListAsync();

            response.Data.Hats = await query.Where(x => string.Compare(x.ct.Name, "Clock") >= 0)
                .Select(d => new ProductMetaViewModel()
                {
                    ProductId = d.p.Id,
                    Name = d.pt.Name,
                    Price = d.p.Price.Value
                }).ToListAsync();

            return response;
        }
    }

    public class GetProductsHomePageQueryResponse : Response<ProductsHomeRequest>
    {
    }
}
