﻿using FluentValidation;
using JewelryApp.Shared.Requests.Invoices;

namespace JewelryApp.Api.Validators.Invoices;

public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceRequest>
{
    public UpdateInvoiceValidator()
    {
        RuleFor(x => x.Id).NotEqual(0).WithMessage("شماره فاکتور معتبر نیست");
        RuleFor(x => x.InvoiceDate).NotEmpty().NotEqual(x => DateTime.MinValue).WithMessage("لطفا تاریخ فاکتور را وارد کنید");
    }
}

