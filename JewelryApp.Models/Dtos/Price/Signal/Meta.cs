using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Price.Signal;

public class Meta
{
    [JsonProperty("shamsiDate")]
    public string ShamsiDate { get; set; } = default!;

    [JsonProperty("requestId")]
    public string RequestId { get; set; } = default!;
}