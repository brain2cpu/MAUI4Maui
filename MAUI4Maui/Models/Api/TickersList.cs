namespace MAUI4Maui.Models.Api;

public class TickersList
{
    public int Count { get; set; }
    public string NextUrl { get; set; }
    public string RequestId { get; set; }
    public Ticker[] Results { get; set; }
    public string Status { get; set; }
}