using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class RoleValidation : AbstractValidator<RoleModel>
    {
        public RoleValidation() {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Please enter rolename");
        }
    }
}