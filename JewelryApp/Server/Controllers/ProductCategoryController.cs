using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.ProductCategories;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ProductCategoryController : ApiController
{
    private readonly IProductCategoryService _productCategoryService;
    private readonly IValidator<AddProductCategoryRequest> _addProductCategoryValidator;
    private readonly IValidator<UpdateProductCategoryRequest> _updateProductCategoryValidator;

    public ProductCategoryController(IProductCategoryService productCategoryService, IValidator<AddProductCategoryRequest> addProductCategoryValidator, IValidator<UpdateProductCategoryRequest> updateProductCategoryValidator)
    {
        _productCategoryService = productCategoryService;
        _addProductCategoryValidator = addProductCategoryValidator;
        _updateProductCategoryValidator = updateProductCategoryValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _productCategoryService.GetProductCategoriesAsync(cancellationToken));

    [HttpGet(nameof(Get))]
    public async Task<IActionResult> Get([FromQuery] GetProductCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _productCategoryService.GetProductCategoryAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductCategoryRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _addProductCategoryValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _productCategoryService.AddProductCategoryAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateProductCategoryValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _productCategoryService.UpdateProductCategoryAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveProductCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await _productCategoryService.RemoveProductCategoryAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }
}