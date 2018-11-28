using System.Collections.Generic;
using System.Linq;
using EF.Services;
using FluentValidation;
using SMS.Areas.Admin.Models;
using SMS.Models;

namespace SMS.Areas.Admin.Validations
{
	public class AdminUserModelValidation : AbstractValidator<UserModel>
	{
		public AdminUserModelValidation()
		{
			RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter username");
			RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter email address");
		    RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter valid email address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter password");
		    RuleFor(x => x.Password).Length(6, 50).WithMessage("Password must be of 6 or more characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please re-type password");
		    RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Both passwords must match");
            RuleFor(x => x.SelectedRoleIds).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("A user must need a role on creation to get started!").Must(NotEqualZero).WithMessage("Please select atleast one role");
		}

		private bool NotEqualZero(int[] ints)
		{
			return ints.All(i => i != 0);
		}

	}
}