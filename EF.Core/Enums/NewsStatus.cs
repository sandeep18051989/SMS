using System.ComponentModel;

namespace EF.Core.Enums
{
    public enum NewsStatus
    {
        [Description("Breaking")]
        Breaking = 1,

        [Description("Expired")]
        Expired = 2,

        [Description("Latest")]
        Latest = 3,

        [Description("Important")]
        Important = 4
    }
}
