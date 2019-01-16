using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class AssessmentStudent : BaseEntity
	{
		public int StudentId { get; set; }
		public int AssessmentId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }
		public DateTime? StartOn { get; set; }
		public DateTime? EndOn { get; set; }
		public bool IsExpired { get; set; }
		public string Url { get; set; }
		public double MarksObtained { get; set; }
		public virtual Assessment Assessment { get; set; }
		public virtual Student Student { get; set; }
		public bool IsActive { get; set; }
		public bool IsCompleted { get; set; }
        public string CertificateHtml { get; set; }

		[NotMapped]
		public ResultStatus ResultStatus
		{
			get
			{
				return (ResultStatus)this.ResultStatusId;
			}
			set
			{
				this.ResultStatusId = (int)value;
			}
		}
		[NotMapped]
		public GradeSystem GradeSystem
		{
			get
			{
				return (GradeSystem)this.GradeSystemId;
			}
			set
			{
				this.GradeSystemId = (int)value;
			}
		}
	}
}
