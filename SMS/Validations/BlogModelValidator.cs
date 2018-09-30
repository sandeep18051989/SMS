using SMS.Models;
using EF.Services;
using FluentValidation;

namespace SMS.Validations
{
    public partial class BlogModelValidator : EntityValidatorBase<BlogModel>
    {
        public BlogModelValidator()
        {
            RuleFor(x => x.BlogHtml).NotEmpty().WithMessage("Please enter blog description text or html");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email Address required");


            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Please Enter subject");

        }


    }
}