namespace VehicleRegistrationService.Endpoints
{
    using System.Text.Json;

    public class ConfEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Version(1);
            Get("/conf");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IConfiguration? allConfig = Resolve<IConfiguration>();
            IEnumerable<KeyValuePair<string, string>> configKv = allConfig.AsEnumerable();

            var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };


            await SendAsync(configKv, 200, ct).ConfigureAwait(false);
        }
    }
}
