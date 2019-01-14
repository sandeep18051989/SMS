using System.ComponentModel;

namespace EF.Core.Enums
{
    public enum DifficultyLevel
    {
        [Description("Easy")]
        Easy = 1,

        [Description("Normal")]
        Normal = 2,

        [Description("Difficult")]
        Difficult = 3,

        [Description("Hard")]
        Hard = 4
    }
}
