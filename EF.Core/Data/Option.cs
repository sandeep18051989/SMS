namespace EF.Core.Data
{
	public partial class Option : BaseEntity
	{
		public string Name { get; set; }
		public string CorrectAnswer { get; set; }
		public int DisplayOrder { get; set; }
		public int QuestionId { get; set; }
		public virtual Question Question { get; set; }
	}
}
