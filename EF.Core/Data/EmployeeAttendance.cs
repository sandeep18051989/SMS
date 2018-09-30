using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class EmployeeAttendance : BaseEntity
	{
		public int EmployeeId { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public int AttendanceStatusId { get; set; }
		public bool IsHoliday { get; set; }
		public DateTime Date { get; set; }
		public int AcadmicYearId { get; set; }

		[NotMapped]
		public AttendanceStatus AttendanceStatus
		{
			get
			{
				return (AttendanceStatus)this.AttendanceStatusId;
			}
			set
			{
				this.AttendanceStatusId = (int)value;
			}
		}
		public virtual Employee Employee { get; set; }
	}
}
