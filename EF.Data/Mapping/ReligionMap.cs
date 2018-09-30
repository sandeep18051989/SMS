using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ReligionMap : CMSEntityTypeConfiguration<Religion>
	{
		public ReligionMap()
		{
			this.ToTable("Religion");
			this.HasKey(b => b.Id);
			EntityTracker.TrackAllProperties<Religion>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
