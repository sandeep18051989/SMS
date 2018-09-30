using EF.Services;
using FluentValidation.Attributes;

namespace SMS.Models
{
	[Validator(typeof(DivisionSubjectModelValidator))]
	public partial class DivisionSubjectModel : BaseEntityModel
	{
		public int SubjectId { get; set; }
		public int DivisionId { get; set; }
		public SubjectModel Subject { get; set; }
		public DivisionModel Division { get; set; }
	}
}
