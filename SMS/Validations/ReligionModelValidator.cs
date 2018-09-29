using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ReligionModelValidator : EntityValidatorBase<ReligionModel>
	{
		public ReligionModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
		}
	}
}
