using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class SchoolModelValidator : EntityValidatorBase<SchoolModel>
	{
		public SchoolModelValidator()
		{
			RuleFor(x => x.UserId).NotEmpty().WithMessage("Please choose user");
			RuleFor(x => x.FullName).NotEmpty().WithMessage("Please enter full name");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter school short name without spaces for URL");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}