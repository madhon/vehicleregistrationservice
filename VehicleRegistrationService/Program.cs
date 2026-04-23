var builder = WebApplication.CreateSlimBuilder(args);

builder.AddSerilog();
builder.AddDefaultHealthChecks();
builder.ConfigureOpenTelemetry();
builder.AddFeatureManagementServices();
builder.Services.RegisterServices();

var app = builder.Build();

app.ConfigureApplication();

app.Run();