using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Core.Data;

namespace EF.Data.Mapping
{
    public class CommentRepliesMap : EntityTypeConfiguration<CommentReplies>
    {
        public CommentRepliesMap()
        {
            Property(t => t.Id).IsOptional();
        }
    }
}
