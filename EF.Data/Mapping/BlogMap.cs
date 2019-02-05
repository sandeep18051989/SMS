using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BlogMap : CMSEntityTypeConfiguration<Blog>
	{
		public BlogMap()
		{
			ToTable("Blog");
			HasKey(b => b.Id);
			Property(b => b.BlogHtml).IsRequired();
			Property(b => b.Email).IsRequired();
			Property(b => b.IpAddress).IsOptional();
			Property(b => b.Name).IsRequired();
			Property(b => b.Subject).IsRequired();
            Property(b => b.SystemName).HasMaxLength(100).IsRequired();

            // Relationships
            HasMany(b => b.Comments).WithMany(c => c.Blogs).Map(m => m.ToTable("Blog_Comment_Map").MapLeftKey("BlogId").MapRightKey("CommentId"));
			HasMany(b => b.Files).WithMany(c => c.Blogs).Map(m => m.ToTable("Blog_File_Map").MapLeftKey("BlogId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<Blog>().Except(x => x.Comments).And(x => x.Files).And(x => x.CreatedOn).And(x => x.Pictures).And(x => x.Videos).And(x => x.ModifiedOn);

		}
	}
}
