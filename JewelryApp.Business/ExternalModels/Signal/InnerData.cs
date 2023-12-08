using System.Text.Json.Serialization;

namespace JewelryApp.Business.ExternalModels.Signal;

public class InnerData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("close")]
    public int Close { get; set; }
}