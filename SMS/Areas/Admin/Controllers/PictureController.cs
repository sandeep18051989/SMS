using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;
using SMS.Mappers;
using EF.Services;
using EF.Core.Data;
using System.Text;

namespace SMS.Areas.Admin.Controllers
{
    public class PictureController : AdminAreaController
    {
        #region Members

        private readonly IPictureService _pictureService;
        private readonly IFileService _fileService;
        private readonly IUserContext _userContext;
        private readonly IVideoService _videoService;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlHelper _urlHelper;
        private readonly ISMSService _smsService;
        private readonly IUserService _userService;

        #endregion
        public PictureController(IPictureService pictureService, IUserContext userContext, IFileService fileService, IVideoService videoService, ISettingService settingService, IPermissionService permissionService, IUrlHelper urlHelper, ISMSService smsService, IUserService userService)
        {
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._fileService = fileService;
            this._videoService = videoService;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._urlHelper = urlHelper;
            this._smsService = smsService;
            this._userService = userService;
        }

        #region Utilities

        public ActionResult LoadGrid()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]")?.FirstOrDefault();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all data    
                var pictureData = (from temppicture in _pictureService.GetAllPictures(onlyOpenResource: true) select temppicture);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    pictureData = pictureData.Where(m => m.AlternateText.Contains(searchValue));
                }

                //total number of rows count     
                var lstHoliday = pictureData as Picture[] ?? pictureData.ToArray();
                recordsTotal = lstHoliday.Count();
                //Paging     
                var data = lstHoliday.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new PictureModel()
                        {
                            AcadmicYearId = x.AcadmicYearId,
                            Id = x.Id,
                            IsActive = x.IsActive,
                            CreatedDateString = x.CreatedOn.Value.ToString("U"),
                            AlternateText = x.AlternateText,
                            DisplayOrder = x.DisplayOrder,
                            Height = x.Height,
                            IsLogo = x.IsLogo,
                            IsOpenResource = x.IsOpenResource,
                            IsThumb = x.IsThumb,
                            Size = x.Size,
                            PictureSrc = x.PictureSrc,
                            UploadedBy = x.UserId > 0 ? _userService.GetUserById(x.UserId).UserName : "Unknown",
                            Url = x.Url
                        }).OrderBy(x => x.DisplayOrder).ToList()
                    },
                    ContentEncoding = Encoding.Default,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public JsonResult GetPictureResources()
        {
            return new JsonResult()
            {
                Data = _pictureService.GetAllPictures(onlyActive: true, onlyOpenResource: true).Select(x => new PictureModel()
                {
                    AcadmicYearId = x.AcadmicYearId,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    CreatedDateString = x.CreatedOn.Value.ToString("U"),
                    AlternateText = x.AlternateText,
                    DisplayOrder = x.DisplayOrder,
                    Height = x.Height,
                    IsLogo = x.IsLogo,
                    IsOpenResource = x.IsOpenResource,
                    IsThumb = x.IsThumb,
                    Size = x.Size,
                    PictureSrc = x.PictureSrc,
                    UploadedBy = x.UserId > 0 ? _userService.GetUserById(x.UserId).UserName : "Unknown",
                    Url = x.Url
                }).OrderBy(x => x.DisplayOrder).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion

        public ActionResult Settings()
        {
            if (!_permissionService.Authorize("ManageSettings"))
                return AccessDeniedView();

            var model = new PictureSettingsModel();
            var user = _userContext.CurrentUser;
            model.ActiveSettings = "PictureSettings";

            var maxWidthAllowedForLargeThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForLargeThumbnails");
            if (maxWidthAllowedForLargeThumbnailsSetting != null)
            {
                model.MaxWidthAllowedForLargeThumbnails = maxWidthAllowedForLargeThumbnailsSetting.Value;
            }

            var maxWidthAllowedForMediumThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForMediumThumbnails");
            if (maxWidthAllowedForMediumThumbnailsSetting != null)
            {
                model.MaxWidthAllowedForMediumThumbnails = maxWidthAllowedForMediumThumbnailsSetting.Value;
            }

            var maxWidthAllowedForSmallThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSmallThumbnails");
            if (maxWidthAllowedForSmallThumbnailsSetting != null)
            {
                model.MaxWidthAllowedForSmallThumbnails = maxWidthAllowedForSmallThumbnailsSetting.Value;
            }

            var maxWidthAllowedForSliderThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSliderThumbnails");
            if (maxWidthAllowedForSliderThumbnailsSetting != null)
            {
                model.MaxWidthAllowedForSliderThumbnails = maxWidthAllowedForSliderThumbnailsSetting.Value;
            }

            var maxHieghtAllowedForLargeThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForLargeThumbnails");
            if (maxHieghtAllowedForLargeThumbnailsSetting != null)
            {
                model.MaxHeightAllowedForLargeThumbnails = maxHieghtAllowedForLargeThumbnailsSetting.Value;
            }

            var maxHieghtAllowedForMediumThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForMediumThumbnails");
            if (maxHieghtAllowedForMediumThumbnailsSetting != null)
            {
                model.MaxHeightAllowedForMediumThumbnails = maxHieghtAllowedForMediumThumbnailsSetting.Value;
            }

            var maxHieghtAllowedForSmallThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSmallThumbnails");
            if (maxHieghtAllowedForSmallThumbnailsSetting != null)
            {
                model.MaxHeightAllowedForSmallThumbnails = maxHieghtAllowedForSmallThumbnailsSetting.Value;
            }

            var maxHieghtAllowedForSliderThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSliderThumbnails");
            if (maxHieghtAllowedForSliderThumbnailsSetting != null)
            {
                model.MaxHeightAllowedForSliderThumbnails = maxHieghtAllowedForSliderThumbnailsSetting.Value;
            }

            var pictureTypesAllowedSetting = _settingService.GetSettingByKey("PictureTypesAllowed");
            if (pictureTypesAllowedSetting != null)
            {
                pictureTypesAllowedSetting.Value.TrimEnd(',');
                model.SelectedPictureTypes = pictureTypesAllowedSetting.Value.Split(',');
            }

            var maxSizeAllowedSetting = _settingService.GetSettingByKey("MaximumSizeAllowed");
            if (maxSizeAllowedSetting != null)
            {
                model.MaximumSizeAllowed = Convert.ToInt32(maxSizeAllowedSetting.Value);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(PictureSettingsModel model)
        {
            if (!_permissionService.Authorize("ManageSettings"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;

            try
            {
                if (ModelState.IsValid)
                {
                    var maxWidthAllowedForLargeThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForLargeThumbnails");
                    if (maxWidthAllowedForLargeThumbnailsSetting != null)
                    {
                        maxWidthAllowedForLargeThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxWidthAllowedForLargeThumbnailsSetting.Value = model.MaxWidthAllowedForLargeThumbnails;
                        _settingService.Update(maxWidthAllowedForLargeThumbnailsSetting);
                    }

                    var maxWidthAllowedForMediumThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForMediumThumbnails");
                    if (maxWidthAllowedForMediumThumbnailsSetting != null)
                    {
                        maxWidthAllowedForMediumThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxWidthAllowedForMediumThumbnailsSetting.Value = model.MaxWidthAllowedForMediumThumbnails;
                        _settingService.Update(maxWidthAllowedForMediumThumbnailsSetting);
                    }

                    var maxWidthAllowedForSmallThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSmallThumbnails");
                    if (maxWidthAllowedForSmallThumbnailsSetting != null)
                    {
                        maxWidthAllowedForSmallThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxWidthAllowedForSmallThumbnailsSetting.Value = model.MaxWidthAllowedForSmallThumbnails;
                        _settingService.Update(maxWidthAllowedForSmallThumbnailsSetting);
                    }

                    var maxWidthAllowedForSliderThumbnailsSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSliderThumbnails");
                    if (maxWidthAllowedForSliderThumbnailsSetting != null)
                    {
                        maxWidthAllowedForSliderThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxWidthAllowedForSliderThumbnailsSetting.Value = model.MaxWidthAllowedForSliderThumbnails;
                        _settingService.Update(maxWidthAllowedForSliderThumbnailsSetting);
                    }

                    var maxHieghtAllowedForLargeThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForLargeThumbnails");
                    if (maxHieghtAllowedForLargeThumbnailsSetting != null)
                    {
                        maxHieghtAllowedForLargeThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxHieghtAllowedForLargeThumbnailsSetting.Value = model.MaxHeightAllowedForLargeThumbnails;
                        _settingService.Update(maxHieghtAllowedForLargeThumbnailsSetting);
                    }

                    var maxHieghtAllowedForMediumThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForMediumThumbnails");
                    if (maxHieghtAllowedForMediumThumbnailsSetting != null)
                    {
                        maxHieghtAllowedForMediumThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxHieghtAllowedForMediumThumbnailsSetting.Value = model.MaxHeightAllowedForMediumThumbnails;
                        _settingService.Update(maxHieghtAllowedForMediumThumbnailsSetting);
                    }

                    var maxHieghtAllowedForSmallThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSmallThumbnails");
                    if (maxHieghtAllowedForSmallThumbnailsSetting != null)
                    {
                        maxHieghtAllowedForSmallThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxHieghtAllowedForSmallThumbnailsSetting.Value = model.MaxHeightAllowedForSmallThumbnails;
                        _settingService.Update(maxHieghtAllowedForSmallThumbnailsSetting);
                    }

                    var maxHieghtAllowedForSliderThumbnailsSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSliderThumbnails");
                    if (maxHieghtAllowedForSliderThumbnailsSetting != null)
                    {
                        maxHieghtAllowedForSliderThumbnailsSetting.ModifiedOn = DateTime.Now;
                        maxHieghtAllowedForSliderThumbnailsSetting.Value = model.MaxHeightAllowedForSliderThumbnails;
                        _settingService.Update(maxHieghtAllowedForSliderThumbnailsSetting);
                    }

                    var pictureTypesAllowedSetting = _settingService.GetSettingByKey("PictureTypesAllowed");
                    if (pictureTypesAllowedSetting != null)
                    {
                        pictureTypesAllowedSetting.ModifiedOn = DateTime.Now;
                        pictureTypesAllowedSetting.Value = string.Join(",", model.SelectedPictureTypes);
                        _settingService.Update(pictureTypesAllowedSetting);
                    }

                    var maxSizeAllowedSetting = _settingService.GetSettingByKey("MaximumSizeAllowed");
                    if (maxSizeAllowedSetting != null)
                    {
                        maxSizeAllowedSetting.Value = model.MaximumSizeAllowed.ToString();
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
        public ActionResult AsyncNewsPictureUpload()
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
            var maxWidthSetting = _settingService.GetSettingByKey("MaxWidthAllowedForMediumThumbnails");
            var maxHeightSetting = _settingService.GetSettingByKey("MaxHeightAllowedForMediumThumbnails");

            var sliderImageWidth = maxWidthSetting.Value;
            var sliderImageHeight = maxHeightSetting.Value;
            if (!string.IsNullOrEmpty(sliderImageWidth) && !string.IsNullOrEmpty(sliderImageHeight))
            {
                int newWidth = Convert.ToInt32(sliderImageWidth);
                int newHeight = Convert.ToInt32(sliderImageHeight);
                stream = Request.InputStream;
                fileName = Request["qqfile"];

                // Save File
                HttpPostedFileBase httpPostedFile = Request.Files[0];
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

            int KBs = temp / 1024;
            temp = temp % 1024;

            var picture = new EF.Core.Data.Picture()
            {
                PictureSrc = fileName,
                AlternateText = "",
                DisplayOrder = 0,
                IsActive = true,
                IsLogo = false,
                IsThumb = false,
                Size = MBs,
                Url = fileName,
                IsOpenResource = false,
                UserId = _userContext.CurrentUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _pictureService.Insert(picture);
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureById(picture.Id).PictureSrc
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult AsyncSliderPictureUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            byte[] bin = null;
            int imgHeight = 0;
            int imgWidth = 0;
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

            #region Content Type
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
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }
            #endregion

            // Create Thumbnails
            var maxWidthSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSliderThumbnails");
            var maxHeightSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSliderThumbnails");
            var sliderImageWidth = maxWidthSetting.Value;
            var sliderImageHeight = maxHeightSetting.Value;
            if (!string.IsNullOrEmpty(sliderImageWidth) && !string.IsNullOrEmpty(sliderImageHeight))
            {
                int newWidth = imgWidth = Convert.ToInt32(sliderImageWidth);
                int newHeight = imgHeight = Convert.ToInt32(sliderImageHeight);
                stream = Request.InputStream;
                fileName = Request["qqfile"];

                // Save File
                HttpPostedFileBase httpPostedFile = Request.Files[0];
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

            int KBs = temp / 1024;
            temp = temp % 1024;

            var picture = new EF.Core.Data.Picture()
            {
                PictureSrc = fileName,
                AlternateText = "",
                DisplayOrder = 0,
                IsActive = true,
                IsLogo = false,
                IsThumb = false,
                IsOpenResource = false,
                Size = MBs,
                Url = fileName,// _urlHelper.GetLocation(false) + thumbPath,
                UserId = _userContext.CurrentUser.Id,
                CreatedOn = DateTime.Now,
                Height = imgHeight,
                Width = imgWidth,
                ModifiedOn = DateTime.Now
            };

            _pictureService.Insert(picture);

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureById(picture.Id).PictureSrc
            },
                 "text/plain");
        }

        [HttpPost]
        public ActionResult AsyncProductPictureUpload()
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
            var maxWidthSetting = _settingService.GetSettingByKey("MaxWidthAllowedForSliderThumbnails");
            var maxHeightSetting = _settingService.GetSettingByKey("MaxHeightAllowedForSliderThumbnails");
            var sliderImageWidth = maxWidthSetting.Value;
            var sliderImageHeight = maxHeightSetting.Value;
            if (!string.IsNullOrEmpty(sliderImageWidth) && !string.IsNullOrEmpty(sliderImageHeight))
            {
                int newWidth = Convert.ToInt32(sliderImageWidth);
                int newHeight = Convert.ToInt32(sliderImageHeight);
                stream = Request.InputStream;
                fileName = Request["qqfile"];

                // Save File
                HttpPostedFileBase httpPostedFile = Request.Files[0];
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

            int KBs = temp / 1024;
            temp = temp % 1024;

            var picture = new EF.Core.Data.Picture()
            {
                PictureSrc = fileName,
                AlternateText = "",
                DisplayOrder = 0,
                IsActive = true,
                IsLogo = false,
                IsThumb = false,
                Url = fileName,
                Size = MBs,
                IsOpenResource = false,
                UserId = _userContext.CurrentUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _pictureService.Insert(picture);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureById(picture.Id).PictureSrc
            },
                 "text/plain");
        }

        [HttpPost]
        public ActionResult DeletePicture(int pictureid)
        {
            if (pictureid == 0)
                throw new Exception("Picture id missing.");

            var picture = _pictureService.GetPictureById(pictureid);

            if (picture != null)
                _pictureService.Delete(pictureid);

            return Json(new
            {
                success = true,
            }, "text/plain");
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
                UserId = _userContext.CurrentUser.Id,
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

        [HttpPost]
        public ActionResult AsyncPictureResourceUpload()
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
                IsOpenResource = true,
                Size = MBs,
                UserId = _userContext.CurrentUser.Id,
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

        [HttpPost]
        public ActionResult AsyncFileUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
                string nameAndLocation = "~/Uploads/files/" + httpPostedFile.FileName;
                httpPostedFile.SaveAs(Server.MapPath(nameAndLocation));
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save(_urlHelper.GetLocation(false) + "Uploads/files/" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            //var fileBinary = new byte[stream.Length];
            //stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    case ".doc":
                        contentType = "application/msword";
                        break;
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".ppt":
                        contentType = "application/vnd.ms-powerpointtd";
                        break;
                    case ".xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".txt":
                        contentType = "text";
                        break;
                    default:
                        break;
                }
            }

            int bytes = Request.Files[0].ContentLength;
            int Gbs = bytes / (1024 * 1024 * 1024);
            int temp = bytes % (1024 * 1024 * 1024);

            int MBs = temp / (1024 * 1024);
            temp = temp % (1024 * 1024);

            int KBs = temp / 1024;
            temp = temp % 1024;

            var file = new EF.Core.Data.File()
            {
                Src = _urlHelper.GetLocation(false) + "Uploads/files/" + fileName,
                Title = "",
                Type = contentType,
                UserId = _userContext.CurrentUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Size = MBs,
            };

            _fileService.Insert(file);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                fileId = file.Id,
                fileUrl = _fileService.GetFileById(file.Id).Src
            },
                 "text/plain");
        }

        #region Create/Update/Delete Actions

        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            var model = new PictureModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new PictureModel();
            var objPicture = _pictureService.GetPictureById(id);
            if (objPicture != null)
            {
                model = objPicture.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(PictureModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            if (ModelState.IsValid && model.Id > 0)
            {
                var objPicture = _pictureService.GetPictureById(model.Id);
                objPicture.ModifiedOn = DateTime.Now;
                objPicture.IsActive = model.IsActive;
                objPicture.IsOpenResource = true;
                objPicture.AcadmicYearId = model.AcadmicYearId;
                objPicture.DisplayOrder = model.DisplayOrder;
                objPicture.AlternateText = model.AlternateText;
                _pictureService.Update(objPicture);
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId == x.Id
                }).ToList();
                return View(model);
            }

            SuccessNotification("Picture Resource Updated Successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            var model = new PictureModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(PictureModel model)
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            if (ModelState.IsValid && model.Id > 0)
            {
                var objPicture = _pictureService.GetPictureById(model.Id);
                objPicture.ModifiedOn = DateTime.Now;
                objPicture.IsActive = model.IsActive;
                objPicture.IsOpenResource = true;
                objPicture.AcadmicYearId = model.AcadmicYearId;
                objPicture.DisplayOrder = model.DisplayOrder;
                objPicture.AlternateText = model.AlternateText;
                _pictureService.Update(objPicture);
                SuccessNotification("Picture Resource Added Successfully.");
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId == x.Id
                }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManagePictures"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _pictureService.Delete(id);

            SuccessNotification("Picture deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _pictureService.ToggleActiveStatusPicture(id);
            ViewBag.Result = "Picture updated Successfully";

            return Json(new { Result = true });
        }

        #endregion

    }
}
