using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class PurchaseModelValidator : EntityValidatorBase<PurchaseModel>
	{
		public PurchaseModelValidator()
		{
			RuleFor(x => x.IName).NotEmpty().WithMessage("Please enter item name");
			RuleFor(x => x.IPurchaseDate).NotEmpty().WithMessage("Please enter item purchase date");
			RuleFor(x => x.IQuantity).NotEmpty().WithMessage("Please enter item quantity");
			RuleFor(x => x.IRate).NotEmpty().WithMessage("Please enter item rate");
			RuleFor(x => x.ITax).NotEmpty().WithMessage("Please enter item tax");
			RuleFor(x => x.VendorId).NotEmpty().WithMessage("Please choose item vendor");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Please choose item");
        }
	}
}
