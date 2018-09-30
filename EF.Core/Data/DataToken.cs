namespace EF.Core.Data
{
	public partial class DataToken : BaseEntity
	{
		public string Name { get; set; }
		public string SystemName { get; set; }
		public string Value { get; set; }
		public bool IsSystemDefined { get; set; }
		public virtual User user { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
	}
}
