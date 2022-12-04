using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Categories.Dtos;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Categories.Queries
{
    public class GetAllCategoriesQuery : IQuery<GetAllCategoriesQueryResponse>
    {
        public string LanguageId { get; set; }
    }

    public class GetAllCategoriesPagingQueryHandler : IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesQueryResponse>
    {
        private readonly NovEShopDbContext _db;
        public GetAllCategoriesPagingQueryHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = from c in _db.Categories
                        join ct in _db.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == request.LanguageId
                        select new { c, ct };

            var categories = await query.Select(x => new CategoryViewModel
            {
                Id = x.c.Id,
                Name = x.ct.Name
            })
                .ToListAsync();

            GetAllCategoriesQueryResponse response = new GetAllCategoriesQueryResponse(categories);
            response.Message = "Lấy danh mục sản phẩm thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class GetAllCategoriesQueryResponse : Response<ICollection<CategoryViewModel>>
    {
        public GetAllCategoriesQueryResponse()
        {
        }

        public GetAllCategoriesQueryResponse(ICollection<CategoryViewModel> data)
            : base(data)
        {
        }
    }
}
