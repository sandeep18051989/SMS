using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validation;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
	[Validator(typeof(BookModelValidator))]
	public partial class BookModel : BaseEntityModel
	{
		public BookModel()
		{
			AvailableBookStatuses = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string BookStatus { get; set; }
        public string AcadmicYear { get; set; }
        public double Price { get; set; }
        public int BookStatusId { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int AcadmicYearId { get; set; }
        public IList<SelectListItem> AvailableBookStatuses { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

    }
}
