using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public partial class FeedbackModelValidator : EntityValidatorBase<FeedbackModel>
    {
        public FeedbackModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(x => x.ContactNumber).NotEmpty().WithMessage("Please enter contact number");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter discription");

        }


    }
}