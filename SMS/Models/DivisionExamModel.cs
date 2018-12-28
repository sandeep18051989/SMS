using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(DivisionExamModelValidator))]
	public partial class DivisionExamModel : BaseEntityModel
	{
		public DivisionExamModel()
		{
			AvailableResultStatuses = new List<SelectListItem>();
			AvailableGradeSystem = new List<SelectListItem>();
            Comments = new List<CommentModel>();
            AvailableAcadmicYears = new List<SelectListItem>();
        }
		public int DivisionId { get; set; }
		public int ExamId { get; set; }
        public int ClassId { get; set; }
        public string Class { get; set; }
        public string Division { get; set; }
        public string Exam { get; set; }
        public int? ResultStatusId { get; set; }
		public int? GradeSystemId { get; set; }
		public double? MarksObtained { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool BreakAllowed { get; set; }
        public bool Selected { get; set; }
        public DateTime? BreakTime { get; set; }
		public int ClassRoomId { get; set; }
        public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public string ClassRoom { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<CommentModel> Comments { get; set; }
    }
}
