using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class PictureModelValidator : EntityValidatorBase<PictureModel>
    {
        public PictureModelValidator()
        {
            RuleFor(x => x.AlternateText).NotEmpty().WithMessage("Please Enter Caption/Alternate Text");
        }


    }
}