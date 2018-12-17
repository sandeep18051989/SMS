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
            this.Property(b => b.BodyHtml).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.IncludeInFooterColumn1).IsOptional();
            this.Property(b => b.IncludeInFooterColumn2).IsOptional();
            this.Property(b => b.IncludeInFooterColumn3).IsOptional();
            this.Property(b => b.IncludeInFooterMenu).IsOptional();
            this.Property(b => b.IncludeInTopMenu).IsOptional();
            this.Property(b => b.MetaDescription).IsOptional();
            this.Property(b => b.MetaKeywords).IsOptional();
            this.Property(b => b.MetaTitle).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.PermissionOriented).IsOptional();
            this.Property(b => b.PermissionRecordId).IsOptional();
            this.Property(b => b.SystemName).IsOptional();
            this.Property(b => b.TemplateId).IsRequired();
            this.Property(b => b.Url).IsOptional();

            // Relationships
            this.HasRequired(temp => temp.Template)
				 .WithMany()
				 .HasForeignKey(temp => temp.TemplateId);
			this.HasRequired(cust => cust.User)
				 .WithMany()
				 .HasForeignKey(cust => cust.UserId);
			this.HasOptional(cust => cust.PermissionRecord)
					 .WithMany()
					 .HasForeignKey(cust => cust.PermissionRecordId);

			EntityTracker.TrackAllProperties<CustomPage>().Except(x => x.User).And(x => x.PermissionRecord).And(x => x.ModifiedOn).And(x => x.CreatedOn);

		}
	}
}
