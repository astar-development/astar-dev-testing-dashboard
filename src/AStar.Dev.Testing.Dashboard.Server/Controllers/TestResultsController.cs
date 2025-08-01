using System.Text.Json;
using AStar.Dev.Testing.Dashboard.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Dev.Testing.Dashboard.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestResultsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestResult>>> GetAsync()
    {
        var filePath = Path.Combine("Data", "test-results.json");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Test result file not found.");
        }

        var json    = await System.IO.File.ReadAllTextAsync(filePath);
        var results = JsonSerializer.Deserialize<List<TestResult>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(results);
    }
}
