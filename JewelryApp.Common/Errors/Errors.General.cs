using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class General
    {
        public static Error ServerError =>
            Error.Failure(code: "General.ServerError", description: "خطایی در ارتباط با سرور رخ داد");
    }
}
