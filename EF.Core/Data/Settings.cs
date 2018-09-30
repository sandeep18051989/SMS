namespace EF.Core.Data
{
	public partial class Settings : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public int SettingType { get; set; }
        public int TypeId { get; set; }
        public virtual User user { get; set; }
    }
}
