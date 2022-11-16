using Microsoft.AspNetCore.Identity.UI.Services;
using NovEShop.Data;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Accounts.Commands
{
    public class AccountForgotPasswordCommand : ICommand<AccountForgotPasswordCommandResponse>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    //public class AccountForgotPasswordCommandHandler : ICommandHandler<AccountForgotPasswordCommand, AccountForgotPasswordCommandResponse>
    //{
    //    private readonly NovEShopDbContext _db;
    //    private readonly IEmailSender _emailSender;

    //    public AccountForgotPasswordCommandHandler(NovEShopDbContext db,
    //        IEmailSender emailSender)
    //    {
    //        _db = db;
    //        _emailSender = emailSender;
    //    }

    //    public Task<AccountForgotPasswordCommandResponse> Handle(AccountForgotPasswordCommand request, CancellationToken cancellationToken)
    //    {
    //        AccountForgotPasswordCommandResponse response = new AccountForgotPasswordCommandResponse();

    //        var user = _db.Users.Find(request.UserId);
    //        if (string.Compare(user.Email, request.Email) < 0)
    //        {
    //            response.Message = "Thao tác không hợp lệ, Email không trùng khớp";
    //            response.IsSucceed = false;
    //        }


    //    }
    //}

    public class AccountForgotPasswordCommandResponse : Response
    {
        public string ResetToken { get; set; }
    }
}
