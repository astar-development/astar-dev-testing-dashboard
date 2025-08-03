using System.Text.Json;
using AStar.Dev.Testing.Dashboard.Models;

namespace AStar.Dev.Testing.Dashboard.Services;

public class TestResultsService(IHttpClientFactory factory)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()  { PropertyNameCaseInsensitive = true };
    private readonly        HttpClient            httpClient            = factory.CreateClient("TestApi");

    public async Task<List<TestResult>> GetAllResultsAsync()
    {
        var response = await httpClient.GetAsync("/api/test-results");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var results = JsonSerializer.Deserialize<List<TestResult>>(json, JsonSerializerOptions)
                      ?? [];

        return results;
    }
}
