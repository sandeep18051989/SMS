using System.ComponentModel;

namespace EF.Core.Enums
{
	public enum NewsStatus
	{
        [Description("Breaking")]
        Breaking = 1,

        [Description("Current")]
        Current = 2,

        [Description("Expired")]
        Expired = 3,

        [Description("Fresh")]
        Fresh = 4,

        [Description("Latest")]
        Latest = 5,

        [Description("Outdated")]
        Outdated = 6,

        [Description("Repeated")]
        Repeated = 7
	}
}
