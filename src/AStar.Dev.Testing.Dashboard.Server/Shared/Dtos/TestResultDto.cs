namespace AStar.Dev.Testing.Dashboard.Server.Shared.Dtos;

public class TestResultDto
{
    public string Name { get; set; }

    public string StatusIcon => Outcome switch
                                {
                                    "Passed"  => "check_circle",
                                    "Failed"  => "error",
                                    "Skipped" => "remove_circle",
                                    _         => "help"
                                };

    public string   Outcome    { get; set; }
    public double   DurationMs { get; set; }
    public string   Category   { get; set; }
    public string   Message    { get; set; }
    public DateTime Timestamp  { get; set; }
}
