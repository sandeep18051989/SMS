using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassRoomMap : CMSEntityTypeConfiguration<ClassRoom>
	{

		public ClassRoomMap()
		{
			this.ToTable("Class_Room");
			this.HasKey(b => b.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Number).IsRequired();

            EntityTracker.TrackAllProperties<ClassRoom>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
