using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class TeacherModelValidator : EntityValidatorBase<TeacherModel>
	{
		public TeacherModelValidator()
		{
			RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please select employee");
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.QualificationId).NotEmpty().WithMessage("Please select qualification");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter short name without spaces to use for URLs");
			RuleFor(x => x.ProfilePictureId).NotEmpty().WithMessage("Please upload latest picture");
			RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select academic year");
		}
	}
}
