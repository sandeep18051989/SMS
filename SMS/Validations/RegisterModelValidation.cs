using SMS.Models;
using EF.Core.Data;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class RegisterModelValidator : EntityValidatorBase<RegisterModel>
    {
        public RegisterModelValidator() {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter valid email address");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter username");
            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password must have 6 or more characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Both passwords must match");
        }
    }
}