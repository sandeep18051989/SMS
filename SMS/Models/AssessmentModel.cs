using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    [Validator(typeof(AssessmentModelValidator))]
    public partial class AssessmentModel : BaseEntityModel
    {
        public AssessmentModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableDifficultyLevels = new List<SelectListItem>();
            AvailableSubjects = new List<SelectListItem>();
        }
        public string Name { get; set; }
        [UIHint("DateTimePicker")]
        public DateTime? StartTime { get; set; }
        [UIHint("DateTimePicker")]
        public DateTime? EndTime { get; set; }
        public string StringStartTime { get; set; }
        public string StringEndTime { get; set; }
        public bool OpenToAnonymousUsers { get; set; }
        public double? PassingMarks { get; set; }
        public double? MaxMarks { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int? SubjectId { get; set; }
        public int AcadmicYearId { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string Instructions { get; set; }
        public int DifficultyLevelId { get; set; }
        public string DifficultyLevel { get; set; }
        public bool IsTimeBound { get; set; }
        public int? TotalQuestions { get; set; }
        public double? DurationInMinutes { get; set; }
        public bool MandatoryToSolveAll { get; set; }
        public bool AllowUserToMoveForwardBackward { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string MessageOnSubmitTest { get; set; }
        public bool ShowResultToCandidate { get; set; }
        [UIHint("Picture")]
        public int SignaturePictureId { get; set; }
        [UIHint("Picture")]
        public int LogoPictureId { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<SelectListItem> AvailableDifficultyLevels { get; set; }
        public IList<SelectListItem> AvailableSubjects { get; set; }

    }
}
