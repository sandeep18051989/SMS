using System;

namespace EF.Core.Data
{
	public partial class TimeTableSetting : BaseEntity
	{
		public DateTime SchoolStartTime { get; set; }
		public DateTime SchoolEndTime { get; set; }
		public double LectureTime { get; set; }
		public bool NoBreak { get; set; }
		public double RecessTimeMin { get; set; }
		public int AcadmicYearId { get; set; }
	}
}
