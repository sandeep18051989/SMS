﻿using System.Collections.Generic;
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
		    AvailableTemplates = new List<SelectListItem>();
		}
		public int ItemsPerPage { get; set; }
		public string PagerLocation { get; set; }
		public string ActiveSettings { get; set; }
		public string Result { get; set; }

		public IList<SelectListItem> AvailableLocations { get; set; }

		public IList<SelectListItem> AvailableTemplates { get; set; }
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