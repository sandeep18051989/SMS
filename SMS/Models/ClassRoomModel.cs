using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(ClassRoomModelValidator))]
	public partial class ClassRoomModel : BaseEntityModel
	{

        public ClassRoomModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
        public string Number { get; set; }
		public string Description { get; set; }

        public bool IsActive { get; set; }

        public int AcadmicYearId { get; set; }

        public string AcadmicYear { get; set; }

        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
    }
}
