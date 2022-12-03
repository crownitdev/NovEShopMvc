using Microsoft.AspNetCore.Identity;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Users.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Users.Commands
{
    public class UpdateUserCommand : UpdateUserRequestDto, ICommand<UpdateUserCommandResponse>
    {
        public string TokenAuth { get; set; }
    }

    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly NovEShopDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateUserCommandResponse();

            var user = _db.Users.Find(request.Id);

            if (user == null)
            {
                response.Message = "User không tồn tại, không thể cập nhật";
                response.IsSucceed = false;

                return response;
            }
            // validate new email
            var isEmailExisted = await _userManager.FindByEmailAsync(request.Email);
            if (isEmailExisted.UserName != user.UserName)
            {
                response.Message = "Email đã tồn tại, hãy thử email khác";
                response.IsSucceed = false;

                return response;
            }

            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.Dob = request.Dob;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var saveChangeState = await _db.SaveChangesAsync();

            if (saveChangeState >= 0)
            {
                response.Message = $"Cập nhật người dùng {request.Id} thành công";
                response.IsSucceed = true;
            }
            else
            {
                response.Message = $"Cập nhật người dùng {request.Id} thất bại";
                response.IsSucceed = false;
            }

            return response;
        }
    }

    public class UpdateUserCommandResponse : Response
    {
    }
}
