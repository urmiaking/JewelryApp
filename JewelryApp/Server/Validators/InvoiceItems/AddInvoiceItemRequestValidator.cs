﻿using FluentValidation;
using JewelryApp.Core.Attributes;
using JewelryApp.Shared.Requests.InvoiceItems;

namespace JewelryApp.Api.Validators.InvoiceItems;

[ScopedService<IValidator<AddInvoiceItemRequest>>]
public class AddInvoiceItemRequestValidator : AbstractValidator<AddInvoiceItemRequest>
{
    public AddInvoiceItemRequestValidator()
    {
        RuleFor(x => x.TaxOffset).LessThanOrEqualTo(100).WithMessage("مالیات باید کمتر یا مساوی 100 باشد");
        RuleFor(x => x.Profit).LessThanOrEqualTo(100).WithMessage("سود باید کمتر یا مساوی 100 باشد");
        RuleFor(x => x.GramPrice).NotEqual(0).WithMessage("لطفا نرخ گرم را وارد کنید");
        RuleFor(x => x.ProductId).NotEqual(0).WithMessage("لطفا جنس را وارد کنید");
        RuleFor(x => x.InvoiceId).NotEqual(0).WithMessage("لطفا فاکتور را وارد کنید");
    }
}
