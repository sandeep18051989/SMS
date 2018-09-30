using System;
using FluentValidation;
using SMS.Areas.Admin.Models;
using EF.Services;
using EF.Services.Service;

namespace SMS.Areas.Admin.Validations
{
    public class PermissionModelValidator : EntityValidatorBase<CreatePermissionModel>
    {
        public PermissionModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Invalid Name");
        }
    }
}