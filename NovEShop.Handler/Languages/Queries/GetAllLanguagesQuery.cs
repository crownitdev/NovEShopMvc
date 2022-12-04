using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Languages.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Languages.Queries
{
    public class GetAllLanguagesQuery : IQuery<GetAllLanguagesQueryResponse>
    {
    }

    public class GetAllLanguagesQueryHandler : IQueryHandler<GetAllLanguagesQuery, GetAllLanguagesQueryResponse>
    {
        private readonly NovEShopDbContext _db;

        public GetAllLanguagesQueryHandler(
            NovEShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllLanguagesQueryResponse> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
        { 
            var languages = await _db.Languages.Select(x => new LanguageViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
                .ToListAsync();

            var response = new GetAllLanguagesQueryResponse();
            response.Data = languages;
            response.Message = $"Đã tìm thấy {response.Data.Count} ngôn ngữ";
            response.IsSucceed = true;

            return response;
        }
    }

    public class GetAllLanguagesQueryResponse : Response<ICollection<LanguageViewModel>>
    {
    }
}
