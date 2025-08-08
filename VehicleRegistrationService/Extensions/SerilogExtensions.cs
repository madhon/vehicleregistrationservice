namespace VehicleRegistrationService;

using System.Globalization;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

internal static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder, string sectionName = "Serilog")
    {
        var serilogOptions = new SerilogOptions();
        builder.Configuration.GetSection(sectionName).Bind(serilogOptions);

        builder.Services.AddSerilog(loggerConfiguration =>
        {
            loggerConfiguration
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();

            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
            loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            if (serilogOptions.UseConsole)
            {
                loggerConfiguration.WriteTo.Async(writeTo =>
                    writeTo.Console(outputTemplate: serilogOptions.LogTemplate, formatProvider: CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrEmpty(serilogOptions.SeqUrl))
            {
                loggerConfiguration.WriteTo.Seq(serilogOptions.SeqUrl, formatProvider: CultureInfo.InvariantCulture);
            }
        });

        return builder;
    }

    internal sealed class SerilogOptions
    {
        public bool UseConsole { get; set; } = true;
        public string? SeqUrl { get; set; }
        public string? LokiUrl { get; set; }

        public string LogTemplate { get; set; } =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3} - {Message:lj}{NewLine}{Exception}";
    }
}