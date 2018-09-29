using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(CategoryModelValidator))]
	public partial class CategoryModel : BaseEntityModel
	{
		public CategoryModel()
		{
			AvailableCastes = new List<SelectListItem>();
		}
		public IList<SelectListItem> AvailableCastes { get; set; }
		public string Name { get; set; }
		public int CasteId { get; set; }

	}
}
