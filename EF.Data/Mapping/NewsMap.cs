using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class NewsMap : CMSEntityTypeConfiguration<News>
	{
		public NewsMap()
		{
			this.ToTable("News");
			this.HasKey(e => e.Id);

			// Relationships
			this.HasMany(e => e.Comments).WithMany(c => c.News).Map(m => m.ToTable("News_Comment_Map").MapLeftKey("NewsId").MapRightKey("CommentId"));
			this.HasMany(e => e.Files).WithMany(c => c.News).Map(m => m.ToTable("News_File_Map").MapLeftKey("NewsId").MapRightKey("FileId"));
			this.HasMany(e => e.Users).WithMany(c => c.News).Map(m => m.ToTable("News_User_Map").MapLeftKey("NewsId").MapRightKey("UserId"));
			EntityTracker.TrackAllProperties<News>().Except(x => x.Videos).And(x => x.Comments).And(x => x.AcadmicYearId).And(x => x.Pictures).And(x => x.Users).And(x => x.Files).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
