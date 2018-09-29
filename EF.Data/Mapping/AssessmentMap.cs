using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class AssessmentMap : CMSEntityTypeConfiguration<Assessment>
	{
		public AssessmentMap()
		{
			this.ToTable("Assessment");
			this.HasKey(b => b.Id);
			EntityTracker.TrackAllProperties<Assessment>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
