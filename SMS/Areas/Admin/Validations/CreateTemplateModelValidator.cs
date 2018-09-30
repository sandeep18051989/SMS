using System;
using FluentValidation;
using SMS.Areas.Admin.Models;
using EF.Services;

namespace SMS.Areas.Admin.Validations
{
    public class CreateTemplateModelValidator : EntityValidatorBase<CreateTemplateModel>
    {
        public CreateTemplateModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
        }
    }
}