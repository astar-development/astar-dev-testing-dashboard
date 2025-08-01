using System.Xml.Linq;
using AStar.Dev.Testing.Dashboard.Server.Models;

namespace AStar.Dev.Testing.Dashboard.Server.Controllers;

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
        var          root                = filePath.Substring(index, index2);

        var results = doc.Descendants(ns + "UnitTestResult")
                         .Select(x => new TestResult
                                      {
                                          ProjectName = root,
                                          Name        = x.Attribute("testName")?.Value,
                                          Outcome     = x.Attribute("outcome")?.Value,
                                          Duration    = x.Attribute("duration")?.Value,
                                          StartTime   = x.Attribute("startTime")?.Value,
                                          EndTime     = x.Attribute("endTime")?.Value
                                      })
                         .ToList();

        return results;
    }
}
