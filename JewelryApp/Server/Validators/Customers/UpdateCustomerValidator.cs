using FluentValidation;
using JewelryApp.Shared.Requests.Customer;

namespace JewelryApp.Api.Validators.Customers;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.InvoiceId).NotEqual(0).WithMessage("شماره فاکتور معتبر نیست");
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام مشتری را وارد کنید");
    }
}
