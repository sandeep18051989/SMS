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
			RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select academic year");
            RuleFor(x => x.PassingMarks).NotEmpty().GreaterThanOrEqualTo(x => x.MaxMarks).WithMessage("Passing marks should be less than or equal to maximum marks");
            RuleFor(x => x.MaxMarks).NotEmpty().LessThanOrEqualTo(x => x.PassingMarks).WithMessage("Maximum marks should be greater than or equal to passing marks");

        }
	}
}
