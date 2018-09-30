using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FilesMap : CMSEntityTypeConfiguration<File>
	{
		public FilesMap()
		{
			this.ToTable("Files");
			this.HasKey(f => f.Id);
			this.Property(f => f.UserId);
		}
	}
}
