namespace AStar.Dev.Testing.Dashboard.Server.Models;

public record TestResult(
    string  Name         ,
    string  Outcome      ,
    string  Duration     ,
    string? Category     ,
    string? ErrorMessage ,
    string? StartTime    ,
    string? EndTime      ,
    string  ProjectName
);
