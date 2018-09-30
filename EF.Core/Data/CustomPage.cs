namespace EF.Core.Data
{
	public partial class CustomPage : BaseEntity, ISlugSupported
	{
		public string Name { get; set; }
		public bool IncludeInTopMenu { get; set; }
		public bool IncludeInFooterMenu { get; set; }
		public bool IncludeInFooterColumn1 { get; set; }
		public bool IncludeInFooterColumn2 { get; set; }
		public bool IncludeInFooterColumn3 { get; set; }
		public int DisplayOrder { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaDescription { get; set; }
		public string MetaTitle { get; set; }
		public string BodyHtml { get; set; }
		public bool PermissionOriented { get; set; }
		public int PermissionRecordId { get; set; }
		public string Url { get; set; }
		public int TemplateId { get; set; }
		public bool IsSystemDefined { get; set; }
		public virtual Template Template { get; set; }
		public virtual User User { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public string SystemName { get; set; }


		#region Navigation Properties
		public virtual PermissionRecord PermissionRecord { get; set; }
		#endregion
	}
}
