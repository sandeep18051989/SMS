using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(SubjectModelValidator))]
	public partial class SubjectModel : BaseEntityModel
	{
		public SubjectModel()
		{
			Division_Class_Subject = new Division_Class_SubjectModel();
		}
		public string Name { get; set; }
		public double Marks { get; set; }
		public double PassMarks { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
		public Division_Class_SubjectModel Division_Class_Subject { get; set; }

		public partial class Division_Class_SubjectModel : BaseEntityModel
		{
			public Division_Class_SubjectModel()
			{
				Class = new ClassModel();
				Division = new DivisionModel();
			}
			public int SubjectId { get; set; }
			public int ClassId { get; set; }
			public int DivisionId { get; set; }
			public ClassModel Class { get; set; }
			public DivisionModel Division { get; set; }
		}
	}
}
