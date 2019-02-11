using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using EF.Core.Data;
using SMS.Models;
using SMS.Mappers;
using System.Text;
using EF.Services.Http;
using EF.Services;

namespace SMS.Areas.Admin.Controllers
{
	public class VideoController : AdminAreaController
	{
		#region Members

		public readonly IVideoService _videoService;
		public readonly IUserContext _userContext;
        private readonly ISMSService _smsService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlHelper _urlHelper;


        #endregion
        public VideoController(IUserContext userContext, IVideoService videoService, ISMSService smsService, IUserService userService, IPermissionService permissionService, IUrlHelper urlHelper)
		{
			this._userContext = userContext;
			this._videoService = videoService;
            this._smsService = smsService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._urlHelper = urlHelper;
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
                var videoData = (from tempvideo in _videoService.GetAllVideos(onlyOpenResource: true) select tempvideo);

                //total number of rows count     
                var lstHoliday = videoData as Video[] ?? videoData.ToArray();
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
                        data = data.Select(x => new VideoModel()
                        {
                            Id = x.Id,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder,
                            IsOpenResource = x.IsOpenResource,
                            Size = x.Size,
                            VideoSrc = x.VideoSrc,
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

        public JsonResult GetVideoResources()
        {
            return new JsonResult()
            {
                Data = _videoService.GetAllVideos(onlyActive: true, onlyOpenResource: true).Select(x => new VideoModel()
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    IsOpenResource = x.IsOpenResource,
                    Size = x.Size,
                    VideoSrc = x.VideoSrc,
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

        [HttpPost]
		public ActionResult AsyncVideoUpload()
		{
			Stream stream = null;
			var fileName = "";
			var contentType = "";
			string result = string.Empty;
            var basePath = _urlHelper.GetLocation(false);
            // IE
            HttpPostedFileBase httpPostedFile = Request.Files[0];
			if (httpPostedFile == null)
				throw new ArgumentException("No file uploaded");

			stream = httpPostedFile.InputStream;
			fileName = Path.GetFileName(httpPostedFile.FileName);
			contentType = httpPostedFile.ContentType;

			var fileExtension = Path.GetExtension(fileName);
			if (!String.IsNullOrEmpty(fileExtension))
				fileExtension = fileExtension.ToLowerInvariant();

			string newFileName = DateTime.Now.ToFileTime().ToString();
			string nameAndLocation = $"\\Uploads\\videos\\{newFileName}{fileExtension}";
			string savePath = $"/Uploads/videos/{newFileName}{fileExtension}";

            if (!Directory.Exists(Server.MapPath($"\\Uploads\\videos\\")))
                Directory.CreateDirectory(Server.MapPath($"\\Uploads\\videos\\"));


            httpPostedFile.SaveAs(Server.MapPath(nameAndLocation));

			var video = new EF.Core.Data.Video()
			{
				VideoSrc = savePath,
				DisplayOrder = 0,
				IsActive = true,
                Url = basePath + $"Uploads/videos/{newFileName}{fileExtension}",
                UserId = _userContext.CurrentUser.Id,
				CreatedOn = DateTime.Now,
				ModifiedOn = DateTime.Now
			};

			_videoService.Insert(video);
			return Json(new
			{
				success = true,
				VideoId = video.Id,
				Video = _videoService.GetVideoById(video.Id).VideoSrc,
				Default = false,
				DisplayOrder = "",
				StartDate = "",
				EndDate = ""
			}, "text/plain");
		}

        [HttpPost]
        public ActionResult AsyncVideoResourceUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            string result = string.Empty;
            var basePath = _urlHelper.GetLocation(false);
            // IE
            HttpPostedFileBase httpPostedFile = Request.Files[0];
            if (httpPostedFile == null)
                throw new ArgumentException("No file uploaded");

            stream = httpPostedFile.InputStream;
            fileName = Path.GetFileName(httpPostedFile.FileName);
            contentType = httpPostedFile.ContentType;

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            string newFileName = DateTime.Now.ToFileTime().ToString();
            string nameAndLocation = $"\\Uploads\\videos\\{newFileName}{fileExtension}";
            string savePath = $"/Uploads/videos/{newFileName}{fileExtension}";

            if (!Directory.Exists(Server.MapPath($"\\Uploads\\videos\\")))
                Directory.CreateDirectory(Server.MapPath($"\\Uploads\\videos\\"));

            httpPostedFile.SaveAs(Server.MapPath(nameAndLocation));

            var video = new Video()
            {
                VideoSrc = savePath,
                DisplayOrder = 0,
                IsActive = true,
                Url = basePath + $"Uploads/videos/{newFileName}{fileExtension}",
                UserId = _userContext.CurrentUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _videoService.Insert(video);
            return Json(new
            {
                success = true,
                VideoId = video.Id,
                Video = _videoService.GetVideoById(video.Id).VideoSrc,
                Default = false,
                DisplayOrder = "",
                StartDate = "",
                EndDate = ""
            }, "text/plain");
        }

        #region Create/Update/Delete Actions

        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            var model = new VideoModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new VideoModel();
            var objVideo = _videoService.GetVideoById(id);
            if (objVideo != null)
            {
                model = objVideo.ToModel();
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(VideoModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            if (ModelState.IsValid)
            {
                var objVideo = _videoService.GetVideoById(model.Id);
                if (objVideo != null)
                {
                    objVideo.ModifiedOn = DateTime.Now;
                    objVideo.IsOpenResource = true;
                    objVideo.DisplayOrder = model.DisplayOrder;
                    objVideo.IsActive = model.IsActive;
                    _videoService.Update(objVideo);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Video updated successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            var model = new VideoModel();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(VideoModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objVideo = _videoService.GetVideoById(model.Id);
                if (objVideo != null)
                {
                    objVideo.ModifiedOn = DateTime.Now;
                    objVideo.IsOpenResource = true;
                    objVideo.DisplayOrder = model.DisplayOrder;
                    objVideo.IsActive = model.IsActive;
                    _videoService.Update(objVideo);
                }
            }
            else
            {
                return View(model);
            }
            SuccessNotification("Video updated successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageVideos"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _videoService.Delete(id);

            SuccessNotification("Video deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _videoService.ToggleActiveStatus(id);
            ViewBag.Result = "Video updated Successfully";

            return Json(new { Result = true });
        }

        #endregion

    }
}
