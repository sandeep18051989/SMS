using EF.Services;
using System;

namespace SMS.Models
{
    public partial class QuestionModel  : BaseEntityModel
    {
        public string Name { get; set; }
        public string Explanation { get; set; }
        public int QuestionTypeId { get; set; }
        public int? SubjectId { get; set; }
        public Guid QuestionGuid { get; set; }
        public DateTime? SolveTime { get; set; }
        public int DifficultyLevelId { get; set; }
        public double RightMarks { get; set; }
        public double NegativeMarks { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}