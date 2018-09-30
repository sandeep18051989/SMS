using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
	public partial class PicturesMap : CMSEntityTypeConfiguration<Picture>
	{
		public PicturesMap()
		{
			this.ToTable("Picture");
			this.HasKey(p => p.Id);
			this.Property(p => p.UserId);
		}
	}
}
