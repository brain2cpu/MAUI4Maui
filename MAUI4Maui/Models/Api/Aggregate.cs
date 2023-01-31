namespace MAUI4Maui.Models.Api;

public class Aggregate
{
    public bool Adjusted { get; set; }
    public int QueryCount { get; set; }
    public string RequestId { get; set; }
    public AggregateResult[] Results { get; set; }
    public int ResultsCount { get; set; }
    public string Status { get; set; }
    public string Ticker { get; set; }
}