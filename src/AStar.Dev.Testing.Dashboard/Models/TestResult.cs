namespace AStar.Dev.Testing.Dashboard.Models;

public record TestResult(
    string   Name         ,
    string   Outcome      ,
    TimeSpan Duration     ,
    string   Category     ,
    string   ErrorMessage ,
    DateTime Timestamp    ,
    string   StartTime    ,
    string   EndTime     );
