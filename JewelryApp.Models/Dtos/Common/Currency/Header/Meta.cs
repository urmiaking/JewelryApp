using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Common.Currency.Header;

public class Meta
{
    [JsonProperty("shamsiDate")]
    public string ShamsiDate { get; set; }

    [JsonProperty("requestId")]
    public string RequestId { get; set; }
}
