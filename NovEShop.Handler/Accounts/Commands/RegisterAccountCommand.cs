using Microsoft.AspNetCore.Identity;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Accounts.Dtos;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Accounts.Commands
{
    public class RegisterAccountCommand : RegisterRequestDto, ICommand<RegisterAccountCommandResponse>
    {
    }

    public class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand, RegisterAccountCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterAccountCommandHandler(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<RegisterAccountCommandResponse> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class RegisterAccountCommandResponse : Response
    {
    }
}
