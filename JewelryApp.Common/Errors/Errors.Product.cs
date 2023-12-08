using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class Product
    {
        public static Error NotFound
            => Error.NotFound(code: "Product.NotFoundError", description: "جنس مورد نطر یافت نشد.");

        public static Error Sold
            => Error.Conflict(code: "Product.SoldError", description: "جنس مورد نطر فروخته شده است.");
    }
}
