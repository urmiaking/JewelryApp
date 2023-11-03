using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Common.Currency.Header;

public class Data
{
    [JsonProperty("market")]
    public string Market { get; set; }

    [JsonProperty("data")]
    public List<JewelryApp.Models.Dtos.Common.Currency.Body.Data> data { get; set; }

    [JsonProperty("totalLength")]
    public int TotalLength { get; set; }
}
