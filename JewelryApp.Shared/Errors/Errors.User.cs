using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class User
    {
        public static Error NotFound
            => Error.NotFound(code: "User.NotFoundError", "کاربر یافت نشد.");
    }
}
