using FluentValidation;
using JewelryApp.Core.Attributes;
using JewelryApp.Shared.Requests.Customer;

namespace JewelryApp.Api.Validators.Customers;

[ScopedService<IValidator<AddCustomerRequest>>]
public class AddCustomerRequestValidator : AbstractValidator<AddCustomerRequest>
{
    public AddCustomerRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام مشتری را وارد کنید");
    }
}
