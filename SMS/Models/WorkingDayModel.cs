using EF.Services;

namespace SMS.Models
{
	public partial class WorkingDayModel : BaseEntityModel
	{
		public string Day { get; set; }
		public string Month { get; set; }
		public string Year { get; set; }
	}
}
