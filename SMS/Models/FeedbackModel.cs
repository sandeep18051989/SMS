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
	    public string FullName { get; set; }
	    public string Email { get; set; }
	    public string Contact { get; set; }
	    public string Description { get; set; }
	    public string Location { get; set; }

	    public DateTime? Date { get; set; }

	    [AllowHtml]
		[UIHint("HtmlEditor")]
		public bool SentSuccess { get; set; }
	}
}