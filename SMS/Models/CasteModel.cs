using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(CasteModelValidator))]
	public partial class CasteModel : BaseEntityModel
	{
		public CasteModel()
		{
			Categories = new List<CategoryModel>();
			Religion = new ReligionModel();
		}

		public IList<CategoryModel> Categories { get; set; }
		public int ReligionId { get; set; }
		public string CasteName { get; set; }
		public ReligionModel Religion { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

	}
}
