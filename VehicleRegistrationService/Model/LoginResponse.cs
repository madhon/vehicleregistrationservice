﻿namespace VehicleRegistrationService.Model;

internal sealed class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}