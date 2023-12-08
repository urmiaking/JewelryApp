using FluentValidation;
using JewelryApp.Common.Enums;
using JewelryApp.Shared.Requests.Products;

namespace JewelryApp.Business.Validators.Products;

public class AddProductValidator : AbstractValidator<AddProductRequest>
{
    public AddProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("نام جنس نمی تواند خالی باشد");
        RuleFor(x => x.Barcode).NotEmpty().WithMessage("بارکد نمی تواند خالی باشد");
        RuleFor(x => x.Weight).NotEmpty().NotEqual(0).WithMessage("وزن جنس نمی تواند صفر باشد");
        RuleFor(x => x.CategoryId).NotEmpty().NotEqual(0).WithMessage("لطفا زیرگروه جنس را مشخص کنید");
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
