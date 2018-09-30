using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(AcadmicYearModelValidator))]
	public partial class AcadmicYearModel : BaseEntityModel
	{
		public AcadmicYearModel()
		{
			AvailableAcadmicYears = new List<SelectListItem>();
		}

		public string Name { get; set; }
		public bool IsActive { get; set; }
		public IList<SelectListItem> AvailableAcadmicYears { get; set; }
	}
}
