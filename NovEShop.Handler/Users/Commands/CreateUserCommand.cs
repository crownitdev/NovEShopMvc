using Microsoft.AspNetCore.Identity;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Users.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Users.Commands
{
    public class CreateUserCommand : CreateUserRequestDto, ICommand<CreateUserCommandResponse>
    {
        public string Token { get; set; }
    }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NovEShopDbContext _db;

        public CreateUserCommandHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateUserCommandResponse();

            var userExisted = _db.Users.Where(x => x.UserName == request.UserName ||
                            x.Email == request.Email)
                .FirstOrDefault();

            if (userExisted != null)
            {
                response.Message = $"Người dùng có UserName: {request.UserName} hoặc Email: {request.Email} đã tồn tại"; ;
                response.IsSucceed = false;
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.Message = "Đã xảy ra lỗi khi tạo người dùng";
                response.IsSucceed = false;
            }
            else
            {
                response.Message = "Tạo người dùng thành công";
                response.IsSucceed = true;
            }

            return response;
        }
    }

    public class CreateUserCommandResponse : Response
    {

    }
}
