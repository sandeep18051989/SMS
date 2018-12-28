using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SMS.Models
{
	[Validator(typeof(SubjectModelValidator))]
	public partial class SubjectModel : BaseEntityModel
	{
		public SubjectModel()
		{
            AvailableAcadmicYears = new List<SelectListItem>();
		}
        public string Name { get; set; }
        public Guid SubjectUniqueId { get; set; }
        public string Code { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }

        public bool Selected { get; set; }
        public bool IsActive { get; set; }
        public int AcadmicYearId { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

    }
}
