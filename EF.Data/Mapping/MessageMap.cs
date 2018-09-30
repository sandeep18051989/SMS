using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class MessageMap : CMSEntityTypeConfiguration<Message>
	{
		public MessageMap()
		{
			this.ToTable("Message");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.MessageGroup).WithMany().HasForeignKey(all => all.MessageGroupId);
			this.HasMany(pro => pro.Files).WithMany(p => p.Messages).Map(m => m.ToTable("Message_File_Map").MapLeftKey("MessageId").MapRightKey("FileId"));

			EntityTracker.TrackAllProperties<Message>().Except(x => x.MessageGroup).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
