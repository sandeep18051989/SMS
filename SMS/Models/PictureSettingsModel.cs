using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(PicturesSettingModelValidator))]
	public class PictureSettingsModel
	{
		#region Width Related
		public string MaxWidthAllowedForLargeThumbnails { get; set; }

		public string MaxWidthAllowedForMediumThumbnails { get; set; }

		public string MaxWidthAllowedForSmallThumbnails { get; set; }

		public string MaxWidthAllowedForSliderThumbnails { get; set; }

		#endregion

		#region Height Related

		public string MaxHeightAllowedForLargeThumbnails { get; set; }

		public string MaxHeightAllowedForMediumThumbnails { get; set; }

		public string MaxHeightAllowedForSmallThumbnails { get; set; }

		public string MaxHeightAllowedForSliderThumbnails { get; set; }
		#endregion
		public string[] SelectedPictureTypes { get; set; }
		public string ActiveSettings { get; set; }
		// In Bytes
		public int MaximumSizeAllowed { get; set; }
		public string Result { get; set; }

	}
}