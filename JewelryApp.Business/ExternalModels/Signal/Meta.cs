using System.Text.Json.Serialization;

namespace JewelryApp.Application.ExternalModels.Signal;

public class Meta
{
    [JsonPropertyName("shamsiDate")]
    public string ShamsiDate { get; set; } = default!;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = default!;
}