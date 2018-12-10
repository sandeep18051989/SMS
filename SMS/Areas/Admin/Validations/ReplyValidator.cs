using SMS.Areas.Admin.Models;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Areas.Admin.Validations
{
    public class ReplyModelValidator : EntityValidatorBase<ReplyModel>
    {
        public ReplyModelValidator()
        {
            RuleFor(x => x.ReplyHtml).NotEmpty().WithMessage("Please write your reply");
        }


    }
}