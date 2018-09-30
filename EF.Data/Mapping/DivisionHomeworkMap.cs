using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionHomeworkMap : CMSEntityTypeConfiguration<DivisionHomework>
	{

		public DivisionHomeworkMap()
		{
			this.ToTable("Division_Homework_Mapping");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.Division).WithMany().HasForeignKey(ca => ca.DivisionId);
			this.HasRequired(ca => ca.Homework).WithMany().HasForeignKey(ca => ca.HomeworkId);

			EntityTracker.TrackAllProperties<DivisionHomework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Division).And(x => x.Homework);

		}
	}
}
