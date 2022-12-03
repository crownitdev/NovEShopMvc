using FluentValidation;
using NovEShop.Handler.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovEShop.Handler.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được bỏ trống")
                .EmailAddress().WithMessage("Email không hợp lệ")
                .Matches(@"[a-z0-9]+@[a-z]+\.[a-z]{2,3}").WithMessage("Email không đúng định dạng");

            //RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Mật khẩu không trùng khớp");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được bỏ trống")
                .MaximumLength(200).WithMessage("Tên của bạn quá dài, xin hãy thử lại");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ không được bỏ trống")
                .MaximumLength(200).WithMessage("Họ của bạn quá dài, xin hãy thử lại");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Ngày sinh không hợp lệ");
        }
    }
}
