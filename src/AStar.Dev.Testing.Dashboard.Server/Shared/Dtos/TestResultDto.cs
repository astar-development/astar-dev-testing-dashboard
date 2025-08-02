namespace AStar.Dev.Testing.Dashboard.Server.Shared.Dtos;

public record TestResultDto
{
    public required string Name { get; set; }

    public string StatusIcon => Outcome switch
                                {
                                    "Passed"  => "check_circle",
                                    "Failed"  => "error",
                                    "Skipped" => "remove_circle",
                                    _         => "help"
                                };

    public required string   Outcome    { get; set; }
    public          double   DurationMs { get; set; }
    public required string   Category   { get; set; }
    public required string   Message    { get; set; }
    public          DateTime Timestamp  { get; set; }
}
