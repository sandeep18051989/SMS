using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class TimeTableModelValidator : EntityValidatorBase<TimeTableModel>
	{
		public TimeTableModelValidator()
		{
			RuleFor(x => x.DivisionId).NotEmpty().WithMessage("Please choose division");
			RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Please choose class");
			RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Please choose teacher");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date is required and should be less than end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");
		}
	}
}
