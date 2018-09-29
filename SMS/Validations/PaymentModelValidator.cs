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
			RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please choose employee");
			RuleFor(x => x.AllowanceId).NotEmpty().WithMessage("Please choose allowance");
			RuleFor(x => x.PDate).NotEmpty().WithMessage("Please choose date");
			RuleFor(x => x.BasicPay).NotEmpty().WithMessage("Please enter basic pay");
			RuleFor(x => x.DA).NotEmpty().WithMessage("Please enter DA");
			RuleFor(x => x.HRA).NotEmpty().WithMessage("Please enter HRA");
			RuleFor(x => x.PF).NotEmpty().WithMessage("Please enter PF");
			RuleFor(x => x.TA).NotEmpty().WithMessage("Please enter TA");
			RuleFor(x => x.TDS).NotEmpty().WithMessage("Please enter TDS");
		}
	}
}
