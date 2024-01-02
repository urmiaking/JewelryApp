using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Populated;

public class CaratVm : PopulatedBaseVm
{
    public static IEnumerable<CaratVm> GetCarats()
    {
        return from CaratType caratType in Enum.GetValues(typeof(CaratType)) select new CaratVm { Id = (int)caratType, Name = caratType.GetDisplayName() };
    }
}