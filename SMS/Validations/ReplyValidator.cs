using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class ReplyModelValidator : EntityValidatorBase<RepliesModel>
    {
        public ReplyModelValidator()
        {
            RuleFor(x => x.ReplyHtml).NotEmpty().WithMessage("Please write your reply");
        }


    }
}