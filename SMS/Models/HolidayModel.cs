using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(HolidayModelValidator))]
	public partial class HolidayModel : BaseEntityModel
	{
		public DateTime Date { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public string Name { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
