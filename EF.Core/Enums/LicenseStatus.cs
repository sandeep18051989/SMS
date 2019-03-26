using System.ComponentModel;

namespace EF.Core.Enums
{
	public enum LicenseStatus
    {
        [Description("Active")]
        Active = 10,

        [Description("Expired")]
        Inactive = 20,
	}
}
