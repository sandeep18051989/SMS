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
        public int AssessmentId { get; set; }
        public int QuestionId { get; set; }
        public string Assessment { get; set; }
        public string Question { get; set; }
        public int DisplayOrder { get; set; }
        public string SolveTime { get; set; }
        public double? NegativeMarks { get; set; }
        public double? RightMarks { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsChecked { get; set; }
    }
}
