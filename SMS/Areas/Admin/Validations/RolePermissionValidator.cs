using System;
using FluentValidation;
using SMS.Areas.Admin.Models;
using EF.Services;
using EF.Services.Service;

namespace SMS.Areas.Admin.Validations
{
    public class RolePermissionValidator : EntityValidatorBase<RolePermissionModel>
    {
        public RolePermissionValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Invalid Role");
        }
    }
}