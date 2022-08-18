namespace VehicleRegistrationService.Model.Validators;

using FluentValidation;

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(p => p.UserName).NotEmpty()
            .WithMessage("Username is required");

        RuleFor(p => p.Password).NotEmpty()
            .WithMessage("Password is required");
    }
}