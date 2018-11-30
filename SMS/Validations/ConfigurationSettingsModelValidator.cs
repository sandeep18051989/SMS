using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public class ConfigurationSettingsModelValidator : AbstractValidator<ConfigurationSettingsModel>
    {
        public ConfigurationSettingsModelValidator()
        {
            RuleFor(x => x.ItemsPerPage).InclusiveBetween(5, 20).WithMessage("Please enter page size from 5 to 20");
            RuleFor(x => x.PagerLocation).NotEmpty().WithMessage("Please enter pager location");
            RuleFor(x => x.SelectedEmailTemplateForForgotPassword).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.SelectedVisitorQueryTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.CommentOnEventTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.CommentOnProductTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.NewUserRegisterTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.ProductAddedTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.ReplyOnCommentTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.RequestQuoteTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.UserSignInAttemptTemplate).NotEmpty().WithMessage("Please select this template");
            RuleFor(x => x.SelectedCommentOnBlogTemplate).NotEmpty().WithMessage("Please select this template");
        }
    }
}