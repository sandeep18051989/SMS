using SMS.Validations;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Areas.Admin.Models
{
	public class ConsolidateUserModel
	{
		public int UniqueCount { get; set; }
		public int ReturnCount { get; set; }
		public string location { get; set; }
		public string address { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public DateTime Date { get; set; }
	}
}