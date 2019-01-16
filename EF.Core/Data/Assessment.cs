using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Core.Data
{
	public partial class Assessment : BaseEntity
	{
        public string Name { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool OpenToAnonymousUsers { get; set; }
		public double? PassingMarks { get; set; }
		public double? MaxMarks { get; set; }
		public string Url { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public string Instructions { get; set; }
		public int DifficultyLevelId { get; set; }
        public int? SubjectId { get; set; }
        public bool IsTimeBound { get; set; }
		public int? TotalQuestions { get; set; }
		public double? DurationInMinutes { get; set; }
		public bool MandatoryToSolveAll { get; set; }
		public bool AllowUserToMoveForwardBackward { get; set; }
		public string MessageOnSubmitTest { get; set; }
		public bool ShowResultToCandidate { get; set; }
        public int SignaturePictureId { get; set; }
        public int LogoPictureId { get; set; }

        [NotMapped]
		public DifficultyLevel DifficultyLevel
		{
			get
			{
				return (DifficultyLevel)this.DifficultyLevelId;
			}
			set
			{
				this.DifficultyLevelId = (int)value;
			}
		}

    }
}
