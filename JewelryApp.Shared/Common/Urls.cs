namespace JewelryApp.Shared.Common;

public static class Urls
{
    #region Account

    public const string Login = "/api/account/Login";
    public const string RefreshToken = "/api/account/Refresh";
    public const string ChangePassword = "/api/account/ChangePassword";
    public const string Logout = "/api/account/Logout";

    #endregion

    #region Customer

    public const string Customers = "/api/Customers";

    #endregion

    #region Product

    public const string Products = "/api/Products";

    #endregion

    #region Invoice

    public const string Invoices = "/api/Invoices";

    #endregion

    #region InvoiceItem

    public const string InvoiceItems = "/api/InvoiceItems";

    #endregion

    #region ProductCategory

    public const string ProductCategories = "/api/ProductCategories";

    #endregion

    #region Price

    public const string Price = "/api/Price";

    #endregion

    #region Old Golds

    public const string OldGolds = "/api/OldGolds";

    #endregion
}