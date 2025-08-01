using AStar.Dev.Testing.Dashboard.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Dev.Testing.Dashboard.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MultiProjectResultsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TestResult>> GetTrxResults()
    {
        var rootPath   = Path.Combine(Directory.GetCurrentDirectory(), "../../test");
        var allResults = new List<TestResult>();

        var directories = Directory.GetDirectories(rootPath);

        foreach (var dir in directories)
        {
            var filePath = Path.Combine(dir, "TestResults", "results.trx");

            if (!System.IO.File.Exists(filePath))
            {
                continue;
            }

            var results = TrxParser.ParseTrx(filePath);
            allResults.AddRange(results);
        }

        return Ok(allResults);
    }
}
