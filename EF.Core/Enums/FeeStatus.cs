using System.ComponentModel;

namespace EF.Core.Enums
{
    public enum FeeStatus
	{
        [Description("Paid")]
        Paid,

        [Description("Partially Paid")]
        PartiallyPaid,

        [Description("UnPaid")]
        Unpaid,

        [Description("Waiting")]
        Waiting
    }
}
