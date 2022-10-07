namespace VehicleRegistrationService.Model
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            // needed for fast endpoints
        }

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
