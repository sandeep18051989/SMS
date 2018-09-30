using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ExamModelValidator))]
	public partial class ExamModel : BaseEntityModel
	{
		public string ExamName { get; set; }
		public double OutOf { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
