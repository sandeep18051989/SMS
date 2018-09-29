using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeedbackMap : CMSEntityTypeConfiguration<Feedback>
	{
		public FeedbackMap()
		{
			this.ToTable("Feedbacks");
			this.HasKey(feed => feed.Id);
			this.Property(feed => feed.UserId).IsOptional();
			this.Property(feed => feed.Location).HasMaxLength(200);
		}
	}
}
