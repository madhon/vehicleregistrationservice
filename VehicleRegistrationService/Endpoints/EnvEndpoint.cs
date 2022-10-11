namespace VehicleRegistrationService.Endpoints
{
    using System.Text.Json;

    public class EnvEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("/env");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IWebHostEnvironment? hostEnvironment = Resolve<IWebHostEnvironment>();

            var thisEnv = new
            {
                ApplicationName = hostEnvironment.ApplicationName,
                Environment = hostEnvironment.EnvironmentName,
            };

            var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

            await SendAsync(thisEnv,200, ct).ConfigureAwait(false);
        }
    }
}
