namespace EF.Core.Data
{
    public partial class QuestionType : BaseEntity
	{
		public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemDefined { get; set; }
	}
}
