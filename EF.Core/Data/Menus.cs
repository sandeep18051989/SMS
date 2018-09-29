namespace EF.Core.Data
{
	public partial class Menus : BaseEntity
	{
		public string Name { get; set; }
		public int DisplayOrder { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public int ParentMenuId { get; set; }

		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

	}
}
