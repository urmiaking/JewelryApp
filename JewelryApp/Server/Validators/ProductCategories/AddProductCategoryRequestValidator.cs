using FluentValidation;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.ProductCategories;

namespace JewelryApp.Api.Validators.ProductCategories;

[ScopedService<IValidator<AddProductCategoryRequest>>]
public class AddProductCategoryRequestValidator : AbstractValidator<AddProductCategoryRequest>
{
    public AddProductCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید");
    }
}