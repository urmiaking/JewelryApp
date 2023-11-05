using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Common.Currency.Body;

public class Data
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("close")]
    public int Close { get; set; }
}
