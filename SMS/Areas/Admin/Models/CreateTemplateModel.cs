using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Areas.Admin.Validations;

namespace SMS.Areas.Admin.Models
{
	[Validator(typeof(CreateTemplateModelValidator))]
	public partial class CreateTemplateModel : BaseEntityModel
	{
		public CreateTemplateModel()
		{
			InsertDataTokensModel = new List<CreateDataTokensModel>();
		}
		public string Name { get; set; }
		public string SystemName { get; set; }
		[AllowHtml]
		[UIHint("HtmlTemplate")]
		public string BodyHtml { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsActive { get; set; }
		public IList<CreateDataTokensModel> InsertDataTokensModel { get; set; }

		[Validator(typeof(CreateDataTokenModelValidator))]
		public partial class CreateDataTokensModel : BaseEntityModel
		{
			public string Name { get; set; }
			public string SystemName { get; set; }
			[AllowHtml]
			[UIHint("HtmlTemplate")]
			public string Value { get; set; }
			public bool IsSystemDefined { get; set; }
			public bool IsActive { get; set; }


		}
	}
}