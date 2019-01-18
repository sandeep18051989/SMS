using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class QuestionModelValidator : EntityValidatorBase<QuestionModel>
	{
		public QuestionModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.RightMarks).NotEmpty().WithMessage("Please enter marks");
            RuleFor(x => x.DifficultyLevelId).NotEmpty().WithMessage("Please select difficulty level");
            RuleFor(x => x.QuestionTypeId).NotEmpty().WithMessage("Please select question type");
		}


	}
}