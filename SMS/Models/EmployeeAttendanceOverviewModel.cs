using System;
using System.Collections.Generic;
using EF.Services;

namespace SMS.Models
{
	public partial class EmployeeAttendanceOverviewModel : BasePageModel
	{
		public EmployeeAttendanceOverviewModel()
		{
			AvailableEmployees = new List<EmployeeModel>();
		}
		public DateTime Date { get; set; }
		public int SelectedEmployeeId { get; set; }
		public IPagedList<EmployeeAttendanceModel> EmployeeAttendanceList { get; set; }
		public IList<EmployeeModel> AvailableEmployees { get; set; }
	}
}
