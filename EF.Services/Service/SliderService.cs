using EF.Core.Data;
using System.Linq;
using System.Collections.Generic;
using EF.Data;
using System;
using EF.Core;

namespace EF.Services.Service
{
    public class SliderService : ISliderService
    {
        public readonly IRepository<Slider> _sliderRepository;
        public readonly IRepository<Picture> _pictureRepository;
        public readonly IPictureService _pictureService;
        public SliderService(IRepository<Slider> sliderRepository, IRepository<Picture> pictureRepository, IPictureService pictureService)
        {
            this._sliderRepository = sliderRepository;
            this._pictureRepository = pictureRepository;
            this._pictureService = pictureService;
        }
        #region ISliderService Members

        public void Insert(Slider slider)
        {
            _sliderRepository.Insert(slider);
        }

        public void Update(Slider slider)
        {
            _sliderRepository.Update(slider);
        }

        #endregion

        #region Utilities
        public Slider GetSlider(bool active = true)
        {
            return _sliderRepository.Table.FirstOrDefault();
        }

        public Slider GetSlider(int id)
        {
            if (id == 0)
                throw new Exception("Slider Id Missing");

            return _sliderRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        public virtual void DeleteSliderPictures(Slider slider, IList<Picture> pictures)
        {
            if (pictures == null)
                throw new ArgumentNullException("pictures");

            foreach (var _picture in pictures)
            {
                slider.Pictures.Remove(_picture);
                _sliderRepository.Update(slider);
                _pictureService.Delete(_picture.Id);
            }
        }

        public virtual IList<Picture> GetPicturesByIds(int[] pictureIds)
        {
            if (pictureIds == null || pictureIds.Length == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                        where pictureIds.Contains(p.Id)
                        select p;

            var pictures = query.ToList();

            var sortedPictures = new List<Picture>();
            foreach (int id in pictureIds)
            {
                var pic = pictures.Find(x => x.Id == id);
                if (pic != null)
                    sortedPictures.Add(pic);
            }
            return sortedPictures;
        }

        public void TogglePictures(Slider slider, IList<Picture> pictures)
        {
            if (pictures == null)
                throw new ArgumentNullException("pictures");

            foreach (var _picture in slider.Pictures)
            {
                if (pictures.FirstOrDefault(x => x.Id == _picture.Id) != null)
                {
                    _picture.IsActive = !_picture.IsActive;
                    _pictureService.Update(_picture);
                }
            }

        }

        #endregion
    }
}
