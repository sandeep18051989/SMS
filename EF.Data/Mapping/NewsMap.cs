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
            this.Property(e => e.AcadmicYearId).IsOptional();
            this.Property(e => e.Author).IsRequired();
            this.Property(e => e.Description).IsOptional();
            this.Property(e => e.EndDate).IsOptional();
            this.Property(e => e.NewsStatusId).IsOptional();
            this.Property(e => e.ShortName).IsOptional();
            this.Property(e => e.StartDate).IsOptional();
            this.Property(e => e.Url).IsOptional();

            // Relationships
            this.HasRequired(all => all.User).WithMany(n => n.News).HasForeignKey(all => all.UserId);

            this.HasMany(e => e.Comments).WithMany(c => c.News).Map(m => m.ToTable("News_Comment_Map").MapLeftKey("NewsId").MapRightKey("CommentId"));
			this.HasMany(e => e.Files).WithMany(c => c.News).Map(m => m.ToTable("News_File_Map").MapLeftKey("NewsId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<News>().Except(x => x.Videos).And(x => x.Comments).And(x => x.AcadmicYearId).And(x => x.Pictures).And(x => x.Files).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
