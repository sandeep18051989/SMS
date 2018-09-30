using System;
using EF.Services;

namespace SMS.Models
{
	public partial class MarksModel : BaseEntityModel
	{
		public MarksModel()
		{
			Student = new StudentModel();
			Exam = new ExamModel();
			Subject = new SubjectModel();
		}
		public string SubjectName { get; set; }
		public double EMarks { get; set; }
		public double OutOf { get; set; }
		public string ExamDate { get; set; }
		public DateTime DDate { get; set; }
		public StudentModel Student { get; set; }
		public ExamModel Exam { get; set; }
		public SubjectModel Subject { get; set; }
	}
}
