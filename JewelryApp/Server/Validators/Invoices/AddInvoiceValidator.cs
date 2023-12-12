using FluentValidation;
using JewelryApp.Shared.Requests.Invoices;

namespace JewelryApp.Api.Validators.Invoices;

public class AddInvoiceValidator : AbstractValidator<AddInvoiceRequest>
{
    public AddInvoiceValidator()
    {
        RuleFor(x => x.InvoiceDate).NotEmpty().NotEqual(x => DateTime.MinValue).WithMessage("لطفا تاریخ فاکتور را وارد کنید");
    }
}
