using FluentValidation;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.Customer;

namespace JewelryApp.Api.Validators.Customers;

[ScopedService<IValidator<UpdateCustomerRequest>>]
public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام مشتری را وارد کنید");
    }
}
