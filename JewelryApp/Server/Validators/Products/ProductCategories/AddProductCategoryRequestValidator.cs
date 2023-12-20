using FluentValidation;
using JewelryApp.Core.Attributes;
using JewelryApp.Shared.Requests.ProductCategories;

namespace JewelryApp.Api.Validators.Products.ProductCategories;

[ScopedService<IValidator<AddProductCategoryRequest>>]
public class AddProductCategoryRequestValidator : AbstractValidator<AddProductCategoryRequest>
{
    public AddProductCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید");
    }
}