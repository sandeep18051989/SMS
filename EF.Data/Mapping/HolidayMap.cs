﻿using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HolidayMap : CMSEntityTypeConfiguration<Holiday>
	{
		public HolidayMap()
		{
			this.ToTable("Holiday");
			this.HasKey(ho => ho.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.DD).IsRequired();
            this.Property(b => b.MM).IsRequired();
            this.Property(b => b.YYYY).IsRequired();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.Date).IsRequired();


            EntityTracker.TrackAllProperties<File>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
