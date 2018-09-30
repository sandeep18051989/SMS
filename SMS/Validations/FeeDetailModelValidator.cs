using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
	public partial class FeeDetailModelValidator : EntityValidatorBase<FeeDetailModel>
	{
		public FeeDetailModelValidator()
		{
			RuleFor(x => x.CashierId).NotEmpty().WithMessage("Please choose cashier");
			RuleFor(x => x.Date).NotEmpty().WithMessage("Please choose date");
			RuleFor(x => x.FeeCategoryStructureId).NotEmpty().WithMessage("Please choose fee category structure");
			RuleFor(x => x.TotalFees).NotEmpty().WithMessage("Please enter total fees");
			RuleFor(x => x.PaidBy).NotEmpty().WithMessage("Please choose payer name");
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please choose student");
		}
	}
}
