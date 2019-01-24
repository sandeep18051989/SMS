using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class RoleValidation : AbstractValidator<RoleModel>
    {
        public RoleValidation() {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Please enter role name");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please select acadmic year");
        }
    }
}