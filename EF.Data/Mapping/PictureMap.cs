using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
	public partial class PictureMap : CMSEntityTypeConfiguration<Picture>
	{
		public PictureMap()
		{
			this.ToTable("Picture");
			this.HasKey(p => p.Id);
            this.Property(b => b.AlternateText).IsOptional();
            this.Property(b => b.AcadmicYearId).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Height).IsOptional();
            this.Property(b => b.PictureSrc).IsRequired();
            this.Property(b => b.Size).IsOptional();
            this.Property(b => b.Url).IsOptional();
            this.Property(b => b.Width).IsOptional();
		    this.Property(b => b.IsOpenResource).IsRequired();
        }
	}
}
