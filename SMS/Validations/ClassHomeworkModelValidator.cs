using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ClassHomeworkModelValidator : EntityValidatorBase<ClassHomeworkModel>
	{
		public ClassHomeworkModelValidator()
		{
			RuleFor(x => x.ClassId).NotEmpty().WithMessage("Please enter class");
			RuleFor(x => x.HomeworkId).NotEmpty().WithMessage("Please choose homework");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date is required and should be less than end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");
		}
	}
}
