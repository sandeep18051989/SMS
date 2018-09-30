using EF.Core.Data;
using System.Collections;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface ISliderService
    {
        void Insert(Slider sliders);
        void Update(Slider sliders);
        Slider GetSlider(bool active = true);
        Slider GetSlider(int id);

        void DeleteSliderPictures(Slider slider, IList<Picture> pictures);
        IList<Picture> GetPicturesByIds(int[] pictureIds);
        void TogglePictures(Slider slider, IList<Picture> pictures);

    }
}
