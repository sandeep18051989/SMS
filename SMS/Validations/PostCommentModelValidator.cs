using SMS.Models;
using EF.Core.Data;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class PostCommentModelValidator : EntityValidatorBase<PostCommentsModel>
    {
        public PostCommentModelValidator()
        {
            RuleFor(x => x.CommentHtml).NotEmpty().WithMessage("Please write comment");
        }


    }
}