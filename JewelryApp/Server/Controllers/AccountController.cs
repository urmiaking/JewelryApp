using JewelryApp.Business.AppServices;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var response = await _service.AuthenticateAsync(request);

        if (response != null)
            return Ok(response);

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(UserTokenDto request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var response = await _service.RefreshAsync(request);

        if (response != null)
            return Ok(response);

        return Unauthorized();
    }

}

