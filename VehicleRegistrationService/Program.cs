using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(c =>
{
    c.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
    c.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
    c.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
    c.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVehicleInfoRepository, InMemoryVehicleInfoRepository>();

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3602";
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60002";

builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddDapr();

builder.Services.AddProblemDetailsConventions();

var app = builder.Build();

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();