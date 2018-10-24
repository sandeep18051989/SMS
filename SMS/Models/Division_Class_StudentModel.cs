using System;
using EF.Services;
using FluentValidation.Attributes;

namespace SMS.Models
{
	public partial class Division_Class_StudentModel : BaseEntityModel
	{
		public Division_Class_StudentModel()
		{
			Student = new StudentModel();
			Class = new ClassModel();
			Division = new DivisionModel();
		}
		public string RollNumber { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Bonafied_Status { get; set; }
		public StudentModel Student { get; set; }
		public ClassModel Class { get; set; }
		public DivisionModel Division { get; set; }
	}
}
