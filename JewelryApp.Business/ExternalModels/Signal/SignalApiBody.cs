using System.Text.Json.Serialization;

namespace JewelryApp.Business.ExternalModels.Signal;

public class SignalApiBody
{
    [JsonPropertyName("property")]
    public string[] Property { get; set; } = default!;

    [JsonPropertyName("sortBy")]
    public string SortBy { get; set; } = default!;

    [JsonPropertyName("desc")]
    public bool Desc { get; set; }

    [JsonPropertyName("market")]
    public string Market { get; set; } = default!;
}