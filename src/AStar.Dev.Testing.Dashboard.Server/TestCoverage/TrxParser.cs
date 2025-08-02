using System.Xml.Linq;
using AStar.Dev.Testing.Dashboard.Server.Models;

namespace AStar.Dev.Testing.Dashboard.Server.TestCoverage;

public static class TrxParser
{
    public static List<TestResult> ParseTrx(string filePath)
    {
        var        doc = XDocument.Load(filePath);
        XNamespace ns  = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

        const string initialSearchString = "../../test/";
        const string secondSearchString  = ".Tests.Unit/TestResults/results.trx";
        var          index               = filePath.LastIndexOf(initialSearchString, StringComparison.Ordinal) + initialSearchString.Length;
        var          index2              = filePath.LastIndexOf(secondSearchString,  StringComparison.Ordinal) - index;
        var          projectName         = filePath.Substring(index, index2);

        var results = doc.Descendants(ns + "UnitTestResult")
                         .Select(x => new TestResult
                                     (
                                      x.Attribute("testName")?.Value!,
                                      x.Attribute("outcome")?.Value!,
                                      x.Attribute("duration")?.Value!,
                                      x.Attribute("startTime")?.Value,
                                      x.Attribute("endTime")?.Value,
                                      x.Attribute("errorMessage")?.Value,
                                      x.Attribute("testType")?.Value,
                                      projectName
                                     ))
                         .ToList();

        return results;
    }
}
