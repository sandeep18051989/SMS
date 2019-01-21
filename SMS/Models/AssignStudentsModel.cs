using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
    public partial class AssignStudentsModel
    {
        public AssignStudentsModel()
        {
            List = new List<AssessmentStudentModel>();
            AvailableClassDivisions = new List<SelectListItem>();
        }
        public int AssessmentId { get; set; }
        public string Assessment { get; set; }
        public string StringStartTime { get; set; }
        public string StringEndTime { get; set; }
        public double? PassingMarks { get; set; }
        public double? MaxMarks { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int? SubjectId { get; set; }
        public string Subject { get; set; }
        public string DifficultyLevel { get; set; }
        public bool IsTimeBound { get; set; }
        public int[] SelectedStudent { get; set; }
        public int ClassDivisionId { get; set; }

        public IList<AssessmentStudentModel> List { get; set; }
        public IList<SelectListItem> AvailableClassDivisions { get; set; }

    }
}
