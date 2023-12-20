using JewelryApp.Core.Exceptions;
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

        var statusCode = exception switch
        {
            ForbiddenAccessException => StatusCodes.Status403Forbidden,
            UnauthenticatedException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
        
        _logger.LogError(exception, exception?.Message);

        return Problem(statusCode: statusCode, title: exception?.Message);
    }
}
