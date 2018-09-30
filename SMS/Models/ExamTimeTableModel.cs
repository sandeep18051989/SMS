using EF.Services;

namespace SMS.Models
{
	public partial class ExamTimeTableModel : BaseEntityModel
	{
		public ExamTimeTableModel()
		{
			Exam = new ExamModel();
			Class = new ClassModel();
			Division = new DivisionModel();
		}
		public int ExamId { get; set; }
		public int DivisionId { get; set; }
		public int ClassId { get; set; }
		public int ExamDate { get; set; }
		public int ExamMonth { get; set; }
		public int ExamYear { get; set; }
		public ExamModel Exam { get; set; }
		public ClassModel Class { get; set; }
		public DivisionModel Division { get; set; }
	}
}
