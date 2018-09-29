using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class CustomPageMap : CMSEntityTypeConfiguration<CustomPage>
	{
		public CustomPageMap()
		{
			this.ToTable("CustomPage");
			this.HasKey(cp => cp.Id);

			// Relationships
			this.HasRequired(temp => temp.Template)
				 .WithMany()
				 .HasForeignKey(temp => temp.TemplateId);
			this.HasRequired(cust => cust.User)
				 .WithMany()
				 .HasForeignKey(cust => cust.UserId);
			this.HasRequired(cust => cust.PermissionRecord)
					 .WithMany()
					 .HasForeignKey(cust => cust.PermissionRecordId);

			EntityTracker.TrackAllProperties<CustomPage>().Except(x => x.User).And(x => x.PermissionRecord).And(x => x.ModifiedOn).And(x => x.CreatedOn);

		}
	}
}
