using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ProductCategoryModelValidator : EntityValidatorBase<ProductCategoryModel>
	{
		public ProductCategoryModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter system name");
            RuleFor(x => x.PictureId).NotEmpty().WithMessage("Please select picture");
        }


	}
}