using FluentValidation;
using JewelryApp.Shared.Requests.ProductCategories;

namespace JewelryApp.Api.Validators.ProductCategories;

public class AddProductCategoryRequestValidator : AbstractValidator<AddProductCategoryRequest>
{
    public AddProductCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید");
    }
}