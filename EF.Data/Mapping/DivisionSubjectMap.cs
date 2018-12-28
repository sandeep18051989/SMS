using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionSubjectMap : CMSEntityTypeConfiguration<DivisionSubject>
	{
		public DivisionSubjectMap()
		{
			this.ToTable("Division_Subject_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.DivisionId).IsRequired();
            this.Property(b => b.SubjectId).IsRequired();

            this.HasRequired(all => all.Subject).WithMany().HasForeignKey(all => all.SubjectId);
			this.HasRequired(all => all.Division).WithMany().HasForeignKey(all => all.DivisionId);

			EntityTracker.TrackAllProperties<DivisionSubject>().Except(x => x.Subject).And(x => x.Division).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
