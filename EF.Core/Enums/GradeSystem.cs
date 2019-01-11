using System.ComponentModel;

namespace EF.Core.Enums
{
	public enum GradeSystem
	{
        [Description("Letter grading and variations")]
        LetterGrading = 1,

        [Description("Pass/Fail")]
        PassFail = 2,

        [Description("Percentage Grading")]
        PercentageGrading = 3,

    }
}
