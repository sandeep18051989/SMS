using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public class ProductValidator : EntityValidatorBase<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter title");
            RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter system name without spaces.");
        }
    }
}