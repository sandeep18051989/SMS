using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class CasteMap : CMSEntityTypeConfiguration<Caste>
	{
		public CasteMap()
		{
			this.ToTable("Caste");
			this.HasKey(b => b.Id);

			this.HasRequired(all => all.Religion).WithMany().HasForeignKey(all => all.ReligionId);
			EntityTracker.TrackAllProperties<Caste>().Except(x => x.Religion).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
