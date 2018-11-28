using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class PermissionRecordValidation : AbstractValidator<PermissionRecordModel>
    {
        public PermissionRecordValidation() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Please enter category");
        }
    }
}