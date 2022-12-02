using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Paginations.Dtos;
using NovEShop.Handler.Users.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Users.Queries
{
    public class GetAllUsersPagingQuery : GetAllUsersPagingRequest, IQuery<GetAllUsersPagingQueryResponse>
    {
        public string BearerToken { get; set; }
    }

    public class GetAllUsersPagingQueryHandler : IQueryHandler<GetAllUsersPagingQuery, GetAllUsersPagingQueryResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersPagingQueryHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<GetAllUsersPagingQueryResponse> Handle(GetAllUsersPagingQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) ||
                                x.PhoneNumber.Contains(request.Keyword) ||
                                x.FirstName.Contains(request.Keyword) ||
                                x.LastName.Contains(request.Keyword));
            }

            int totalRows = await query.CountAsync();

            var data = await query.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IsActive = x.IsActive
                })
                .ToListAsync();


            var response = new GetAllUsersPagingQueryResponse(data, request.PageNumber, request.PageSize);
            response.Message = $"Đã tìm thấy {totalRows} người dùng";
            response.IsSucceed = true;

            return response;
        }
    }

    public class GetAllUsersPagingQueryResponse : PaginationResponse<IEnumerable<UserViewModel>>
    {
        public GetAllUsersPagingQueryResponse(IEnumerable<UserViewModel> data, int pageNumber, int pageSize)
            :base(data, pageNumber: pageNumber, pageSize: pageSize)
        {
        }
    }
}
