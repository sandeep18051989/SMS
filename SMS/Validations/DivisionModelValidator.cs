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
			RuleFor(x => x.DivisionName).NotEmpty().WithMessage("Please enter division");
			RuleFor(x => x.ClassId).NotEmpty().WithMessage("Please choose class");
		}
	}
}
