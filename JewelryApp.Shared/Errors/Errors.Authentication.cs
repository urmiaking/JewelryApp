using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Conflict(code: "Authentication.InvalidCredentialsError", description: "نام کاربری یا رمز عبور اشتباه است.");

        public static Error InvalidToken =>
            Error.Validation(code: "Authentication.InvalidTokenError", description: "توکن نامعتبر است. لطفا دوباره وارد شوید.");

        public static Error PasswordNotValid =>
            Error.Conflict(code: "Authentication.PasswordNotValidError", description: "تغییر رمز انجام نشد. لطفا مجددا امتحان کنید");

        public static Error NotAuthenticated =>
            Error.Conflict(code: "Authentication.NotAuthenticatedError", description: "احراز هویت انجام نشد. لطفا وارد سیستم شوید");

        public static Error Forbidden =>
            Error.Conflict(code: "Authentication.ForbiddenError", description: "شما دسترسی لازم برای این قابلیت را ندارید");
    }
}
