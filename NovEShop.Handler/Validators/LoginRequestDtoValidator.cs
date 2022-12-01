using FluentValidation;
using NovEShop.Handler.Accounts.Dtos;

namespace NovEShop.Handler.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username không được bỏ trống");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được bỏ trống")
                .MinimumLength(6).WithMessage("Mật khẩu không hợp lệ");
        }
    }
}
