using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Errors = JewelryApp.Shared.Errors.Errors;

namespace JewelryApp.Api.Controllers;

public class AccountController : ApiController
{
    private readonly IAccountService _service;
    private readonly IValidator<AuthenticationRequest> _authenticationValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public AccountController(IAccountService service, IValidator<AuthenticationRequest> authenticationValidator, IValidator<ChangePasswordRequest> changePasswordValidator)
    {
        _service = service;
        _authenticationValidator = authenticationValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    [AllowAnonymous]
    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        var validationResult = await _authenticationValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _service.AuthenticateAsync(request);

        if (response.IsError && response.FirstError == Errors.Authentication.InvalidCredentials)
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: response.FirstError.Description);

        return response.Match(Ok, Problem);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Refresh))]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var response = await _service.RefreshAsync(request);

        if (response.IsError)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: response.FirstError.Description);
        }

        return response.Match(Ok, Problem);
    }

    [HttpPost(nameof(ChangePassword))]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _service.ChangePasswordAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpGet(nameof(Logout))]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _service.LogoutAsync(cancellationToken);
        return Ok();
    }
}

