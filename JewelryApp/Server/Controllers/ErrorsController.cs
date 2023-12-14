using JewelryApp.Api.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ErrorsController : ApiController
{
    private readonly ILogger<ErrorsController> _logger;

    public ErrorsController(ILogger<ErrorsController> logger)
    { 
        _logger = logger;
    }

    [HttpGet("/error")]
    [AllowAnonymous]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exception switch
        {
            IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
            _ => (StatusCodes.Status500InternalServerError, exception?.Message)
        };

        _logger.LogError(exception, message);

        return Problem(statusCode: statusCode, title: message);
    }
}
