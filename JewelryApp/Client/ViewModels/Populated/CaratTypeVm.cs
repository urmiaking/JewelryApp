using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Populated;

public class CaratTypeVm : PopulatedBaseVm
{
    public static IEnumerable<CaratTypeVm> GetCarats()
    {
        return from CaratType caratType in Enum.GetValues(typeof(CaratType)) select new CaratTypeVm { Id = (int)caratType, Name = caratType.GetDisplayName() };
    }
}