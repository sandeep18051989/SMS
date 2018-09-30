using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
	public partial class MenusMap : CMSEntityTypeConfiguration<Menus>
	{
		public MenusMap()
		{
			this.ToTable("Menu");
			this.HasKey(m => m.Id);
			this.Property(m => m.Title).IsOptional();

		}
	}
}
