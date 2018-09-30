using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
	public partial class InstallDatabaseValidation : EntityValidatorBase<InstallDatabaseModel>
	{
		public InstallDatabaseValidation()
		{
			RuleFor(x => x.Datasource).NotEmpty().WithMessage("Invalid Datasource");
			RuleFor(x => x.Database).NotEmpty().WithMessage("Invalid Database");
			RuleFor(x => x.Username).NotEmpty().WithMessage("Invalid Username");
			RuleFor(x => x.Password).NotEmpty().WithMessage("Please Enter Database Password");
			RuleFor(x => x.AdminUsername).NotEmpty().WithMessage("Please Enter Admin Username");
			RuleFor(x => x.School.AcadmicYear).NotEmpty().WithMessage("Please Enter Acadmic Year");
			RuleFor(x => x.School.FullName).NotEmpty().WithMessage("Please Enter School Full Name");
			//RuleFor(x => x.School.SystemName).NotEmpty().WithMessage("Please Enter School System Name Without Spaces");
			RuleFor(x => x.AdminPassword).NotEmpty().Length(8, 50).WithMessage("Invalid Password, Admin Password must have 8 characters long");

		}
	}
}