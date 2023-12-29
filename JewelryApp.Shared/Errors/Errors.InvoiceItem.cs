using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class InvoiceItem
    {
        public static Error NotFound => 
            Error.NotFound(code: "InvoiceItem.NotFoundError", description: "جنس موردنظر یافت نشد");

        public static Error Deleted =>
            Error.Conflict(code: "InvoiceItem.DeletedError", description: "جنس مورد نظر در فاکتور مربوطه از قبل حذف شده است");

        public static Error Exists =>
            Error.Conflict(code: "InvoiceItem.ExistsError", description: "جنس مورد نظر از قبل به فاکتور اضافه شده است");
    }
}