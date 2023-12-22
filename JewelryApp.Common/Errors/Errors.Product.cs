using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class Product
    {
        public static Error NotFound
            => Error.NotFound(code: "Product.NotFoundError", description: "جنس مورد نطر یافت نشد.");

        public static Error Sold
            => Error.Conflict(code: "Product.SoldError", description: "جنس مورد نطر فروخته شده است.");

        public static Error BarcodeExists
            => Error.Conflict(code: "Product.BarcodeExistsError", description: "بارکد جنس از قبل موجود است.");

        public static Error Deleted
            => Error.Conflict(code: "Product.DeletedError", description: "جنس مورد نظر قبلا حذف شده است");
    }
}
