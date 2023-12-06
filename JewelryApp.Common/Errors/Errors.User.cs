using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class User
    {
        public static Error NotFound
            => Error.NotFound(code: "User.NotFoundError", "کاربر یافت نشد.");
    }
}
