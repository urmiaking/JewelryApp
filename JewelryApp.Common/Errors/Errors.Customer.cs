using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class Customer
    {
        public static Error NotFound
            => Error.NotFound("Customer.NotFoundError", description: "مشتری با مشخصات وارد شده پیدا نشد");

        public static Error Exists
            => Error.Conflict("Customer.ExistsError", description: "اطلاعات مشتري از قبل ثبت شده است");

        public static Error Deleted
            => Error.Conflict("Customer.DeletedError", description: "اين مشتري از قبل حذف شده است");
    }
}
