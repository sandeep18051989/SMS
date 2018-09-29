using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class TeacherExamModelValidator : EntityValidatorBase<TeacherExamModel>
	{
		public TeacherExamModelValidator()
		{
			RuleFor(x => x.ClassRoomId).NotEmpty().WithMessage("Please select classroom");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.EndDate).WithMessage("Start date is required and must be less than or equal to end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.StartDate).WithMessage("End date is required and must be greater than or equal to start date");
			RuleFor(x => x.ExamId).NotEmpty().WithMessage("Please choose exam");
			RuleFor(x => x.ResultStatusId).NotEmpty().WithMessage("Please choose result status");
			RuleFor(x => x.TeacherId).NotEmpty().WithMessage("Please choose teacher");
		}
	}
}
