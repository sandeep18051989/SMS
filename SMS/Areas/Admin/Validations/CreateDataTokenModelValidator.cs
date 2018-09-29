using System;
using FluentValidation;
using SMS.Areas.Admin.Models;
using EF.Services;
using static SMS.Areas.Admin.Models.CreateTemplateModel;

namespace SMS.Areas.Admin.Validations
{
	public class CreateDataTokenModelValidator : EntityValidatorBase<CreateDataTokensModel>
	{
		public CreateDataTokenModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter system name");
			RuleFor(x => x.Value).NotEmpty().WithMessage("Please enter value");
		}
	}
}