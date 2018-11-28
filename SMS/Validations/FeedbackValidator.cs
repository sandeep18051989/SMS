using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public partial class FeedbackModelValidator : AbstractValidator<FeedbackModel>
    {
        public FeedbackModelValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(x => x.Contact).NotEmpty().WithMessage("Please enter contact number");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter discription");

        }


    }
}