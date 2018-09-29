using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassExamMap : CMSEntityTypeConfiguration<ClassExam>
	{

		public ClassExamMap()
		{
			this.ToTable("Class_Exam_Mapping");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.Class).WithMany().HasForeignKey(ca => ca.ClassId);
			this.HasRequired(ca => ca.Exam).WithMany().HasForeignKey(ca => ca.ExamId);
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);

			EntityTracker.TrackAllProperties<ClassExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom).And(x => x.Class).And(x => x.Exam);

		}
	}
}
