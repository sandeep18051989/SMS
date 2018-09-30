using System;
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
			Class = new ClassModel();
			ClassRoom = new ClassRoomModel();
		}
		public int ClassId { get; set; }
		public string DivisionName { get; set; }
		public string Description { get; set; }
		public ClassModel Class { get; set; }
		public int ClassRoomId { get; set; }
		public ClassRoomModel ClassRoom { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}

}
