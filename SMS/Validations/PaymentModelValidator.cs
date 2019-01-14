using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class PaymentModelValidator : EntityValidatorBase<PaymentModel>
	{
		public PaymentModelValidator()
		{
			RuleFor(x => x.BasicPay).NotEmpty().WithMessage("Please enter basic pay");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please select employee");
            RuleFor(x => x.Gross_Pay).NotEmpty().WithMessage("Gross Pay not calculated correctly");
            RuleFor(x => x.Net_Pay).NotEmpty().WithMessage("Net Pay not calculated correctly");
        }
	}
}
