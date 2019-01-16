using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
    [Validator(typeof(AssessmentStudentModelValidator))]
    public partial class AssessmentStudentModel : BaseEntityModel
    {
        public AssessmentStudentModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableResultStatuses = new List<SelectListItem>();
            AvailableGradeSystem = new List<SelectListItem>();
        }
        public int StudentId { get; set; }
        public int ResultStatusId { get; set; }
        public int GradeSystemId { get; set; }
        public DateTime? StartOn { get; set; }
        public DateTime? EndOn { get; set; }
        public bool IsExpired { get; set; }
        public string Url { get; set; }
        public double MarksObtained { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public string CertificateHtml { get; set; }
        public IList<SelectListItem> AvailableResultStatuses { get; set; }
        public IList<SelectListItem> AvailableGradeSystem { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

    }
}