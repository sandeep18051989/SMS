using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HomeworkMap : CMSEntityTypeConfiguration<Homework>
	{

		public HomeworkMap()
		{
			this.ToTable("Homework");
			this.HasKey(b => b.Id);
			this.HasMany(pro => pro.Comments).WithMany(p => p.Homeworks).Map(m => m.ToTable("Homework_Comment_Map").MapLeftKey("HomeworkId").MapRightKey("CommentId"));

			EntityTracker.TrackAllProperties<Homework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
