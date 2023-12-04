using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Price.Signal;

public class SignalApiBody
{
    [JsonProperty("property")]
    public string[] Property { get; set; } = default!;

    [JsonProperty("sortBy")]
    public string SortBy { get; set; } = default!;

    [JsonProperty("desc")]
    public bool Desc { get; set; }

    [JsonProperty("market")]
    public string Market { get; set; } = default!;
}