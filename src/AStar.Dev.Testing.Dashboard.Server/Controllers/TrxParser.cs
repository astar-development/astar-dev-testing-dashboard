using System.Xml.Linq;
using AStar.Dev.Testing.Dashboard.Server.Models;

namespace AStar.Dev.Testing.Dashboard.Server.Controllers;

public static class TrxParser
{
    public static List<TestResult> ParseTrx(string filePath)
    {
        var        doc = XDocument.Load(filePath);
        XNamespace ns  = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

        var results = doc.Descendants(ns + "UnitTestResult")
                         .Select(x => new TestResult
                                      {
                                          Name      = x.Attribute("testName")?.Value,
                                          Outcome   = x.Attribute("outcome")?.Value,
                                          Duration  = x.Attribute("duration")?.Value,
                                          StartTime = x.Attribute("startTime")?.Value,
                                          EndTime   = x.Attribute("endTime")?.Value
                                      })
                         .ToList();

        return results;
    }
}
