namespace VehicleRegistrationService.Model
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            // needed for fast endpoints
        }

        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
