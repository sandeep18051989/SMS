using EF.Core.Data;

namespace SMS.Models
{
	public partial class HomeHeaderModel
	{
		public bool LogoEnabled { get; set; }
		public int Theme { get; set; }

		public Picture pictures { get; set; }
	}
}