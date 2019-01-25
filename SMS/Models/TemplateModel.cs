using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;

namespace SMS.Models
{
	public partial class TemplateModel : BaseEntityModel
	{
		public TemplateModel()
		{
			customPagesModel = new List<CustomPageModel>();
			dataTokens = new List<DataTokenModel>();
		}
		public int TemplateId { get; set; }
		public string Name { get; set; }
		[AllowHtml]
		[UIHint("HtmlTemplate")]
		public string BodyHtml { get; set; }
		public bool IsSystemDefined { get; set; }

        public string Subject { get; set; }
        public bool IsActive { get; set; }
		public IList<CustomPageModel> customPagesModel { get; set; }

		public IList<DataTokenModel> dataTokens { get; set; }
	}
}