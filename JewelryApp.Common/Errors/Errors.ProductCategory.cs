using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class ProductCategory
    {
        public static Error NotFound
            => Error.NotFound(code: "ProductCategory.NotFoundError", description: "دسته بندی مورد نظر یافت نشد");

        public static Error Exists 
            => Error.Conflict(code: "ProductCategory.ExistsError", description: "دسته بندی مورد نظر از قبل موجود می باشد");

        public static Error Used
            => Error.Conflict(code: "ProductCategory.UsedError", description: "دسته بندی مورد نظر توسط اجناسی در حال استفاده می باشد");

        public static Error Deleted
            => Error.Conflict(code: "ProductCategory.DeletedError", description: "دسته بندی مورد نظر قبلا حذف شده است");
    }
}