using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class Invoice
    {
        public static Error NotFound =>
            Error.NotFound(code: "Invoice.NotFoundError", description: "فاکتور یافت نشد");

        public static Error Exists =>
            Error.NotFound(code: "Invoice.ExistsError", description: "شماره فاکتور وارد شده از قبل وجود دارد");
    }
}