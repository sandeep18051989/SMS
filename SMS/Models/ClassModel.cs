using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ClassModelValidator))]
	public partial class ClassModel : BaseEntityModel
	{
		public string Name { get; set; }
        public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
    }

	public partial class Division_Class_SubjectModel : BaseEntityModel
    {
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int DivisionId { get; set; }
        public SubjectModel Subject { get; set; }
        public DivisionModel Division { get; set; }
    }

}
