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

    [Validator(typeof(StudentAssessmentModelValidator))]
    public partial class StudentAssessmentModel : BaseEntityModel
    {
        public StudentAssessmentModel()
        {
            Student = new StudentModel();
            AvailableResultStatuses = new List<SelectListItem>();
            AvailableGradeSystem = new List<SelectListItem>();
        }
        public int StudentId { get; set; }
        public int ResultStatusId { get; set; }
        public double MarksObtained { get; set; }
        public int GradeSystemId { get; set; }
        public IList<SelectListItem> AvailableResultStatuses { get; set; }
        public IList<SelectListItem> AvailableGradeSystem { get; set; }
        public StudentModel Student { get; set; }

    }
}
