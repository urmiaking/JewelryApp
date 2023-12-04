using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Price.Signal;

public class InnerData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("close")]
    public int Close { get; set; }
}