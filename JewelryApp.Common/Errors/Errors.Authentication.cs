using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Conflict(code: "Authentication.InvalidCredentialsError", description: "نام کاربری یا رمز عبور اشتباه است.");

        public static Error InvalidToken =>
            Error.Validation(code: "Authentication.InvalidTokenError", description: "توکن نامعتبر است. لطفا دوباره وارد شوید.");

        public static Error PasswordNotValid =>
            Error.Validation(code: "Authentication.PasswordNotValidError", description: "رمز عبور جدید باید حداقل 6 رقم باشد.");
    }
}
