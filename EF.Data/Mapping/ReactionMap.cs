using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class ReactionMap : CMSEntityTypeConfiguration<Reaction>
    {
        public ReactionMap()
        {
            this.ToTable("Reaction");
            this.HasKey(p => p.Id);
            this.Property(p => p.UserId);

            this.HasOptional(react => react.Blog).WithMany(c => c.Reactions).HasForeignKey(react => react.BlogId);
            this.HasOptional(react => react.Comment).WithMany(c => c.Reactions).HasForeignKey(react => react.CommentId);
            this.HasOptional(react => react.Event).WithMany(c => c.Reactions).HasForeignKey(react => react.EventId);
            this.HasOptional(react => react.News).WithMany(c => c.Reactions).HasForeignKey(react => react.NewsId);
            this.HasOptional(react => react.Picture).WithMany(c => c.Reactions).HasForeignKey(react => react.PictureId);
            this.HasOptional(react => react.Product).WithMany(c => c.Reactions).HasForeignKey(react => react.ProductId);
            this.HasOptional(react => react.Reply).WithMany(c => c.Reactions).HasForeignKey(react => react.ReplyId);
            this.HasOptional(react => react.Video).WithMany(c => c.Reactions).HasForeignKey(react => react.VideoId);
        }
    }
}
