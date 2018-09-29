using SMS.Models;
using EF.Core.Data;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class PostReplyModelValidator : EntityValidatorBase<PostReplyModel>
    {
        public PostReplyModelValidator()
        {
            RuleFor(x => x.ReplyHtml).NotEmpty().WithMessage("Please write your reply");
        }


    }
}