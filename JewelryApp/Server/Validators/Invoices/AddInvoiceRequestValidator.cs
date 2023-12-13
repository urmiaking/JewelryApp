using FluentValidation;
using JewelryApp.Shared.Requests.Invoices;

namespace JewelryApp.Api.Validators.Invoices;

public class AddInvoiceRequestValidator : AbstractValidator<AddInvoiceRequest>
{
    public AddInvoiceRequestValidator()
    {
        RuleFor(x => x.InvoiceDate).NotEmpty().NotEqual(x => DateTime.MinValue).WithMessage("لطفا تاریخ فاکتور را وارد کنید");
    }
}
