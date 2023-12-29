using FluentValidation;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.Authentication;

namespace JewelryApp.Api.Validators.Authentication;

[ScopedService<IValidator<AuthenticationRequest>>]
public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
{
    public AuthenticationRequestValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(5);
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6);
    }
}
