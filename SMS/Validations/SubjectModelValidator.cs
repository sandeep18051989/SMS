using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class SubjectModelValidator : EntityValidatorBase<SubjectModel>
	{
		public SubjectModelValidator()
		{
			RuleFor(x => x.Marks).NotEmpty().LessThan(x => x.PassMarks).WithMessage("Please enter total marks must be less than passing marks");
			RuleFor(x => x.PassMarks).NotEmpty().GreaterThan(x => x.Marks).WithMessage("Please enter passing marks less than total marks for this subject");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
