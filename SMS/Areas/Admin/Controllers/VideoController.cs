using System;
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
        // GET: Admin/Picture
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Picture/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Picture/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Picture/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Picture/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Picture/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Picture/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Picture/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AsyncUpload()
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
                string nameAndLocation = "~/Uploads/videos/" + httpPostedFile.FileName;
                httpPostedFile.SaveAs(Server.MapPath(nameAndLocation));
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save("~/Uploads/videos/" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
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

            var picture = new EF.Core.Data.Picture()
            {
                PictureSrc = "~/Uploads/images/" + fileName,
                AlternateText = "",
                DisplayOrder = 0,
                IsActive = true,
                IsLogo = false,
                IsThumb = false,
                Url = "",
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

    }
}
