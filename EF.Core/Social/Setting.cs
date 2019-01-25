using EF.Core;

namespace EF.Core.Social
{
    public partial class SocialSetting : BaseEntity
    {
        public SocialSetting() { }
        
        public SocialSetting(string name, string value) {
            this.Name = name;
            this.Value = value;
        }
        
        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
