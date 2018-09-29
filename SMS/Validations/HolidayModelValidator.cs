using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class HolidayModelValidator : EntityValidatorBase<HolidayModel>
	{
		public HolidayModelValidator()
		{
			RuleFor(x => x.Date).NotEmpty().WithMessage("Please select date");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
