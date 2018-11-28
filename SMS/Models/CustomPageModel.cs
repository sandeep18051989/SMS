using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(CustomPageModelValidator))]
	public partial class CustomPageModel : BaseEntityModel
	{
		public CustomPageModel()
		{
			template = new TemplateModel();
			AvailableTemplates = new List<SelectListItem>();
		}
		public string Name { get; set; }
		[AllowHtml]
		[UIHint("HtmlTemplate")]
		public string BodyHtml { get; set; }
		public string Url { get; set; }
		public int TemplateId { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsActive { get; set; }
		public string SystemName { get; set; }
		public bool IncludeInTopMenu { get; set; }
		public bool IncludeInFooterMenu { get; set; }
		public bool IncludeInFooterColumn1 { get; set; }
		public bool IncludeInFooterColumn2 { get; set; }
		public bool IncludeInFooterColumn3 { get; set; }

		public string MetaKeywords { get; set; }
	    [AllowHtml]
	    [UIHint("HtmlTemplate")]
        public string MetaDescription { get; set; }

		public string MetaTitle { get; set; }

		public int PermissionRecordId { get; set; }
		public int DisplayOrder { get; set; }
		public bool PermissionOriented { get; set; }
		public IList<SelectListItem> AvailableTemplates { get; set; }
		public TemplateModel template { get; set; }
	}
}