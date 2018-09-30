using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ClassExamModelValidator : EntityValidatorBase<ClassExamModel>
	{
		public ClassExamModelValidator()
		{
			RuleFor(x => x.BreakTime).NotEmpty().WithMessage("Please enter break time");
			RuleFor(x => x.ClassId).NotEmpty().WithMessage("Please choose class");
			RuleFor(x => x.ClassRoomId).NotEmpty().WithMessage("Please choose exam room");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date is required and should be less than end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");
			RuleFor(x => x.ExamId).NotEmpty().WithMessage("Please choose exam");
			RuleFor(x => x.ResultStatusId).NotEmpty().WithMessage("Please choose result status");
		}
	}
}
