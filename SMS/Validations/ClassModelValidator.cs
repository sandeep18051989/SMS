using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class ClassModelValidator : EntityValidatorBase<ClassModel>
	{
		public ClassModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter class name");
		}
	}
}
