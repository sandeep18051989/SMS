namespace EF.Core.Data
{
	public partial class House : BaseEntity
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public string Description { get; set; }
		public int PictureId { get; set; }
		public virtual Picture Picture { get; set; }
		public int AcadmicYearId { get; set; }
	}
}
