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
            RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required");
            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate).WithMessage("End date required and should be greater than start date").When(x => x.StartDate != null);
            RuleFor(x => x.Venue).NotEmpty().WithMessage("Please enter venue");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter description");
        }


    }
}