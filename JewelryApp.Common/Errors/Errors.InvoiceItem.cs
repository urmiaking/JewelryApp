﻿using ErrorOr;

namespace JewelryApp.Common.Errors;

public partial class Errors
{
    public static class InvoiceItem
    {
        public static Error NotFound => 
            Error.NotFound(code: "NotFoundError", description: "جنس موردنظر در فاکتور مربوطه یافت نشد");
    }
}