using System;

namespace EF.Core.Data
{
	public partial class Location : BaseEntity
	{
		public virtual User User { get; set; }
		public string Address { get; set; }
		public string Host { get; set; }
		public string Area { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
	}
}
