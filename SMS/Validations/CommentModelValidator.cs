using SMS.Models;
using EF.Core.Data;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class CommentModelValidator : EntityValidatorBase<CommentModel>
    {
        public CommentModelValidator()
        {
            RuleFor(x => x.CommentHtml).NotEmpty().WithMessage("Please write comment");
        }

    }
}