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
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
            RuleFor(x => x.DifficultyLevelId).NotEmpty().WithMessage("Please select difficulty level");
            RuleFor(x => x.LogoPictureId).NotEmpty().WithMessage("Please select logo visible on certificates");
            RuleFor(x => x.SignaturePictureId).NotEmpty().WithMessage("Please select logo used for certificate signature");
            RuleFor(x => x.StartTime).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Start date is required");
			RuleFor(x => x.EndTime).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("End date required and should be greater or equal than start date");
			RuleFor(x => x.MaxMarks).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(Double)).GreaterThanOrEqualTo(x => x.PassingMarks).WithMessage("Maximum marks is mandatory and should be greater or equal to passing marks");
			RuleFor(x => x.PassingMarks).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(Double)).LessThanOrEqualTo(x => x.MaxMarks).WithMessage("Passing marks is mandatory and should be less or equal to maximum marks");

		}


	}
}