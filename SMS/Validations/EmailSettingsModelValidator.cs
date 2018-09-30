using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public class EmailSettingsModelValidator : EntityValidatorBase<EmailSettingsModel>
    {
        public EmailSettingsModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter username");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password");
            RuleFor(x => x.Port).NotEmpty().WithMessage("Port is required");
            RuleFor(x => x.Host).NotEmpty().WithMessage("Host is required");
            RuleFor(x => x.FromEmail).NotEmpty().EmailAddress().WithMessage("Invalid Email Address");
        }
    }
}