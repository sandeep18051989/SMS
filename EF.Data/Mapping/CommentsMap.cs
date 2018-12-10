using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class CommentsMap : CMSEntityTypeConfiguration<Comment>
	{
		public CommentsMap()
		{
			this.ToTable("Comment");
			this.HasKey(c => c.Id);
            this.Property(b => b.BlockedBy).IsOptional();
            this.Property(b => b.BlockReason).IsOptional();
            this.Property(b => b.CommentHtml).IsRequired();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Username).IsOptional();

            EntityTracker.TrackAllProperties<Comment>().Except(x => x.Blogs).And(x => x.Events).And(x => x.Products).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
