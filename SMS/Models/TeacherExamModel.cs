using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(TeacherExamModelValidator))]
	public partial class TeacherExamModel : BaseEntityModel
	{
		public TeacherExamModel()
		{
			AvailableResultStatuses = new List<SelectListItem>();
			AvailableGradeSystem = new List<SelectListItem>();
            Comments = new List<CommentModel>();
        }
        public int TeacherId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }

		public double MarksObtained { get; set; }
		public string Exam { get; set; }
		public string Teacher { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime BreakTime { get; set; }
		public int ClassRoomId { get; set; }
		public string ClassRoom { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }
        public IList<CommentModel> Comments { get; set; }

    }
}
