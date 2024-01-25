using ErrorOr;
using JewelryApp.Shared.Responses.OldGolds;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class OldGolds
    {
        public static Error NotFound =>
            Error.NotFound("OldGolds.NotFoundError", "طلای کهنه مورد نظر یافت نشد");

        public static Error Deleted =>
            Error.Conflict("OldGolds.DeletedError", "طلای کهنه مورد نظر از قبل حذف شده است");
    }
}