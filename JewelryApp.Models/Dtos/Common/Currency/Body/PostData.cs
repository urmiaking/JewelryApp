using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Common.Currency.Body;

public class PostData
{
    [JsonProperty("property")]
    public string[] Property { get; set; }

    [JsonProperty("sortBy")]
    public string SortBy { get; set; }

    [JsonProperty("desc")]
    public bool Desc { get; set; }

    [JsonProperty("market")]
    public string Market { get; set; }
}
