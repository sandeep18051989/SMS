using System.ComponentModel;

namespace EF.Core.Enums
{
	public enum BookStatus
	{
        [Description("Available")]
        Available = 1,

        [Description("Coming Soon")]
        ComingSoon = 2,

        [Description("Unavailable")]
        Unavailable = 3,

        [Description("Issued")]
        Issued = 4
	}
}
