using System;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class SliderController : PublicHttpController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;

        #endregion Fileds

        #region Constructor

        public SliderController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
        }

        #endregion

        // GET: Common
        [ChildActionOnly]
        public ActionResult Index()
        {
            var model = new SliderModel();
            var activeSlider = _sliderService.GetSlider(true);
            if (activeSlider != null)
            {
                model.SliderId = activeSlider.Id;

                var sliderSettings = _settingService.GetSettingsByEntityId(activeSlider.Id).ToList();
                if (sliderSettings.Count > 0)
                {
                    foreach (var setting in sliderSettings)
                    {
                        if (setting.Name == "MaxPictures" && setting.EntityId == activeSlider.Id)
                        {
                            model.MaxPictures = Convert.ToInt32(setting.Value);
                        }
                        if (setting.Name == "SliderCaptionOff" && setting.EntityId == activeSlider.Id)
                        {
                            model.CaptionOff = setting.Value == "True" ? true : false;
                        }
                    }
                }

                if (activeSlider.Pictures.Count > 0)
                {
                    foreach (var picture in activeSlider.Pictures.Where(x => x.IsActive == true))
                    {
                        model.Pictures.Add(new PictureModel()
                        {
                            AlternateText = picture.AlternateText,
                            IsActive = picture.IsActive,
                            Id = picture.Id,
                            Src = picture.PictureSrc,
                            Size = picture.Size,
                            Url = picture.Url,
                            CaptionOff = model.CaptionOff,
                            Width = picture.Width,
                            Height = picture.Height
                        });
                    }
                }

                // Limit Pictures According to Setting
                if (model.Pictures.Count > model.MaxPictures)
                    model.Pictures = model.Pictures.Take(model.MaxPictures).ToList();

            }
            return PartialView("~/Views/Slider/Index.cshtml", model);
        }

        // To render user links
        [ChildActionOnly]
        public ActionResult UserLinks()
        {
            var user = _userContext.CurrentUser;

            if (user == null)
            {
                var model = new UserLinksModel
                {
                    IsAuthenticated = false
                    //Links = "<ul class='nav nav-pills'><li><a href='#'><i class='fa fa-icon-cog'></i>Login</a></li></ul>"
                };

                return PartialView(model);
            }
            else
            {
                var model = new UserLinksModel
                {
                    IsAuthenticated = true,
                    user = user
                    //Links = "<ul class='nav nav-pills'><li>Welcome <a href='#'><i class='fa fa-icon-user'></i>" + user.UserName + "</a></li></ul>"
                };

                return PartialView(model);
            }
        }
    }
}
