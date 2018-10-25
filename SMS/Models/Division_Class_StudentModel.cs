using System;
using EF.Services;
using FluentValidation.Attributes;

namespace SMS.Models
{
	public partial class Division_Class_StudentModel : BaseEntityModel
	{
		public string RollNumber { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Bonafied_Status { get; set; }
		public int StudentId { get; set; }
		public int ClassId { get; set; }
		public int DivisionId { get; set; }
	}
}
