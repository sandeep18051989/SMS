using EF.Core.Data;
using System.Linq;
using System.Collections.Generic;
using EF.Data;
using System;
using System.Linq.Dynamic;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Enums;

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

        public void Delete(int id)
        {
            _sliderRepository.Delete(id);
        }

        #endregion

        #region Utilities

        public IList<Slider> GetAllSliders(bool? onlyActive = null, bool? showSystemDefined = null)
        {
            return _sliderRepository.Table.Where(e => (!showSystemDefined.HasValue || e.IsSystemDefined == showSystemDefined.Value) && (!onlyActive.HasValue || e.IsActive == onlyActive.Value)).OrderByDescending(a => a.CreatedOn).ToList();
        }

        public IList<Picture> GetAllSliderPicturesBySliderId(int id, bool? onlyActive = null)
        {
            return _sliderRepository.GetByID(id) ?.Pictures.OrderByDescending(a => a.CreatedOn).ToList();
        }

        public Slider GetSliderById(int id)
        {
            if (id == 0)
                throw new Exception("Slider Id Missing");

            return _sliderRepository.GetByID(id);
        }

        public Slider GetSliderByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Slider Name Missing!");

            return _sliderRepository.Table.FirstOrDefault(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public Slider GetSliderByDisplayArea(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var displayAreasList = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(DisplayAreas)))
            {
                var id = (int) item;
                displayAreasList.Add(new SelectListItem()
                {
                    Text = EnumExtensions.GetDescriptionByValue<DisplayAreas>(id),
                    Value = id.ToString()
                });
            }
            return _sliderRepository.Table.FirstOrDefault(x => displayAreasList.Any(y => y.Text.Trim().ToLower() == name.Trim().ToLower() && y.Value == x.DisplayArea.ToString()));
        }

        public Slider GetDefaultSlider()
        {
            return _sliderRepository.Table.FirstOrDefault(x => x.IsSystemDefined);
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

        public virtual void DeleteSliders(IList<Slider> sliders)
        {
            if (sliders == null)
                throw new ArgumentNullException("sliders");

            foreach (var slider in sliders)
            {
                if (!slider.IsSystemDefined)
                    _sliderRepository.Delete(slider.Id);
            }
        }

        public virtual IList<Slider> GetSliderByIds(int[] sliderIds)
        {
            if (sliderIds == null)
                return new List<Slider>();

            if (sliderIds.Length == 0)
                return new List<Slider>();

            var query = from r in _sliderRepository.Table
                        where sliderIds.Contains(r.Id) && r.IsSystemDefined == false
                        select r;

            var sliders = query.ToList();

            var sortedSliders = new List<Slider>();
            foreach (int id in sliderIds)
            {
                var slider = sliders.Find(x => x.Id == id);
                if (slider != null)
                    sortedSliders.Add(slider);
            }
            return sortedSliders;
        }

        public void ToggleActiveStatus(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderRepository.GetByID(id);
            if (slider != null && !slider.IsSystemDefined)
            {
                slider.IsActive = !slider.IsActive;
                _sliderRepository.Update(slider);
            }

        }

        public void ToggleCaptionStatus(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderRepository.GetByID(id);
            if (slider != null && !slider.IsSystemDefined)
            {
                slider.ShowCaption = !slider.ShowCaption;
                _sliderRepository.Update(slider);
            }

        }

        public void ToggleIndicatorStatus(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderRepository.GetByID(id);
            if (slider != null && !slider.IsSystemDefined)
            {
                slider.ShowNextPrevIndicators = !slider.ShowNextPrevIndicators;
                _sliderRepository.Update(slider);
            }

        }

        public void ToggleThumbnailStatus(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var slider = _sliderRepository.GetByID(id);
            if (slider != null && !slider.IsSystemDefined)
            {
                slider.ShowThumbnails = !slider.ShowThumbnails;
                _sliderRepository.Update(slider);
            }

        }


        #endregion
    }
}
