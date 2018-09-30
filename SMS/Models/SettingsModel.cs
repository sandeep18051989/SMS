namespace SMS.Models
{
	public partial class SettingsModel
    {
        public SettingsModel()
        {
            User = new UserModel();
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public int SettingType { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}