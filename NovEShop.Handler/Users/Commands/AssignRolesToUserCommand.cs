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
    public class AssignRolesToUserCommand : RoleAssignRequestDto, ICommand<AssignRolesToUserCommandResponse>
    {
        public string TokenAuth { get; set; }
    }

    public class AssignRolesToUserCommandHandler : ICommandHandler<AssignRolesToUserCommand, AssignRolesToUserCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NovEShopDbContext _db;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AssignRolesToUserCommandHandler(
            NovEShopDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
        }

        public async Task<AssignRolesToUserCommandResponse> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var response = new AssignRolesToUserCommandResponse();

            var user = _db.Users.Where(x => x.Id == request.Id).FirstOrDefault();
            if (user == null)
            {
                response.Message = $"Người dùng {request.Id} không tồn tại";
                response.IsSucceed = false;

                return response;
            }

            var removeRoles = request.Roles.Where(x => x.Selected == false)
                .Select(x => x.Name)
                .ToList();
            foreach (var roleName in removeRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            

            //await _userManager.RemoveFromRolesAsync(user, removeRoles);

            var addedRoles = request.Roles.Where(x => x.Selected == true)
                .Select(x => x.Name)
                .ToList();

            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            response.Message = $"Chỉnh sửa Role cho người dùng {request.Id} thành công";
            response.IsSucceed = true;
            return response;
        }
    }

    public class AssignRolesToUserCommandResponse : Response
    {
    }
}
