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
                response.Message = "Id người dùng không hợp lệ";

                return response;
            }

            var user = _userManager.Users.Where(x => x.Id == request.Id)
                .FirstOrDefault();

            if (user == null)
            {
                response.IsSucceed = false;
                response.Message = "Người dùng không tồn tại";

                return response;
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Dob = user.Dob
            };

            var roles = await _userManager.GetRolesAsync(user);

            userViewModel.Roles = roles;

            if (user != null)
            {
                response.Data = userViewModel;
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
