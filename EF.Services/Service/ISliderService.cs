using EF.Core.Data;
using System.Collections;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface ISliderService
    {
        void Insert(Slider slider);
        void Update(Slider slider);
        void Delete(int id);
        Slider GetSliderById(int id);
        Slider GetSliderByName(string name);
        Slider GetDefaultSlider();
        IList<Slider> GetAllSliders(bool? onlyActive = null, bool? showSystemDefined = null);
        IList<Picture> GetAllSliderPicturesBySliderId(int id, bool? onlyActive = null);
        Slider GetSliderByDisplayArea(string name);
        void DeleteSliderPictures(Slider slider, IList<Picture> pictures);
        IList<Picture> GetPicturesByIds(int[] pictureIds);
        void TogglePictures(Slider slider, IList<Picture> pictures);
        void DeleteSliders(IList<Slider> sliders);
        IList<Slider> GetSliderByIds(int[] roleIds);
        void ToggleActiveStatus(int id);
        void ToggleIndicatorStatus(int id);
        void ToggleCaptionStatus(int id);
        void ToggleThumbnailStatus(int id);
    }
}
