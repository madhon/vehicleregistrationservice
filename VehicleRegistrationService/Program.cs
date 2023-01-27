var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

builder.RegisterServices();

var app = builder.Build();

app.ConfigureApplication();

app.Run();