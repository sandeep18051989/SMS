namespace EF.Core.Data
{
	public partial class Feedback : BaseEntity
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Contact { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public bool IsDeleted { get; set; }
	}
}
