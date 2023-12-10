using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class Invoice
    {
        public static Error NotFound =>
            Error.NotFound(code: "Invoice.NotFoundError", description: "فاکتور یافت نشد");
    }
}