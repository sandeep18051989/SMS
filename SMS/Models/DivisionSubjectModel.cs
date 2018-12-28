using EF.Services;
using FluentValidation.Attributes;

namespace SMS.Models
{
	[Validator(typeof(DivisionSubjectModelValidator))]
	public partial class DivisionSubjectModel : BaseEntityModel
	{
		public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
    }
}
