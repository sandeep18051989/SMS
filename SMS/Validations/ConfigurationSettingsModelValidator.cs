using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public class ConfigurationSettingsModelValidator : EntityValidatorBase<ConfigurationSettingsModel>
    {
        public ConfigurationSettingsModelValidator()
        {
            RuleFor(x => x.ItemsPerPage).InclusiveBetween(5, 20).WithMessage("Please enter page size from 5 to 20");
            RuleFor(x => x.PagerLocation).NotEmpty().WithMessage("Please enter pager location");
        }
    }
}