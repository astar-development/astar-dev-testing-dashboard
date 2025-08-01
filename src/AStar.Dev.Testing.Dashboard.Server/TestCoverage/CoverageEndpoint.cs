using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using AStar.Dev.Testing.Dashboard.Server.Controllers;
using AStar.Dev.Testing.Dashboard.Server.Models;

namespace AStar.Dev.Testing.Dashboard.Server.TestCoverage;

// A record to hold the coverage summary for a single file.
public static class CoverageEndpoint
{
    public static WebApplication MapCodeCoverageEndpoints(this WebApplication app)
    {
// ------------------------------------------------------------------------------------------------
// Shared configuration and utility methods
// ------------------------------------------------------------------------------------------------

// IMPORTANT: Update this to the root directory where your projects are located.
// This is the starting point for the script to find the test results.
        string       path                = Directory.GetCurrentDirectory();
        string rootDirectoryToScan = Path.Combine(path, "../../../");
        Console.WriteLine(rootDirectoryToScan);


// ------------------------------------------------------------------------------------------------
// API Endpoints
// ------------------------------------------------------------------------------------------------

// Endpoint for Code Coverage Results
IResult FindAndProcessFiles(string filePattern, Func<string, JsonNode> processFile)
{
    // Check if the root directory exists
    if (!Directory.Exists(rootDirectoryToScan))
    {
        return Results.NotFound($"The root directory '{rootDirectoryToScan}' does not exist.");
    }

    // Find all test result files recursively.
    // The `testresults.trx` and `coverage.json` files are nested within
    // a GUID-named folder inside the `TestResults` directory.
    var files = Directory.EnumerateFiles(
                                         rootDirectoryToScan,
                                         filePattern,
                                         SearchOption.AllDirectories).ToList();

    if (files.Count == 0)
    {
        return Results.NotFound($"No files matching '{filePattern}' were found in '{rootDirectoryToScan}'.");
    }

    var groupedData = new Dictionary<string, JsonNode>();

    foreach (var file in files)
    {
        try
        {
            // Infer the project name from the file path.
            // A common pattern is that the project directory is two levels up from the file itself.
            var projectDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(file)));

            if (projectDirectory == null)
            {
                continue;
            }

            var projectName = Path.GetFileName(projectDirectory);

            // Process the file and add to the dictionary.
            groupedData[projectName] = processFile(file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file '{file}': {ex.Message}");
        }
    }

    return Results.Ok(groupedData);
}

app.MapGet("/api/coverage", () =>
                            {
                                return FindAndProcessFiles("coverage.json", file =>
                                                                            {
                                                                                var jsonContent = File.ReadAllText(file);
                                                                                var jsonObject = JsonNode.Parse(jsonContent);

                                                                                if (jsonObject?["files"] is not JsonObject filesNode)
                                                                                {
                                                                                    return JsonNode.Parse("{}")!;
                                                                                }

                                                                                var summary = new Dictionary<string, FileCoverageSummary>();

                                                                                CalculateFileCoverageSummary(filesNode, summary);

                                                                                return JsonNode.Parse(JsonSerializer.Serialize(summary))!;
                                                                            });
                            })
   .WithName("GetCoverageResults")
   .WithOpenApi();
        return app;
    }

    private static void CalculateFileCoverageSummary(JsonObject filesNode, Dictionary<string, FileCoverageSummary> summary)
    {
        foreach (var fileProperty in filesNode)
        {
            var filePathKey = fileProperty.Key;
            var fileNode    = fileProperty.Value;

            if (fileNode is not JsonObject fileJsonObject)
            {
                continue;
            }

            var linesNode       = fileJsonObject["lines"] as JsonObject;
            var linesCovered    = linesNode?.AsEnumerable().Count(line => line.Value?.GetValue<int>() > 0)  ?? 0;
            var linesNotCovered = linesNode?.AsEnumerable().Count(line => line.Value?.GetValue<int>() == 0) ?? 0;
            var totalLines      = linesCovered + linesNotCovered;
            var linePercentage  = totalLines > 0 ? (double)linesCovered / totalLines * 100 : 100.0;

            var branchesNode       = fileJsonObject["branches"] as JsonObject;
            var branchesCovered    = 0;
            var branchesNotCovered = 0;

            if (branchesNode != null)
            {
                foreach (var branchLine in branchesNode)
                {
                    if (branchLine.Value is JsonArray branchesArray)
                    {
                        foreach (var branch in branchesArray)
                        {
                            if (branch is JsonArray branchData && branchData.Count >= 2)
                            {
                                var coverageCount = branchData[1]?.GetValue<int>() ?? 0;
                                if (coverageCount > 0)
                                {
                                    branchesCovered++;
                                }
                                else
                                {
                                    branchesNotCovered++;
                                }
                            }
                        }
                    }
                }
            }

            var totalBranches    = branchesCovered + branchesNotCovered;
            var branchPercentage = totalBranches > 0 ? (double)branchesCovered / totalBranches * 100 : 100.0;

            summary[filePathKey] = new FileCoverageSummary(
                                                           linesCovered,
                                                           linesNotCovered,
                                                           branchesCovered,
                                                           branchesNotCovered,
                                                           linePercentage,
                                                           branchPercentage);
        }
    }
}
