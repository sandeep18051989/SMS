using SMS.Models;
using EF.Core.Data;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class RegisterModelValidator : EntityValidatorBase<RegisterModel>
    {
        public RegisterModelValidator() {

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please Enter Name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password Must Have 6 Characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Both Passwords Must Match");
        }
    }
}