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
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
		}
	}
}
