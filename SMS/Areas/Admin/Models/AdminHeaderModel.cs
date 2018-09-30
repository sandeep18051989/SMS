using EF.Core.Data;

namespace SMS.Areas.Admin.Models
{
	public class AdminHeaderModel
	{
		public bool LogoEnabled { get; set; }
		public int Theme { get; set; }

		public Picture pictures { get; set; }
	}
}