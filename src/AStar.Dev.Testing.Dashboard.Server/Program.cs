using AStar.Dev.Testing.Dashboard.Server.TestCoverage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
                         {
                             options.AddDefaultPolicy(policy =>
                                                      {
                                                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                                      });
                         });

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapOpenApi();

app.MapControllers();
app.UseHttpsRedirection();

app.MapCodeCoverageEndpoints()
   .MapTestResultsEndpoint()
   .UseCors();

await app.RunAsync();
