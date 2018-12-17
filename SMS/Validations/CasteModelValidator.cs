using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class CasteModelValidator : EntityValidatorBase<CasteModel>
	{
		public CasteModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.ReligionId).NotEmpty().WithMessage("Please choose religion");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please choose acadmic year");
        }
	}
}