using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(CasteModelValidator))]
	public partial class CasteModel : BaseEntityModel
	{
		public CasteModel()
		{
            AvailableCategories = new List<CategoryModel>();
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableReligions = new List<SelectListItem>();
        }

		public IList<CategoryModel> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<SelectListItem> AvailableReligions { get; set; }
        public int ReligionId { get; set; }
		public string Name { get; set; }
		public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public bool Selected { get; set; }
        public bool IsActive { get; set; }
        public string Religion { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

    }
}
