using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class AcadmicYearMap : CMSEntityTypeConfiguration<AcadmicYear>
	{
		public AcadmicYearMap()
		{
			this.ToTable("AcadamicYear");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.User).WithMany().HasForeignKey(all => all.UserId);
			EntityTracker.TrackAllProperties<AcadmicYear>().Except(x => x.User).And(x => x.CreatedOn).And(x => x.ModifiedOn);
 		}
	}
}
