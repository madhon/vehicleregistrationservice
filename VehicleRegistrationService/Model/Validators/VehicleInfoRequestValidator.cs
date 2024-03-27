namespace VehicleRegistrationService.Model.Validators
{
    using FluentValidation;

    internal class VehicleInfoRequestValidator : AbstractValidator<VehicleInfoRequest>
    {
        public VehicleInfoRequestValidator()
        {
            RuleFor(p => p.LicenseNumber).NotEmpty()
                .WithMessage("License Number is required");
        }
    }
}
