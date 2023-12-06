using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Errors;
using JewelryApp.Shared.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Api.Controllers;

public class AccountController : ApiController
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

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
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _service.RefreshAsync(request);

            if (response.IsError)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: response.FirstError.Description);
            }

            return response.Match(Ok, Problem);
        }
        catch (ValidationException e)
        {
            ModelState.AddModelError(nameof(request.RefreshToken), e.Message);
            return Unauthorized(ModelState);
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

