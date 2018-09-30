using EF.Services;
using FluentValidation;

namespace SMS.Models
{
	public partial class DivisionSubjectModelValidator : EntityValidatorBase<DivisionSubjectModel>
	{
		public DivisionSubjectModelValidator()
		{
			RuleFor(x => x.DivisionId).NotEmpty().WithMessage("Please select division");
			RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Please select subject");
		}
	}
}
