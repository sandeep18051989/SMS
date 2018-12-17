using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ClassRoomModelValidator : EntityValidatorBase<ClassRoomModel>
	{
		public ClassRoomModelValidator()
		{
			RuleFor(x => x.Number).NotEmpty().WithMessage("Please enter room number");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please choose acadmic year");
        }
	}
}
