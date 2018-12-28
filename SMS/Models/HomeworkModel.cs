using System.Collections.Generic;
using System.Web.Mvc;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;

namespace SMS.Models
{
	[Validator(typeof(HomeworkModelValidator))]
	public partial class HomeworkModel : BaseEntityModel
	{
		public HomeworkModel()
		{
			Comments = new List<CommentModel>();
            AvailableAcadmicYears = new List<SelectListItem>();
        }
		public string Name { get; set; }
		public string Description { get; set; }
		public int AcadmicYearId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CreatedOnString { get; set; }
        public bool Selected { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
		public IList<CommentModel> Comments { get; set; }

	}
}
