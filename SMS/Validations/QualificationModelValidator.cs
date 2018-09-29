using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class QualificationModelValidator : EntityValidatorBase<QualificationModel>
	{
		public QualificationModelValidator()
		{
			RuleFor(x => x.QualificationName).NotEmpty().WithMessage("Please enter qualification");
		}
	}
}
