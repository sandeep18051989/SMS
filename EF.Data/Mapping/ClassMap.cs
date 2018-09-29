using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassMap : CMSEntityTypeConfiguration<Class>
	{

		public ClassMap()
		{
			this.ToTable("Class");
			this.HasKey(b => b.Id);
			EntityTracker.TrackAllProperties<Class>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
