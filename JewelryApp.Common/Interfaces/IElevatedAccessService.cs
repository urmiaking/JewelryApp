namespace JewelryApp.Core.Interfaces;

public interface IElevatedAccessService
{
    bool IsAdminUser();
    bool IsMainUser();
    Guid? GetUserId();
}