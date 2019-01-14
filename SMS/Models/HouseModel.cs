using EF.Core.Data;
using EF.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
	public partial class HouseModel : BaseEntityModel
	{
        public HouseModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
        public string Name { get; set; }
        public string PictureSrc { get; set; }
        public bool IsActive { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
	}
}
