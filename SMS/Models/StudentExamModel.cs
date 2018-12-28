using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(StudentExamModelValidator))]
	public partial class StudentExamModel : BaseEntityModel
	{
		public StudentExamModel()
		{
			AvailableGradeSystem = new List<SelectListItem>();
			AvailableResultStatuses = new List<SelectListItem>();
            Comments = new List<CommentModel>();
        }
        public int StudentId { get; set; }
        public string Student { get; set; }
        public string RollNumber { get; set; }
        public int ExamId { get; set; }
		public string Exam { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }
		public double MarksObtained { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime? BreakTime { get; set; }
		public int ClassRoomId { get; set; }
        public string ClassRoom { get; set; }
        public IList<CommentModel> Comments { get; set; }
        public bool Selected { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }

	}
}
