using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
    [Validator(typeof(AssessmentQuestionModelValidator))]
    public partial class AssessmentQuestionModel : BaseEntityModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PassingMarks { get; set; }
        public double MaxMarks { get; set; }

    }
}
