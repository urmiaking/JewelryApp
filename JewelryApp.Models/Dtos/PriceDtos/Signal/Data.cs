using System.Text.Json.Serialization;

namespace JewelryApp.Models.Dtos.PriceDtos.Signal;

public class Data
{
    [JsonPropertyName("market")]
    public string Market { get; set; } = default!;

    [JsonPropertyName("data")]
    public List<InnerData>? InnerData { get; set; }

    [JsonPropertyName("totalLength")]
    public int TotalLength { get; set; }
}