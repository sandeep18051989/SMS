using SMS.Validations;
using EF.Services;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(FeedbackModelValidator))]
	public partial class FeedbackModel : BaseEntityModel
	{
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string ContactNumber { get; set; }

		public DateTime Date { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		public bool SentSuccess { get; set; }
		public string Location { get; set; }
	}
}