using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ErrorController : ApiController
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    { 
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        _logger.LogError(exception, exception?.Message);

        return Problem(statusCode: StatusCodes.Status500InternalServerError, title: exception?.Message);
    }
}
