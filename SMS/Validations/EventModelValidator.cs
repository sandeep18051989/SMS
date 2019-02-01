using System;
using FluentValidation;
using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
    public partial class EventModelValidator : EntityValidatorBase<EventModel>
    {
        public EventModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter name");
            RuleFor(x => x.StartDate).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required and should be less than or equal to end date");
            RuleFor(x => x.EndDate).NotEmpty().Must(date => date != default(DateTime)).WithMessage("End date required and should be greater than start date").When(x => x.StartDate != null);
            RuleFor(x => x.Venue).NotEmpty().WithMessage("Please enter venue");
            RuleFor(x => x.HeadLine).NotEmpty().WithMessage("Please enter headline");
        }


    }
}