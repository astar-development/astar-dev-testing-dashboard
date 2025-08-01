using AStar.Dev.Test.Dashboard.Ui.Models;
using AStar.Dev.Test.Dashboard.Ui.Services;
using Microsoft.AspNetCore.Components;

namespace AStar.Dev.Testing.Dashboard.Components.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject]
    public TestResultsService ResultsService { get; set; } = null!;

    private List<TestResult> Results { get; set; } = [];

    protected override async Task OnInitializedAsync() => Results = await ResultsService.GetAllResultsAsync();
}

