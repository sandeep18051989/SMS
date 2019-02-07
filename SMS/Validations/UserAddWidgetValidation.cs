using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class UserAddWidgetValidation : EntityValidatorBase<UserModel>
    {
        public UserAddWidgetValidation() {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter Username");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter email address");
            RuleFor(x => x.Password).NotEmpty().Length(6, 50).WithMessage("Password should be of 6 or more characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Both passwords must match");
        }
    }
}