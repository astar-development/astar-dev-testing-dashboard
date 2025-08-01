using System.Text.Json;
using AStar.Dev.Test.Dashboard.Ui.Models;

namespace AStar.Dev.Test.Dashboard.Ui.Services;

public class TestResultsService(IHttpClientFactory factory)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()  { PropertyNameCaseInsensitive = true };
    private readonly        HttpClient            httpClient            = factory.CreateClient("TestApi");

    public async Task<List<TestResult>> GetAllResultsAsync()
    {
        var response = await httpClient.GetAsync("/api/multiprojectresults");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var results = JsonSerializer.Deserialize<List<TestResult>>(json, JsonSerializerOptions)
                      ?? [];

        return results;
    }
}
