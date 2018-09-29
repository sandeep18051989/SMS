using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(StudentAttendanceModelValidator))]
	public partial class StudentAttendanceModel : BaseEntityModel
	{
		public StudentAttendanceModel()
		{
			Student = new StudentModel();
			AvailableStatuses = new List<SelectListItem>();
		}
		public int StudentId { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public int AttendanceStatusId { get; set; }
		public bool IsHoliday { get; set; }
		public DateTime Date { get; set; }
		public StudentModel Student { get; set; }
		public IList<SelectListItem> AvailableStatuses { get; set; }
	}
}
