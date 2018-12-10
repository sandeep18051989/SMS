using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BlogMap : CMSEntityTypeConfiguration<Blog>
	{
		public BlogMap()
		{
			this.ToTable("Blog");
			this.HasKey(b => b.Id);
            this.Property(b => b.BlogHtml).IsRequired();
            this.Property(b => b.Email).IsRequired();
            this.Property(b => b.IpAddress).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.Subject).IsRequired();

            // Relationships
            this.HasMany(b => b.Videos).WithMany(v => v.Blogs).Map(m => m.ToTable("Blog_Video_Map").MapLeftKey("BlogId").MapRightKey("VideoId"));
			this.HasMany(b => b.Comments).WithMany(c => c.Blogs).Map(m => m.ToTable("Blog_Comment_Map").MapLeftKey("BlogId").MapRightKey("CommentId"));
			this.HasMany(b => b.Files).WithMany(c => c.Blogs).Map(m => m.ToTable("Blog_File_Map").MapLeftKey("BlogId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<Blog>().Except(x => x.Comments).And(x => x.Files).And(x => x.CreatedOn).And(x => x.Pictures).And(x => x.Videos).And(x => x.ModifiedOn);

		}
	}
}
