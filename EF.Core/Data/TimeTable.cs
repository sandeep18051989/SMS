using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class TimeTable : BaseEntity
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int LectureNumber { get; set; }
		public bool RecessAllowed { get; set; }
		public int WeekDayId { get; set; }

		public int DivisionSubjectId { get; set; }
		public int TeacherId { get; set; }
		public virtual DivisionSubject DivisionSubject { get; set; }
		public virtual Teacher Teacher { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		[NotMapped]
		public WeekDays WeekDay
		{
			get
			{
				return (WeekDays)this.WeekDayId;
			}
			set
			{
				this.WeekDayId = (int)value;
			}
		}
	}
}
