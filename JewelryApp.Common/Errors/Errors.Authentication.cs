using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Validation(code: "Authentication.InvalidCredentialsError", description: "نام کاربری یا رمز عبور اشتباه است.");

        public static Error InvalidToken =>
            Error.Validation(code: "Authentication.InvalidTokenError", description: "توکن نامعتبر است. لطفا دوباره وارد شوید.");

        public static Error TokenNotExpired =>
            Error.Validation(code: "Authentication.TokenNotExpiredError", description: "توکن هنوز منقضی نشده است.");

        public static Error TokenExpired =>
            Error.Validation(code: "Authentication.TokenExpiredError", description: "توکن منقضی شده است.");

        public static Error RefreshTokenNotFound =>
            Error.Validation(code: "Authentication.RefreshTokenNotFoundError", description: "توکن یافت نشد، لطفا دوباره وارد شوید.");

        public static Error RefreshTokenInvalidated =>
            Error.Validation(code: "Authentication.RefreshTokenInvalidatedError", description: "توکن باطل شده است.");

        public static Error RefreshTokenUsed =>
            Error.Validation(code: "Authentication.RefreshTokenUsedError", description: "توکن قبلا استفاده شده است.");

        public static Error RefreshTokenNotValid =>
            Error.Validation(code: "Authentication.RefreshTokenNotValidError", description: "توکن معتبر نیست.");

        public static Error PasswordNotValid =>
            Error.Validation(code: "Authentication.PasswordNotValidError", description: "رمز عبور جدید باید حداقل 6 رقم باشد.");
    }
}
