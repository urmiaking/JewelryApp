using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class Invoice
    {
        public static Error NotFound =>
            Error.NotFound(code: "Invoice.NotFoundError", description: "فاکتور یافت نشد");

        public static Error Exists =>
            Error.Conflict(code: "Invoice.ExistsError", description: "شماره فاکتور وارد شده از قبل وجود دارد");

        public static Error Deleted =>
            Error.Conflict(code: "Invoice.DeletedError", description: "فاکتور مورد نظر از قبل حذف شده است");
    }
}