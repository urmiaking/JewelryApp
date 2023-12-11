using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Errors = JewelryApp.Core.Errors.Errors;

namespace JewelryApp.Api.Controllers;

public class AccountController : ApiController
{
    private readonly IAccountService _service;
    private readonly IValidator<AuthenticationRequest> _validator;

    public AccountController(IAccountService service, IValidator<AuthenticationRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request);

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
        catch (Exception e)
        {
            return Problem(title: e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        try
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
        catch (Exception e)
        {
            return Problem(title: e.Message);
        }
    }

    [Route("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        try
        {
            var response = await _service.ChangePasswordAsync(request);

            if (response.IsError)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: response.FirstError.Description);
            }

            return response.Match(Ok, Problem);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message);
        }
    }
}

