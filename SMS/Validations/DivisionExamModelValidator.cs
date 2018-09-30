using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class DivisionExamModelValidator : EntityValidatorBase<DivisionExamModel>
	{
		public DivisionExamModelValidator()
		{
			RuleFor(x => x.ClassRoomId).NotEmpty().WithMessage("Please select classroom");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.EndDate).WithMessage("Start date is required and must be less than or equal to end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.StartDate).WithMessage("End date is required and must be greater than or equal to start date");
		}
	}
}
