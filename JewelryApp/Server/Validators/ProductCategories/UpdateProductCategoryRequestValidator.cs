using FluentValidation;
using JewelryApp.Core.Attributes;
using JewelryApp.Shared.Requests.ProductCategories;

namespace JewelryApp.Api.Validators.ProductCategories;

[ScopedService<IValidator<UpdateProductCategoryRequest>>]
public class UpdateProductCategoryRequestValidator : AbstractValidator<UpdateProductCategoryRequest>
{
    public UpdateProductCategoryRequestValidator()
    {
        RuleFor(x => x.Id).NotEqual(0).WithMessage("لطفا دسته بندی مورد نظر برای ویرایش را مشخص کنید");
        RuleFor(x => x.Name).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید");
    }
}