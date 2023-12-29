using ErrorOr;
using System.Net.Http.Json;
using System.Net;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Errors;

namespace JewelryApp.Client.Services.Extensions;

public static class HttpExtensions
{
    public static async Task<ErrorOr<TResponse>> GenerateErrorOrResponseAsync<TResponse>(this HttpResponseMessage response, CancellationToken token = default)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: token) ??
                       ErrorOr<TResponse>.From(new List<Error> { Errors.General.ServerError });

            case HttpStatusCode.Created:
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: token) ??
                       ErrorOr<TResponse>.From(new List<Error> { Errors.General.ServerError });

            case HttpStatusCode.BadRequest:
                var badRequestResult = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: token);
                var firstError = badRequestResult?.Errors.Select(x => x.Value.FirstOrDefault()).FirstOrDefault();
                return Error.Validation(description: firstError ?? string.Empty);

            case HttpStatusCode.Unauthorized:
                return Errors.Authentication.NotAuthenticated;

            case HttpStatusCode.Forbidden:
                return Errors.Authentication.Forbidden;

            case HttpStatusCode.NotFound:
                var notFoundResult = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: token);
                return Error.Validation(description: notFoundResult?.Title ?? string.Empty);

            case HttpStatusCode.Conflict:
                var conflictResult = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: token);
                return Error.Validation(description: conflictResult?.Title ?? string.Empty);

            case HttpStatusCode.UnsupportedMediaType:
                return Errors.General.Unsupported;

            default:
                return Errors.General.ServerError;
        }
    }

    public static async Task<TResponse?> GenerateResponseAsync<TResponse>(this HttpResponseMessage response, CancellationToken token = default)
    {
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: token);
    }
}