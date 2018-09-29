using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class SubjectExamModelValidator : EntityValidatorBase<SubjectExamModel>
	{
		public SubjectExamModelValidator()
		{
			RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Please select subject");
			RuleFor(x => x.ExamId).NotEmpty().WithMessage("Please select exam");
			RuleFor(x => x.ResultStatusId).NotEmpty().WithMessage("Please choose result status");
		}
	}
}
