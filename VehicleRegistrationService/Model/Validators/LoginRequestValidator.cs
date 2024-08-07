﻿namespace VehicleRegistrationService.Model.Validators;

using FluentValidation;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(p => p.UserName).NotEmpty()
            .WithMessage("Username is required");

        RuleFor(p => p.Password).NotEmpty()
            .WithMessage("Password is required");
    }
}