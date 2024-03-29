﻿using FluentValidation;
using JewelryApp.Core.Enums;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Enums;
using JewelryApp.Shared.Requests.Products;

namespace JewelryApp.Api.Validators.Products;

[ScopedService<IValidator<AddProductRequest>>]
public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("نام جنس نمی تواند خالی باشد");
        RuleFor(x => x.Barcode).NotEmpty().WithMessage("بارکد نمی تواند خالی باشد");
        RuleFor(x => x.Weight).NotEmpty().NotEqual(0).WithMessage("وزن جنس نمی تواند صفر باشد");
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("لطفا زیرگروه جنس را مشخص کنید")
            .NotEqual(0).WithMessage("لطفا زیرگروه جنس را مشخص کنید");
        RuleFor(x => x.CaratType)
            .NotEmpty()
            .Must(caratType => Enum.IsDefined(typeof(CaratType), caratType))
            .WithMessage("نوع عیار جنس اشتباه می باشد");
        RuleFor(x => x.ProductType)
            .NotEmpty()
            .Must(caratType => Enum.IsDefined(typeof(ProductType), caratType))
            .WithMessage("نوع جنس اشتباه می باشد");
        RuleFor(x => x.WageType)
            .NotEmpty()
            .Must(caratType => Enum.IsDefined(typeof(WageType), caratType))
            .WithMessage("نوع اجرت جنس اشتباه می باشد");
    }
}
