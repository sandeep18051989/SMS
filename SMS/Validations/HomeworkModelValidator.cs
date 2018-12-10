using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class HomeworkModelValidator : EntityValidatorBase<HomeworkModel>
	{
		public HomeworkModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.StudentApprovalStatusId).NotEmpty().WithMessage("Please mark student approval status");
			RuleFor(x => x.TeacherApprovalStatusId).NotEmpty().WithMessage("Please mark teacher approval status");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter homework in detail");
		}
	}
}
