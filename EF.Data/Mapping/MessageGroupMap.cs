﻿using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class MessageGroupMap : CMSEntityTypeConfiguration<MessageGroup>
	{
		public MessageGroupMap()
		{
			this.ToTable("MessageGroup");
			this.HasKey(b => b.Id);
            this.Property(e => e.Description).IsOptional();
            this.Property(e => e.Name).IsRequired();

            // Relationships
            this.HasMany(e => e.ClassRoomDivisions).WithMany(p => p.MessageGroups).Map(m => m.ToTable("MessageGroup_Class_Division_Map").MapLeftKey("MessageGroupId").MapRightKey("ClassRoomDivisionId"));
			this.HasMany(e => e.Students).WithMany(p => p.MessageGroups).Map(m => m.ToTable("MessageGroup_Student_Map").MapLeftKey("MessageGroupId").MapRightKey("StudentId"));
			this.HasMany(e => e.Teachers).WithMany(p => p.MessageGroups).Map(m => m.ToTable("MessageGroup_Teacher_Map").MapLeftKey("MessageGroupId").MapRightKey("TeacherId"));

			EntityTracker.TrackAllProperties<MessageGroup>().Except(x => x.CreatedOn).And(x => x.ClassRoomDivisions).And(x => x.Students).And(x => x.Teachers).And(x => x.ModifiedOn);

		}
	}
}
