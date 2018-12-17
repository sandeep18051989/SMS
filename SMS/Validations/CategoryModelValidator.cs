using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class CategoryModelValidator : EntityValidatorBase<CategoryModel>
	{
		public CategoryModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
		}


	}
}