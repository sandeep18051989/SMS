using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class AssessmentModelValidator : EntityValidatorBase<AssessmentModel>
	{
		public AssessmentModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");


			RuleFor(x => x.MaxMarks).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(Double)).LessThanOrEqualTo(x => x.PassingMarks).WithMessage("Maximum marks is mandatory and should be greater than passing marks");
			RuleFor(x => x.PassingMarks).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(Double)).GreaterThanOrEqualTo(x => x.MaxMarks).WithMessage("Passing marks is mandatory and should be less than maximum marks");

		}


	}
}