using System.Text.Json.Serialization;

namespace JewelryApp.Business.ExternalModels.Signal;

public class Meta
{
    [JsonPropertyName("shamsiDate")]
    public string ShamsiDate { get; set; } = default!;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = default!;
}