using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class TimeTableSettingModelValidator : EntityValidatorBase<TimeTableSettingModel>
	{
		public TimeTableSettingModelValidator()
		{
			RuleFor(x => x.LectureTime).NotEmpty().WithMessage("Please enter lecture time");
			RuleFor(x => x.SchoolStartTime).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.SchoolEndTime).WithMessage("School start time is required");
			RuleFor(x => x.SchoolEndTime).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.SchoolStartTime).WithMessage("School end time is required and should be greater or equal to school start time");
			RuleFor(x => x.RecessTimeMin).NotEmpty().WithMessage("Please enter recess time");
		}


	}
}
