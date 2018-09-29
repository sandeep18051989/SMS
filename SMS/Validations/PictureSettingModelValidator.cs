using System.Linq;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public class PicturesSettingModelValidator : EntityValidatorBase<PictureSettingsModel>
	{
		public PicturesSettingModelValidator()
		{
			RuleFor(x => x.MaxHeightAllowedForLargeThumbnails).NotEmpty().WithMessage("Please Enter Maximum Height Allowed For Large Pictures");
			RuleFor(x => x.MaxHeightAllowedForMediumThumbnails).NotEmpty().WithMessage("Please Enter Maximum Height Allowed For Medium Pictures(For Event, News Pictures)");
			RuleFor(x => x.MaxHeightAllowedForSliderThumbnails).NotEmpty().WithMessage("Please Enter Maximum Height Allowed For Slider Pictures(For Slider Pictures)");
			RuleFor(x => x.MaxHeightAllowedForSmallThumbnails).NotEmpty().WithMessage("Please Enter Maximum Height Allowed For Small Pictures(For General Pictures)");
			RuleFor(x => x.MaxWidthAllowedForLargeThumbnails).NotEmpty().WithMessage("Please Enter Maximum Width Allowed For Large Pictures");
			RuleFor(x => x.MaxWidthAllowedForMediumThumbnails).NotEmpty().WithMessage("Please Enter Maximum Width Allowed For Medium Pictures(For Event, News Pictures)");
			RuleFor(x => x.MaxWidthAllowedForSliderThumbnails).NotEmpty().WithMessage("Please Enter Maximum Width Allowed For Slider Pictures(For Slider Pictures)");
			RuleFor(x => x.MaxWidthAllowedForSmallThumbnails).NotEmpty().WithMessage("Please Enter Maximum Width Allowed For Small Pictures(For General Pictures)");
			RuleFor(x => x.MaximumSizeAllowed).NotEmpty().WithMessage("Please Enter Maximum Size Allowed(In Bytes e.g 1 KB = 1000 Bytes)");
			RuleFor(x => x.SelectedPictureTypes).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("Please select atleast one type").Must(NotEqualZero).WithMessage("Please select atleast one type");
		}

		private bool NotEqualZero(string[] types)
		{
			return types.All(i => !string.IsNullOrEmpty(i));
		}

	}
}