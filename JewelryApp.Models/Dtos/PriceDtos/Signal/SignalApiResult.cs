using System.Text.Json.Serialization;

namespace JewelryApp.Models.Dtos.PriceDtos.Signal;

public class SignalApiResult
{
    [JsonPropertyName("data")]
    public Data Data { get; set; } = default!;

    [JsonPropertyName("meta")]
    public Meta Meta { get; set; } = default!;
}