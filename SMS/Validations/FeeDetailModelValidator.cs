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
			RuleFor(x => x.CashierId).NotEmpty().WithMessage("Please select cashier");
			RuleFor(x => x.FeeCategoryStructureId).NotEmpty().WithMessage("Please select fee category structure");
			RuleFor(x => x.TotalFees).NotEmpty().WithMessage("Please enter total fees");
            RuleFor(x => x.FeesPaid).NotEmpty().WithMessage("Please enter fees paid");
            RuleFor(x => x.PaidBy).NotEmpty().WithMessage("Please enter payer");
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please select student");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Please select date");
            RuleFor(x => x.DDChequeNumber).NotEmpty().When(x => x.BankName == null || x.BankName == "").WithMessage("Please enter bank name");
        }
	}
}
