using AStar.Dev.Testing.Dashboard.Server.Controllers;
using AStar.Dev.Testing.Dashboard.Server.Models;

namespace AStar.Dev.Testing.Dashboard.Server.TestCoverage;

public static class TestResultsEndpoint
{
    public static WebApplication MapTestResultsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/test-results", () =>
                                        {
                                            var rootPath   = Path.Combine(Directory.GetCurrentDirectory(), "../../test");
                                            var allResults = new List<TestResult>();

                                            var directories = Directory.GetDirectories(rootPath);

                                            foreach (var dir in directories)
                                            {
                                                var filePath = Path.Combine(dir, "TestResults", "results.trx");

                                                if (!File.Exists(filePath))
                                                {
                                                    continue;
                                                }

                                                var results = TrxParser.ParseTrx(filePath);
                                                allResults.AddRange(results);
                                            }

                                            return Results.Ok(allResults);
                                        })
           .WithName("GetTestResults")
           .WithOpenApi();

        return app;
    }
}
