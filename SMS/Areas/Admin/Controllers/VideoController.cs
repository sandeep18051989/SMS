using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;

namespace SMS.Areas.Admin.Controllers
{
	public class VideoController : AdminAreaController
	{
		#region Members

		public readonly IPictureService _pictureService;
		public readonly IVideoService _videoService;
		public readonly IUserContext _userContext;

		#endregion
		public VideoController(IPictureService pictureService, IUserContext userContext, IVideoService videoService)
		{
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._videoService = videoService;
		}

		Process _ffmpeg;

		[HttpPost]
		public ActionResult AsyncVideoUpload()
		{
			Stream stream = null;
			var fileName = "";
			var contentType = "";
			string result = string.Empty;
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
			httpPostedFile.SaveAs(Server.MapPath(nameAndLocation));

			var video = new EF.Core.Data.Video()
			{
				VideoSrc = savePath,
				DisplayOrder = 0,
				IsActive = true,
				Url = "",
				UserId = _userContext.CurrentUser.Id,
				CreatedOn = DateTime.Now,
				ModifiedOn = DateTime.Now
			};

			_videoService.Insert(video);
			return Json(new
			{
				success = true,
				VideoId = video.Id,
				Picture = _videoService.GetVideoById(video.Id).VideoSrc,
				Default = false,
				DisplayOrder = "",
				StartDate = "",
				EndDate = ""
			}, "text/plain");
		}
	}
}
