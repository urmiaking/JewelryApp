using FluentValidation;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.OldGolds;

namespace JewelryApp.Api.Validators.OldGolds;

[ScopedService<IValidator<AddOldGoldRequest>>]
public class AddOldGoldValidator : AbstractValidator<AddOldGoldRequest>
{
    public AddOldGoldValidator()
    {
        RuleFor(x => x.Weight).NotEmpty().WithMessage("لطفا وزن جنس را وارد کنید");
    }
}