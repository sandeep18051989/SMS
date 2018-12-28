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
			RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter homework description");
		}
	}
}
