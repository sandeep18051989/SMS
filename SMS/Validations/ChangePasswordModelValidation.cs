using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public class ChangePasswordModelValidator : EntityValidatorBase<ChangePasswordModel>
	{
		public ChangePasswordModelValidator()
		{
			RuleFor(x => x.Password).NotEmpty().Length(8, 50).WithMessage("Invalid Password, Password must have 8 characters long");
			RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Both Passwords Must Match");
		}
	}
}