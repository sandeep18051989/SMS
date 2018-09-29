using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class DesignationModelValidator : EntityValidatorBase<DesignationModel>
	{
		public DesignationModelValidator()
		{
			RuleFor(x => x.DesignationName).NotEmpty().WithMessage("Please enter designation");
		}
	}
}
