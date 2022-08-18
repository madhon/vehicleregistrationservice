namespace VehicleRegistrationService.Model.Validators
{
    using FluentValidation;

    public class VehicleInfoRequestValidator : Validator<VehicleInfoRequest>
    {
        public VehicleInfoRequestValidator()
        {
            RuleFor(p => p.LicenseNumber).NotEmpty()
                .WithMessage("License Number is required");
        }
    }
}
