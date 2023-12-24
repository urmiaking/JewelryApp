using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.ProductCategories;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ProductCategoriesController : ApiController
{
    private readonly IProductCategoryService _productCategoryService;
    private readonly IValidator<AddProductCategoryRequest> _addProductCategoryValidator;
    private readonly IValidator<UpdateProductCategoryRequest> _updateProductCategoryValidator;

    public ProductCategoriesController(IProductCategoryService productCategoryService, IValidator<AddProductCategoryRequest> addProductCategoryValidator, IValidator<UpdateProductCategoryRequest> updateProductCategoryValidator)
    {
        _productCategoryService = productCategoryService;
        _addProductCategoryValidator = addProductCategoryValidator;
        _updateProductCategoryValidator = updateProductCategoryValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _productCategoryService.GetProductCategoriesAsync(cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _productCategoryService.GetProductCategoryByIdAsync(id, cancellationToken);

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

        return response.Match(x => CreatedAtAction(nameof(GetById), new { id = x.Id }, response.Value), Problem);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
    {
        var response = await _productCategoryService.RemoveProductCategoryAsync(id, cancellationToken);

        return response.Match(Ok, Problem);
    }
}