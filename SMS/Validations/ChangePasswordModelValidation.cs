using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public class ChangePasswordModelValidator : EntityValidatorBase<ChangePasswordModel>
	{
		public ChangePasswordModelValidator()
		{
			RuleFor(x => x.NewPassword).NotEmpty().Length(8, 50).When(x => x.Id > 0).WithMessage("Invalid Password, Password must have 8 characters long");
			RuleFor(x => x.ConfirmNewPassword).NotEmpty().Equal(x => x.NewPassword).When(x => x.Id > 0).WithMessage("Both Passwords Must Match");
		}
	}
}