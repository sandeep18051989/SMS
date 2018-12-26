using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class DesignationModelValidator : AbstractValidator<DesignationModel>
	{
		public DesignationModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
		}
	}
}
