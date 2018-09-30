using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(EmployeeAttendanceModelValidator))]
	public partial class EmployeeAttendanceModel : BaseEntityModel
	{
		public EmployeeAttendanceModel()
		{
			Employee = new EmployeeModel();
		}
		public int EmployeeId { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public DateTime AttendanceDate { get; set; }
		public int AttendanceStatusId { get; set; }
		public bool IsHoliday { get; set; }
		public EmployeeModel Employee { get; set; }

		public IPagedList<EmployeeAttendanceModel> EmployeeAttendanceList { get; set; }
	}
}
