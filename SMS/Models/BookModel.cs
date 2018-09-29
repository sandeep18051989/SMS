using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validation;

namespace SMS.Models
{
	[Validator(typeof(BookModelValidator))]
	public partial class BookModel : BaseEntityModel
	{
		public BookModel()
		{
			AvailableBookStatuses = new List<SelectListItem>();
		}

		public string Name { get; set; }
		public string Author { get; set; }
		public double Price { get; set; }
		public int BookStatusId { get; set; }
		public string Description { get; set; }
		public IList<SelectListItem> AvailableBookStatuses { get; set; }

	}
}
