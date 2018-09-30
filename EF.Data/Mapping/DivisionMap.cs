using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionMap : CMSEntityTypeConfiguration<Division>
	{
		public DivisionMap()
		{
			this.ToTable("Division");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.Class).WithMany().HasForeignKey(all => all.ClassId);
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);
			EntityTracker.TrackAllProperties<Division>().Except(x => x.Class).And(x => x.CreatedOn).And(x => x.ClassRoom).And(x => x.ModifiedOn);

		}
	}
}
