using ErrorOr;

namespace JewelryApp.Core.Errors;

public partial class Errors
{
    public static class General
    {
        public static Error ServerError =>
            Error.Failure(code: "General.ServerError", description: "خطایی در ارتباط با سرور رخ داد");

        public static Error NoInternet =>
            Error.Failure(code: "General.NoInternetError", description: "خطایی در ارتباط با API رخ داد");
    }
}
