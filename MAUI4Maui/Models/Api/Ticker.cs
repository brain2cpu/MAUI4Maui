using System.Text.Json.Serialization;

namespace MAUI4Maui.Models.Api;

public class Ticker
{
    public bool Active { get; set; }
    public string Cik { get; set; }
    public string CompositeFigi { get; set; }
    public string CurrencyName { get; set; }
    public DateTime LastUpdatedUtc { get; set; }
    public string Locale { get; set; }
    public string Market { get; set; }
    public string Name { get; set; }
    public string PrimaryExchange { get; set; }
    public string ShareClassFigi { get; set; }
    
    [JsonPropertyName("ticker")]
    public string TickerId { get; set; }
    
    public string Type { get; set; }
}