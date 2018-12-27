using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(DivisionModelValidator))]
	public partial class DivisionModel : BaseEntityModel
	{
		public DivisionModel()
		{
		    AvailableAcadmicYears = new List<SelectListItem>();
            AvailableClassRooms = new List<SelectListItem>();
        }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
        public bool Selected { get; set; }
        public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public int DisplayOrder { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public string CreatedOnString { get; set; }
	    public string ModifiedOnString { get; set; }
        public int ClassRoomId { get; set; }
        public IList<SelectListItem> AvailableClassRooms { get; set; }
    }

}
