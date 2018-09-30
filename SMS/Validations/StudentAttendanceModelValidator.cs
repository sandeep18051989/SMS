using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class StudentAttendanceModelValidator : EntityValidatorBase<StudentAttendanceModel>
	{
		public StudentAttendanceModelValidator()
		{
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please select student");
			RuleFor(x => x.AttendanceStatusId).NotEmpty().WithMessage("Please choose attendance status");
			RuleFor(x => x.Date).NotEmpty().WithMessage("Please choose date");
		}
	}
}
