using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class DivisionModelValidator : EntityValidatorBase<DivisionModel>
	{
		public DivisionModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter division");
			RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
		}
	}
}
