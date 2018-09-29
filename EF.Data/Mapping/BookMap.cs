using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BookMap : CMSEntityTypeConfiguration<Book>
	{
		public BookMap()
		{
			this.ToTable("Book");
			this.HasKey(b => b.Id);
			this.Property(b => b.Description).IsOptional();
			this.Property(b => b.Price).IsOptional();

			EntityTracker.TrackAllProperties<Book>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
