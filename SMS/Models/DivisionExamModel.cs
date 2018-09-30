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
			ClassRoom = new ClassRoomModel();
			Class = new ClassModel();
			Student = new StudentModel();
			AvailableResultStatuses = new List<SelectListItem>();
			AvailableGradeSystem = new List<SelectListItem>();
		}
		public int DivisionId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }

		public double MarksObtained { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime BreakTime { get; set; }
		public int ClassRoomId { get; set; }
		public int StudentGroup { get; set; }
		public ClassModel Class { get; set; }
		public StudentModel Student { get; set; }
		public ClassRoomModel ClassRoom { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }
	}
}
