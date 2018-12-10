﻿using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DesignationMap : CMSEntityTypeConfiguration<Designation>
	{
		public DesignationMap()
		{
			this.ToTable("Designation");
			this.HasKey(b => b.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();

            EntityTracker.TrackAllProperties<Designation>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
