using System.ComponentModel;

namespace EF.Core.Enums
{
	public enum DisplayAreas
    {
        [Description("Header")]
        Header = 1,
        [Description("Middle")]
        Middle = 2,
        [Description("Footer")]
        Footer = 3
	}
}
