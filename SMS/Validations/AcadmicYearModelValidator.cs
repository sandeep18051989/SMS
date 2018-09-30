using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class AcadmicYearModelValidator : EntityValidatorBase<AcadmicYearModel>
	{
		public AcadmicYearModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Name - e.g. 2018-19, 2019-20...");
		}
	}
}
