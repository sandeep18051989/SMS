using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class UserValidation : EntityValidatorBase<UserModel>
    {
        public UserValidation() {

            RuleFor(x => x.Username).NotEmpty().WithMessage("Invalid Username");
            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password Must Have 6 Characters");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter username");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter email address");
            RuleFor(x => x.Password).NotEmpty().Length(6, 50).WithMessage("Password should be of 6 or more characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Both passwords must match");
            //RuleFor(x => x.SelectedRoleIds).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("A user must need a role on creation to get started!").Must(NotEqualZero).WithMessage("Please select atleast one role");

        }
    }
}