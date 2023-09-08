using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Common.Enums
{
    public enum Caret
    {
        [Display(Name = "18 عیار")]
        Eighteen = 1,

        [Display(Name = "21 عیار")]
        TwentyOne = 2,

        [Display(Name = "22 عیار")]
        TwentyTwo = 3,

        [Display(Name = "24 عیار")]
        TwentyFour = 4
    }
}
