using AStar.Dev.Test.Dashboard.Ui.Services;
using AStar.Dev.Testing.Dashboard;
using AStar.Dev.Testing.Dashboard.Components;
using AStar.Dev.Testing.Dashboard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<TestResultsService>();

builder.Services.AddHttpClient("TestApi", client =>
                                          {
                                              client.BaseAddress = new ("https://localhost:7253"); // Get from configuration
                                          });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
