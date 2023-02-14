using System.Net.Http.Json;
using MAUI4Maui.Models;
using MAUI4Maui.Models.Api;

namespace MAUI4Maui.Services;

public sealed class ApiClient : IDisposable
{
    // TODO: add your key
    private const string ApiKey = "g4Qu5DKklfGxpyH1jbXfY5thANaTTBD9";

    private const string Domain = "https://api.polygon.io/";
    private const string Suffix = " - United States Dollar";

    private HttpClient _httpClient = new ();

    public async Task<List<NameId>> GetTickersAsync(string searchTerm = null)
    {
        var url = $"{Domain}v3/reference/tickers?market=crypto&active=true&order=asc&limit=1000&sort=name&apiKey={ApiKey}";

        if (!string.IsNullOrEmpty(searchTerm))
            url += $"&search={searchTerm}";

        var httpRes = await _httpClient.GetFromJsonAsync<TickersList>(url);

        return httpRes.Results
            .Where(x => x.Name.Contains(Suffix))
            .Select(x => new NameId { Name = x.Name.Replace(Suffix, ""), Id = x.TickerId })
            .ToList();
    }

    public async Task<List<DateValue>> GetAggregatesAsync(string ticker, DateTime start, DateTime end)
    {
        var limit = (end - start).Days + 2;
        var url = $"{Domain}v2/aggs/ticker/{ticker}/range/1/day/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}?adjusted=true&sort=asc&limit={limit}&apiKey={ApiKey}";

        var httpRes = await _httpClient.GetFromJsonAsync<Aggregate>(url);

        return httpRes.Results
            .Select(x => new DateValue { Date = UnixTimeStampToDateTime(x.T), Value = x.C })
            .ToList();
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public void Dispose()
    {
        if (_httpClient != null)
        {
            _httpClient.Dispose();
            _httpClient = null;
        }
    }
}