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
            this.Property(b => b.BlogId).IsOptional();
            this.Property(b => b.CommentId).IsOptional();
            this.Property(b => b.EventId).IsOptional();
            this.Property(b => b.NewsId).IsOptional();
            this.Property(b => b.PictureId).IsOptional();
            this.Property(b => b.ProductId).IsOptional();
            this.Property(b => b.Rating).IsOptional();
            this.Property(b => b.ReplyId).IsOptional();
            this.Property(b => b.VideoId).IsOptional();
            this.Property(b => b.Username).IsOptional();

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
