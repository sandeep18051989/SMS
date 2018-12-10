using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SMS.Models
{
	[Validator(typeof(ClassHomeworkModelValidator))]
	public partial class ClassHomeworkModel : BaseEntityModel
	{
		public ClassHomeworkModel()
		{
            AvailableClasses = new List<SelectListItem>();
            AvailableHomeworks = new List<SelectListItem>();
        }
		public int ClassId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<SelectListItem> AvailableHomeworks { get; set; }
		public IList<SelectListItem> AvailableClasses { get; set; }
	}
}
