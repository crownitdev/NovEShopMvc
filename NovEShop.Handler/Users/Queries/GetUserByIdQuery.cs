using Microsoft.AspNetCore.Identity;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Users.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Users.Queries
{
    public class GetUserByIdQuery : IQuery<GetUserByIdQueryResponse>
    {
        public int Id { get; set; }
        public string TokenAuth { get; set; }
    }

    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdQueryHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetUserByIdQueryResponse();

            if (string.IsNullOrEmpty(request.Id.ToString()))
            {
                response.IsSucceed = false;
                response.Message = "Id người dùng không tồn tại";

                return response;
            }

            var user = _userManager.Users.Where(x => x.Id == request.Id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IsActive = x.IsActive,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName
                })
                .FirstOrDefault();

            if (user != null)
            {
                response.Data = user;
                response.Message = "Lấy dữ liệu người dùng thành công";
                response.IsSucceed = true;
            }
            else
            {
                response.Message = "Người dùng không tồn tại";
                response.IsSucceed = false;
            }

            return response;
        }
    }

    public class GetUserByIdQueryResponse : Response<UserViewModel>
    {
    }
}
