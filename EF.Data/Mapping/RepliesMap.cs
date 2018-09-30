using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class RepliesMap : CMSEntityTypeConfiguration<Reply>
    {
        public RepliesMap()
        {
            this.ToTable("Reply");
            this.HasKey(rep => rep.Id);

            this.HasRequired(rep => rep.Comment)
                .WithMany()
                .HasForeignKey(com => com.CommentId);
        }
    }
}
