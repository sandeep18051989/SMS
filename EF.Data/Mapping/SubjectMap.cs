using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class SubjectMap : CMSEntityTypeConfiguration<Subject>
	{
		public SubjectMap()
		{
			this.ToTable("Subject");
			this.HasKey(b => b.Id);
			EntityTracker.TrackAllProperties<Subject>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
