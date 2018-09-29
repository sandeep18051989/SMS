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
			EntityTracker.TrackAllProperties<Comment>().Except(x => x.Blogs).And(x => x.Events).And(x => x.Products).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
