using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TeacherExamMap : CMSEntityTypeConfiguration<TeacherExam>
	{

		public TeacherExamMap()
		{
			this.ToTable("Teacher_Exam");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.Teacher).WithMany().HasForeignKey(ca => ca.TeacherId);
			this.HasRequired(ca => ca.Exam).WithMany().HasForeignKey(ca => ca.ExamId);
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);
			EntityTracker.TrackAllProperties<TeacherExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom).And(x => x.Teacher).And(x => x.Exam);

		}
	}
}
