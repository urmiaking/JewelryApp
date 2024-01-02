using JewelryApp.Shared.Enums;

namespace JewelryApp.Client.ViewModels.Populated;

public class WageTypeVm : PopulatedBaseVm
{
    public static IEnumerable<WageTypeVm> GetWageTypes()
    {
        return from WageType wageType in Enum.GetValues(typeof(WageType)) select new WageTypeVm { Id = (int)wageType, Name = wageType.GetDisplayName() };
    }
}