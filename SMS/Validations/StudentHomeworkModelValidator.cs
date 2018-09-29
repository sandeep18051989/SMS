using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class StudentHomeworkModelValidator : EntityValidatorBase<StudentHomeworkModel>
	{
		public StudentHomeworkModelValidator()
		{
			RuleFor(x => x.HomeworkId).NotEmpty().WithMessage("Please choose homework");
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please choose student");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.EndDate).WithMessage("Start date is required and must be less than or equal to end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.StartDate).WithMessage("End date is required and must be greater than or equal to start date");
		}
	}
}
