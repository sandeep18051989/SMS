using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class EmployeeAttendanceModelValidator : EntityValidatorBase<EmployeeAttendanceModel>
	{
		public EmployeeAttendanceModelValidator()
		{
			RuleFor(x => x.AttendanceDate).NotEmpty().WithMessage("Please select date");
			RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Please select employee");
		}
	}
}
