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
			ClassRoom = new ClassRoomModel();
			Exam = new ExamModel();
			Teacher = new TeacherModel();
			AvailableResultStatuses = new List<SelectListItem>();
			AvailableGradeSystem = new List<SelectListItem>();
		}
		public int TeacherId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }

		public double MarksObtained { get; set; }
		public ExamModel Exam { get; set; }
		public TeacherModel Teacher { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime BreakTime { get; set; }
		public int ClassRoomId { get; set; }

		public ClassRoomModel ClassRoom { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }

	}
}
