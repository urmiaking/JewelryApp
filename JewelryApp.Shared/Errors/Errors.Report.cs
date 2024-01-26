using ErrorOr;

namespace JewelryApp.Shared.Errors;

public partial class Errors
{
    public static class Report
    {
        public static Error FileNotFound =>
            Error.NotFound("Report.FileNotFoundError", "فایل مورد نظر یافت نشد");
    }
}