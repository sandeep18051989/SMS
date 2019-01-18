using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class AssessmentQuestionModelValidator : EntityValidatorBase<AssessmentQuestionModel>
	{
		public AssessmentQuestionModelValidator()
		{
			RuleFor(x => x.AssessmentId).NotEmpty().WithMessage("Please select assessment");
			RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Please select question");
		}


	}
}