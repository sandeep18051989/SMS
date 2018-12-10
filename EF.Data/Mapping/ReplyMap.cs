using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class ReplyMap : CMSEntityTypeConfiguration<Reply>
    {
        public ReplyMap()
        {
            this.ToTable("Reply");
            this.HasKey(rep => rep.Id);
            this.Property(b => b.CommentId).IsRequired();
            this.Property(b => b.Dislikes).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Likes).IsOptional();
            this.Property(b => b.StudentId).IsOptional();
            this.Property(b => b.StudentName).IsOptional();
            this.Property(b => b.TeacherId).IsOptional();
            this.Property(b => b.TeacherName).IsOptional();

            this.HasRequired(rep => rep.Comment)
                .WithMany()
                .HasForeignKey(com => com.CommentId);
        }
    }
}
