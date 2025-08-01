namespace AStar.Dev.Testing.Dashboard.Server.TestCoverage;

public record FileCoverageSummary(
    int    LinesCovered,
    int    LinesNotCovered,
    int    BranchesCovered,
    int    BranchesNotCovered,
    double LinePercentage,
    double BranchPercentage);
