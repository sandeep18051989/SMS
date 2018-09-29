using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EF.Core.Data;
using EF.Services;

namespace SMS.Models
{
	public class SliderSettingsModel : BaseEntityModel
	{
		public SliderSettingsModel()
		{
			Pictures = new List<PictureModel>();
			Slider = new Slider();
		}
		public Slider Slider { get; set; }
		public IList<PictureModel> Pictures { get; set; }

		public bool SliderCaptionOff { get; set; }
		public string MaxPictures { get; set; }
		public string ActiveSettings { get; set; }

		public string MaxHeightSetting { get; set; }
		public string MaxWidthSetting { get; set; }
		[UIHint("SliderPictureUpload")]
		public int PictureId { get; set; }

	}
}