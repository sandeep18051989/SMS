using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class FeeCategoryModelValidator : EntityValidatorBase<FeeCategoryModel>
	{
		public FeeCategoryModelValidator()
		{
			RuleFor(x => x.FeeAmount).NotEmpty().WithMessage("Please enter fee amount");
			RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Please choose category");
			RuleFor(x => x.ClassDivisionId).NotEmpty().WithMessage("Please choose class");
			RuleFor(x => x.PeriodFrom).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.PeriodTo).WithMessage("Period from is required and must be less than or equal to period to date");
			RuleFor(x => x.PeriodTo).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.PeriodFrom).WithMessage("Period to is required and must be greater than or equal to period from date");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
