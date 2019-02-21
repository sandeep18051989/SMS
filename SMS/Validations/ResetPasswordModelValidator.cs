using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class ResetPasswordModelValidator : EntityValidatorBase<ResetPasswordModel>
    {
        public ResetPasswordModelValidator() {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Please enter current password");
            RuleFor(x => x.NewPassword).NotEmpty().Length(6, 50).Equal(m => m.OldPassword).WithMessage("Please enter new password with atleast 6 characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotEqual(m => m.NewPassword).WithMessage("Both password and confirm passwords must match");
        }
    }
}