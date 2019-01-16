using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class AssessmentStudentModelValidator : AbstractValidator<AssessmentStudentModel>
	{
		public AssessmentStudentModelValidator()
		{
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please select student");
            RuleFor(x => x.ResultStatusId).NotEmpty().WithMessage("Please select result status");
        }
	}
}