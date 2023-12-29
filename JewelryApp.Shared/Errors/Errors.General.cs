using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class General
    {
        public static Error ServerError =>
            Error.Failure(code: "General.ServerError", description: "خطایی در ارتباط با سرور رخ داد");

        public static Error NoInternet =>
            Error.Failure(code: "General.NoInternetError", description: "خطایی در ارتباط با API رخ داد");

        public static Error Unsupported =>
            Error.Failure(code: "General.UnsupportedError", description: "خطایی در ارسال اطلاعات رخ داد. لطفا با ادمین سیستم ارتباط بگیرید");
    }
}
