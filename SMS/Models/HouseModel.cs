using EF.Core.Data;
using EF.Services;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
	public partial class HouseModel : BaseEntityModel
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		public PictureModel Picture { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

	}
}
