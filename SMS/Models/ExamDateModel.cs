using System;
using EF.Services;

namespace SMS.Models
{
	public partial class ExamDateModel : BaseEntityModel
	{
		public ExamDateModel()
		{
			Exam = new ExamModel();
			Class = new ClassModel();
			Division = new DivisionModel();
		}
		public int ExamId { get; set; }
		public int DivisionId { get; set; }
		public int ClassId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Ex_Rec_Id { get; set; }
		public ExamModel Exam { get; set; }
		public ClassModel Class { get; set; }
		public DivisionModel Division { get; set; }
	}
}
