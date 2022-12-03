using Microsoft.AspNetCore.Identity;
using NovEShop.Data;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Users.Commands
{
    public class DeleteUserCommand : ICommand<DeleteUserCommandResponse>
    {
        public string TokenAuth { get; set; }
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NovEShopDbContext _db;

        public DeleteUserCommandHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteUserCommandResponse();

            var user = _db.Users.Find(request.Id);

            if (user == null)
            {
                response.Message = "User không tồn tại, không thể xoá";
                response.IsSucceed = false;

                return response;
            }

            var deleteResult = await _userManager.DeleteAsync(user);

            if (deleteResult.Succeeded)
            {
                response.Message = $"Xoá người dùng {request.Id} thành công";
                response.IsSucceed = true;
            }
            else
            {
                response.Errors = new List<string>();
                foreach (var error in deleteResult.Errors)
                {
                    response.Errors.Add(error.Description);
                }
                response.Message = $"Xoá người dùng {request.Id} thất bại";
                response.IsSucceed = false;
            }

            return response;
        }
    }

    public class DeleteUserCommandResponse : Response
    {
    }
}
