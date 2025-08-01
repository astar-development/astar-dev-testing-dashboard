namespace AStar.Dev.Testing.Dashboard.Server.Models;

public class TestResult
{
    public string   Name         { get;  set; }
    public string   Outcome      { get;  set; } // Passed, Failed, Skipped
    public string   Duration     { get;  set; }
    public string   Category     { get;  set; }
    public string   ErrorMessage { get;  set; }
    public DateTime Timestamp    { get;  set; }
    public string?  StartTime    { get ; set ; }
    public string?  EndTime      { get ; set ; }
}
