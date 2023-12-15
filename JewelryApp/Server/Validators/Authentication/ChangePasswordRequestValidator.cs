using FluentValidation;
using JewelryApp.Shared.Requests.Authentication;

namespace JewelryApp.Api.Validators.Authentication;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("لطفا نام کاربری را وارد کنید");
        RuleFor(x => x.OldPassword).NotEmpty().WithMessage("لطفا رمز عبور فعلی را وارد کنید");
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("لطفا رمز عبور فعلی را وارد کنید");
        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("لطفا رمز عبور فعلی را وارد کنید")
            .Equal(x => x.NewPassword).WithMessage("رمز عبور جدید با تکرار آن مطابقت ندارد");
    }
}