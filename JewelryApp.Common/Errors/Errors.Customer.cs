using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class Customer
    {
        public static Error NotFound
            => Error.NotFound("مشتری با مشخصات وارد شده پیدا نشد");
    }
}
