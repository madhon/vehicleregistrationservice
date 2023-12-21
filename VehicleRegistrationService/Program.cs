var builder = WebApplication.CreateSlimBuilder(args);

builder.AddSerilog();

builder.RegisterServices();

var app = builder.Build();

app.ConfigureApplication();

app.Run();