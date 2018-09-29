using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class SliderController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		public readonly ISettingService _settingService;
		private readonly IPermissionService _permissionService;

		#endregion Fileds

		#region Constructor

		public SliderController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IPermissionService permissionService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._permissionService = permissionService;
		}

		#endregion

		public ActionResult Index()
		{
			if (!_permissionService.Authorize("ManageConfiguration"))
				return AccessDeniedView();

			var model = new SliderSettingsModel();
			model.ActiveSettings = "SliderSettings";
			var activeSlider = _sliderService.GetSlider(true);
			var MaxWidthAllowedForSliderThumbnails = _settingService.GetSettingByKey("MaxWidthAllowedForSliderThumbnails");
			if (MaxWidthAllowedForSliderThumbnails != null)
			{
				model.MaxWidthSetting = MaxWidthAllowedForSliderThumbnails.Value;
			}

			var MaxHeightAllowedForSliderThumbnails = _settingService.GetSettingByKey("MaxHeightAllowedForSliderThumbnails");
			if (MaxHeightAllowedForSliderThumbnails != null)
			{
				model.MaxHeightSetting = MaxHeightAllowedForSliderThumbnails.Value;
			}

			model.Slider = activeSlider;
			model.Id = activeSlider.Id;
			var maxPictureSetting = _settingService.GetSettingByKey("MaxPictures");
			if (maxPictureSetting != null)
			{
				model.MaxPictures = maxPictureSetting.Value;
			}
			if (activeSlider.Pictures.Count > 0)
			{
				model.Pictures = activeSlider.Pictures.Select(item => new PictureModel
				{
					Id = item.Id,
					AlternateText = item.AlternateText,
					Height = item.Height,
					IsActive = item.IsActive,
					IsLogo = item.IsLogo,
					Src = item.PictureSrc,
					Url = item.Url,
					UserId = item.UserId
				}).ToList();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(SliderSettingsModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageConfiguration"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;

			if (ModelState.IsValid)
			{
				var activeSlider = _sliderService.GetSlider(true);
				if (activeSlider != null)
				{
					model.Slider = activeSlider;
					var sliderSettings = _settingService.GetSettingsByEntityId(activeSlider.Id).ToList();
					var maxPictureSetting = _settingService.GetSettingByKey("MaxPictures");
					if (maxPictureSetting != null)
					{
						maxPictureSetting.Value = model.MaxPictures;
						maxPictureSetting.ModifiedOn = DateTime.Now;
						_settingService.Update(maxPictureSetting);
					}

					if (frm.Keys.Count > 0)
					{
						var pictureids = frm["pictureids"]?.ToString().Split(',');
						if (pictureids?.Length > 0)
						{
							foreach (var picid in pictureids)
							{
								int pictureid = Convert.ToInt32(picid);
								var picture = _pictureService.GetPictureById(pictureid);
								if (picture != null)
								{
									activeSlider.Pictures.Add(picture);
								}
							}
						}
					}

					SuccessNotification("Slider settings saved successfully.");
					model.ActiveSettings = "SliderSettings";
					return View(model);
				}
			}
			model.ActiveSettings = "SliderSettings";
			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult UpdatePicture(int pictureId, string alternateText, string displayOrder, bool active)
		{
			if (pictureId > 0)
			{
				var sliderSettings = _settingService.GetSettingsByType(SettingTypeEnum.SliderSetting).ToList();
				var picture = _pictureService.GetPictureById(pictureId);
				if (picture != null)
				{
					picture.AlternateText = alternateText;
					picture.DisplayOrder = Convert.ToInt32(displayOrder);
					picture.IsActive = active;
					_pictureService.Update(picture);
				}
				else
				{
					return Json(new { Success = "False", Message = "Picture Id Does Not Exists In The Database" }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { Success = "False", Message = "Picture Id Not Found!" }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { Success = "True", Message = "Picture Updated Succesfully" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult DeletePicture(int pictureId)
		{
			var user = _userContext.CurrentUser;
			if (pictureId > 0)
			{
				if (user != null)
				{
					var activeSlider = _sliderService.GetSlider(true);
					if (activeSlider != null)
					{
						var sliderSettings = _settingService.GetSettingsByType(SettingTypeEnum.SliderSetting).ToList();
						var _picture = _pictureService.GetPictureById(pictureId);
						if (_picture != null)
						{
							activeSlider.Pictures.Remove(_picture);
							_sliderService.Update(activeSlider);
						}
						else
						{
							return Json(new { Success = "False", Message = "Picture Id Does Not Exists In The Database" }, JsonRequestBehavior.AllowGet);
						}
					}
				}
				else
				{
					return Json(new { Success = "False", Message = "Access Denied!" });
				}
			}
			else
			{
				return Json(new { Success = "False", Message = "Picture Id Not Found!" });
			}

			return Json(new { Success = "True", Message = "Picture Deleted Succesfully" });
		}

		public PartialViewResult GetSliderPicturesList()
		{
			var model = new List<PictureModel>();
			var user = _userContext.CurrentUser;
			if (user != null)
			{
				var activeSlider = _sliderService.GetSlider(true);
				if (activeSlider != null)
				{
					model = activeSlider.Pictures.Select(item => new PictureModel
					{
						AlternateText = item.AlternateText,
						Height = item.Height,
						IsActive = item.IsActive,
						IsLogo = item.IsLogo,
						Src = item.PictureSrc,
						Url = item.Url,
						UserId = item.UserId,
						Id = item.Id
					}).ToList();
				}
			}

			return PartialView("_PicturesList", model);
		}

		public PartialViewResult GetPicturesList()
		{
			var pictureList = _pictureService.GetAllPictures().ToList();
			var model = pictureList.Select(item => new PictureModel
			{
				AlternateText = item.AlternateText,
				Height = item.Height,
				IsActive = item.IsActive,
				IsLogo = item.IsLogo,
				Src = item.PictureSrc,
				Url = item.Url,
				UserId = item.UserId,
				Id = item.Id
			}).ToList();
			return PartialView("_PicturesList", model);
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

		[HttpPost]
		public ActionResult DeleteSelected(int sliderId, ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageSlider"))
				return AccessDeniedView();

			if (sliderId == 0)
				throw new Exception("Slider ID Missing");

			var _slider = _sliderService.GetSlider(sliderId);

			if (_slider != null)
				_sliderService.DeleteSliderPictures(_slider, _sliderService.GetPicturesByIds(selectedIds.ToArray()).ToList());

			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult ToggleSelected(int sliderId, ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageSlider"))
				return AccessDeniedView();

			if (sliderId == 0)
				throw new Exception("Slider ID Missing");

			var _slider = _sliderService.GetSlider(sliderId);

			if (_slider != null)
				_sliderService.TogglePictures(_slider, _sliderService.GetPicturesByIds(selectedIds.ToArray()).ToList());

			return Json(new { Result = true });
		}
	}
}
