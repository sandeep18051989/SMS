using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ConfigurationSettingsModelValidator))]
	public class ConfigurationSettingsModel : BaseEntityModel
	{
		public ConfigurationSettingsModel()
		{
			AvailableLocations = new List<SelectListItem>();
			AvailableForgotPasswordTemplates = new List<SelectListItem>();
			AvailableVisitorQueryTemplates = new List<SelectListItem>();
			AvailableCommentOnEventTemplates = new List<SelectListItem>();
			AvailableCommentOnProductTemplates = new List<SelectListItem>();
			AvailableProductAddedTemplates = new List<SelectListItem>();
			AvailableReplyOnCommentTemplates = new List<SelectListItem>();
			AvailableNewUserRegisterTemplates = new List<SelectListItem>();
			AvailableUserSignInAttemptTemplates = new List<SelectListItem>();
			AvailableRequestQuoteTemplates = new List<SelectListItem>();
			AvailableCommentOnBlogTemplates = new List<SelectListItem>();
		}
		public int ItemsPerPage { get; set; }
		public string PagerLocation { get; set; }
		public string ActiveSettings { get; set; }
		public string Result { get; set; }

		public IList<SelectListItem> AvailableLocations { get; set; }

		public IList<SelectListItem> AvailableForgotPasswordTemplates { get; set; }
		public IList<SelectListItem> AvailableVisitorQueryTemplates { get; set; }
		public IList<SelectListItem> AvailableCommentOnEventTemplates { get; set; }
		public IList<SelectListItem> AvailableCommentOnProductTemplates { get; set; }
		public IList<SelectListItem> AvailableProductAddedTemplates { get; set; }
		public IList<SelectListItem> AvailableReplyOnCommentTemplates { get; set; }
		public IList<SelectListItem> AvailableNewUserRegisterTemplates { get; set; }
		public IList<SelectListItem> AvailableUserSignInAttemptTemplates { get; set; }
		public IList<SelectListItem> AvailableRequestQuoteTemplates { get; set; }
		public IList<SelectListItem> AvailableCommentOnBlogTemplates { get; set; }

		public string SelectedVisitorQueryTemplate { get; set; }
		public string SelectedCommentOnBlogTemplate { get; set; }
		public string CommentOnEventTemplate { get; set; }
		public string CommentOnProductTemplate { get; set; }
		public string ProductAddedTemplate { get; set; }
		public string ReplyOnCommentTemplate { get; set; }
		public string NewUserRegisterTemplate { get; set; }
		public string UserSignInAttemptTemplate { get; set; }
		public string RequestQuoteTemplate { get; set; }
		public string SelectedEmailTemplateForForgotPassword { get; set; }
	}
}