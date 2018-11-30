using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class SliderValidation : AbstractValidator<SliderModel>
    {
        public SliderValidation() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
        }
    }
}