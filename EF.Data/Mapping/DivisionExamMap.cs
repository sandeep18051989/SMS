using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionExamMap : CMSEntityTypeConfiguration<DivisionExam>
	{

		public DivisionExamMap()
		{
			this.ToTable("Division_Exam_Mapping");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);

			EntityTracker.TrackAllProperties<DivisionExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom);

		}
	}
}
