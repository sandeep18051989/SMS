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
            this.Property(b => b.AlternateText).IsOptional();
            this.Property(b => b.AcadmicYearId).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Height).IsOptional();
            this.Property(b => b.PictureSrc).IsRequired();
            this.Property(b => b.Size).IsOptional();
            this.Property(b => b.Url).IsOptional();
            this.Property(b => b.Width).IsOptional();
        }
	}
}
