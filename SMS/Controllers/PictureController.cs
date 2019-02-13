using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Enums;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class PictureController : PublicHttpController
    {
        #region Members

        public readonly IPictureService _pictureService;
        public readonly IFileService _fileService;
        public readonly IUserContext _userContext;
        public readonly IVideoService _videoService;
        public readonly ISettingService _settingService;
        public readonly IPermissionService _permissionService;
        private readonly IUrlHelper _urlHelper;

        #endregion
        public PictureController(IPictureService pictureService, IUserContext userContext, IFileService fileService, IVideoService videoService, ISettingService settingService, IPermissionService permissionService, IUrlHelper urlHelper)
        {
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._fileService = fileService;
            this._videoService = videoService;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._urlHelper = urlHelper;
        }

        public ActionResult Settings()
        {
            var model = new PictureSettingsModel();
            var user = _userContext.CurrentUser;
            model.ActiveSettings = "PictureSettings";
            var pictureSettings = _settingService.GetSettingsByType(SettingTypeEnum.PictureSetting).ToList();
            if (pictureSettings.Count > 0)
            {
                foreach (var setting in pictureSettings)
                {
                    if (setting.Name == "MaxWidthAllowedForLargeThumbnails")
                    {
                        model.MaxWidthAllowedForLargeThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxWidthAllowedForMediumThumbnails")
                    {
                        model.MaxWidthAllowedForMediumThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxWidthAllowedForSmallThumbnails")
                    {
                        model.MaxWidthAllowedForSmallThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxWidthAllowedForSliderThumbnails")
                    {
                        model.MaxWidthAllowedForSliderThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxHeightAllowedForLargeThumbnails")
                    {
                        model.MaxHeightAllowedForLargeThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxHeightAllowedForMediumThumbnails")
                    {
                        model.MaxHeightAllowedForMediumThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxHeightAllowedForSmallThumbnails")
                    {
                        model.MaxHeightAllowedForSmallThumbnails = setting.Value;
                    }
                    if (setting.Name == "MaxHeightAllowedForSliderThumbnails")
                    {
                        model.MaxHeightAllowedForSliderThumbnails = setting.Value;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(PictureSettingsModel model)
        {
            var user = _userContext.CurrentUser;

            try
            {
                if (ModelState.IsValid)
                {
                    var pictureSettings = _settingService.GetSettingsByType(SettingTypeEnum.PictureSetting).ToList();
                    if (pictureSettings.Count > 0)
                    {
                        foreach (var setting in pictureSettings)
                        {
                            setting.ModifiedOn = DateTime.Now;
                            setting.UserId = user.Id;
                            if (setting.Name == "MaxWidthAllowedForLargeThumbnails")
                            {
                                setting.Value = model.MaxWidthAllowedForLargeThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxWidthAllowedForMediumThumbnails")
                            {
                                setting.Value = model.MaxWidthAllowedForMediumThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxWidthAllowedForSmallThumbnails")
                            {
                                setting.Value = model.MaxWidthAllowedForSmallThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxWidthAllowedForSliderThumbnails")
                            {
                                setting.Value = model.MaxWidthAllowedForSliderThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxHeightAllowedForLargeThumbnails")
                            {
                                setting.Value = model.MaxHeightAllowedForLargeThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxHeightAllowedForMediumThumbnails")
                            {
                                setting.Value = model.MaxHeightAllowedForMediumThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxHeightAllowedForSmallThumbnails")
                            {
                                setting.Value = model.MaxHeightAllowedForSmallThumbnails;
                                _settingService.Update(setting);
                            }
                            if (setting.Name == "MaxHeightAllowedForSliderThumbnails")
                            {
                                setting.Value = model.MaxHeightAllowedForSliderThumbnails;
                                _settingService.Update(setting);
                            }
                        }
                    }
                }
                model.Result = "Picture Settings Saved Successfully.";
                model.ActiveSettings = "PictureSettings";
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult AsyncPictureUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            byte[] bin = null;
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }

            // Create Thumbnails
            var maxWidthSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSmallThumbnails");
            var maxHeightSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSmallThumbnails");
            var sliderImageWidth = maxWidthSetting.Value;
            var sliderImageHeight = maxHeightSetting.Value;
            // ReSharper disable once ComplexConditionExpression
            if (!string.IsNullOrEmpty(sliderImageWidth) && !string.IsNullOrEmpty(sliderImageHeight))
            {
                int newWidth = Convert.ToInt32(sliderImageWidth);
                int newHeight = Convert.ToInt32(sliderImageHeight);
                stream = Request.InputStream;
                fileName = Request["qqfile"];

                // Save File
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile != null)
                    using (System.Drawing.Bitmap postedImage = new System.Drawing.Bitmap(httpPostedFile.InputStream))
                    {
                        bin = _pictureService.scaleImage(postedImage, newWidth, newHeight, false);
                        fileName = "data:image/jpeg;base64," + Convert.ToBase64String(bin);
                    }
            }

            int bytes = Request.Files[0].ContentLength;
            int Gbs = bytes / (1024 * 1024 * 1024);
            int temp = bytes % (1024 * 1024 * 1024);

            int MBs = temp / (1024 * 1024);
            temp = temp % (1024 * 1024);

            int kBs = temp / 1024;

            var picture = new EF.Core.Data.Picture()
            {
                PictureSrc = fileName,
                AlternateText = "",
                DisplayOrder = 0,
                IsActive = true,
                IsLogo = false,
                IsThumb = false,
                Url = fileName,
                IsOpenResource = false,
                Size = MBs,
                UserId = _userContext.CurrentUser != null ? _userContext.CurrentUser.Id : 1,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _pictureService.Insert(picture);

            return Json(new
            {
                success = true,
                PictureId = picture.Id,
                Picture = _pictureService.GetPictureById(picture.Id).PictureSrc,
                Default = false,
                DisplayOrder = "",
                StartDate = "",
                EndDate = ""
            }, "text/plain");
        }

    }
}
