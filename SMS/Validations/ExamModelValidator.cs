using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class ExamModelValidator : EntityValidatorBase<ExamModel>
	{
		public ExamModelValidator()
		{
			RuleFor(x => x.ExamName).NotEmpty().WithMessage("Please enter exam name");
			RuleFor(x => x.OutOf).NotEmpty().WithMessage("Please enter total marks");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
