using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class BlogController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IPermissionService _permissionService;
		private readonly IUrlHelper _urlHelper;
		private readonly IUrlService _urlService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public BlogController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IPermissionService permissionService, IUrlHelper urlHelper, IUrlService urlService, ISMSService smsService)
		{
			_userService = userService;
			_pictureService = pictureService;
			_userContext = userContext;
			_sliderService = sliderService;
			_settingService = settingService;
			_videoService = videoService;
			_commentService = commentService;
			_replyService = replyService;
			_blogService = blogService;
			_permissionService = permissionService;
			_urlHelper = urlHelper;
			_urlService = urlService;
			_smsService = smsService;
		}

		#endregion

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
				var blogData = (from tempblogs in _blogService.GetAllBlogs() select tempblogs);

				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					blogData = blogData.Where(m => m.Name.Contains(searchValue) || m.BlogHtml.Contains(searchValue) || m.Email.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = blogData.Count();
				//Paging     
				var data = blogData.Skip(skip).Take(pageSize).ToList();

				//Returning Json Data 
				return new JsonResult()
				{
					Data = new
					{
						draw = draw,
						recordsFiltered = recordsTotal,
						recordsTotal = recordsTotal,
						data = data.Select(x => new BlogListModel()
						{
							Name = !string.IsNullOrEmpty(x.Name) ? x.Name : "",
							Email = !string.IsNullOrEmpty(x.Email) ? x.Email : "",
							Subject = x.Subject,
							CommentsCount = x.Comments.Count,
							Id = x.Id,
                            AcadmicYearId = x.AcadmicYearId,
							AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
							IsActive = x.IsActive,
							PicturesCount = x.Pictures.Count,
							ReactionsCount = x.Reactions.Count,
							VideosCount = x.Videos.Count,
                            BlogHtml = x.BlogHtml,
                            IpAddress = x.IpAddress,
                            SystemName = x.SystemName
						})
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

		[HttpPost]
		public ActionResult LoadPictureGrid(int id)
		{
			try
			{
				var blogData = (from associatedpicture in _pictureService.GetBlogPictureByBlogId(id) select associatedpicture).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = blogData.Select(x => new PictureListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							IsDefault = x.IsDefault,
							BlogId = id,
							PictureId = x.PictureId,
							PictureSrc = x.Picture?.PictureSrc
						})
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

		public ActionResult LoadVideoGrid(int id)
		{
			try
			{
				var blogData = (from associatedvideo in _videoService.GetBlogVideosByBlogId(id) select associatedvideo);
				return new JsonResult()
				{
					Data = new
					{
						data = blogData.Select(x => new VideoListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							BlogId = id,
							VideoId = x.VideoId,
							VideoSrc = x.Video?.VideoSrc
						})
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

		#endregion

		#region Blog Methods

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var model = new List<BlogModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			var model = new BlogModel();
			if (id == 0)
				throw new Exception("Blog Id Missing");

			var eve = _blogService.GetBlogById(id);
			if (eve != null)
			{
				model = eve.ToModel();
			}

			model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
			{
				Text = x.Name.Trim(),
				Value = x.Id.ToString(),
				Selected = x.IsActive
			}).ToList();
			return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(BlogModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var newBlogPictures = new List<BlogPicture>();
			// Check for duplicate blog, if any
			var _blog = _blogService.GetBlogByName(model.Name);
			if (_blog != null && _blog.Id != model.Id)
				ModelState.AddModelError("Name", "An Blog with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var blogItem = _blogService.GetBlogById(model.Id);

				if (blogItem == null || blogItem.IsDeleted)
					return RedirectToAction("List");

				blogItem = model.ToEntity(blogItem);
				blogItem.ModifiedOn = DateTime.Now;
				blogItem.UserId = user.Id;
				_blogService.Update(blogItem);

				// Save URL Record
				model.SystemName = blogItem.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(blogItem, model.SystemName);
			}
			else
			{
				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				ErrorNotification("An error occured while updating blog. Please try again.");
				return View(model);
			}

			SuccessNotification("Blog updated successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = model.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			var model = new BlogModel();
			model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
			{
				Text = x.Name.Trim(),
				Value = x.Id.ToString(),
				Selected = x.IsActive
			}).ToList();
			return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(BlogModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			var currentUser = _userContext.CurrentUser;
			// Check for duplicate blog, if any
			var newBlog = new Blog();
			var _blog = _blogService.GetBlogByName(model.Name);
			if (_blog != null)
				ModelState.AddModelError("Name", "An Blog with the same name already exists. Please choose a different name.");

			model.UserId = currentUser.Id;
			if (ModelState.IsValid)
			{
				newBlog = model.ToEntity();
				newBlog.CreatedOn = newBlog.ModifiedOn = DateTime.Now;

				_blogService.Insert(newBlog);

				// Save URL Record
				model.SystemName = newBlog.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(newBlog, model.SystemName);
			}
			else
			{
				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				ErrorNotification("An error occured while creating blog. Please try again.");
				return View(model);
			}

			SuccessNotification("Blog created successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = newBlog.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			_blogService.Delete(id);

			SuccessNotification("Blog deleted successfully");
			return RedirectToAction("List");
		}

		#endregion

		#region Blog Picture/Video

		[HttpPost]
		public ActionResult DeleteBlogPicture(int id)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Picture id not found");

			var pictureRecord = _pictureService.GetBlogPictureByPictureId(id);
			if (pictureRecord != null)
			{
				_pictureService.DeleteBlogPicture(pictureRecord.Id);
			}
			else
			{
				var picture = _pictureService.GetPictureById(id);
				if (picture != null)
					_pictureService.Delete(picture.Id);
			}

			SuccessNotification("Blog picture deleted successfully");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		[HttpPost]
		public ActionResult DeleteBlogVideo(int id)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Video id not found");

			var videoRecord = _videoService.GetBlogVideoByVideoId(id);
			if (videoRecord != null)
			{
				_videoService.DeleteBlogVideo(videoRecord.Id);
			}
			else
			{
				var video = _videoService.GetVideoById(id);
				if (video != null)
					_pictureService.Delete(video.Id);
			}

			SuccessNotification("Blog video deleted successfully");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult BlogPictureAdd(int pictureId, int displayOrder, bool isDefault, int blogId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisblog = _blogService.GetBlogById(blogId);
			if (thisblog == null)
				throw new ArgumentException("No blog found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var blogPictureData = new EF.Core.Data.BlogPicture();
			blogPictureData.PictureId = pictureId;
			blogPictureData.BlogId = blogId;
			blogPictureData.DisplayOrder = displayOrder;
			blogPictureData.IsDefault = isDefault;
			blogPictureData.UserId = _userContext.CurrentUser.Id;
			blogPictureData.CreatedOn = blogPictureData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				blogPictureData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				blogPictureData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_pictureService.InsertBlogPicture(blogPictureData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateBlogPicture(int blogId, int pictureId, int displayOrder, bool isDefault, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisblog = _blogService.GetBlogById(blogId);
			if (thisblog == null)
				throw new ArgumentException("No blog found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var blogPictureData = _pictureService.GetBlogPictureByPictureId(pictureId);
			if (blogPictureData != null)
			{
				blogPictureData.DisplayOrder = displayOrder;
				blogPictureData.IsDefault = isDefault;
				blogPictureData.ModifiedOn = DateTime.Now;

				if (blogPictureData.IsDefault)
				{
					var blogDefaultPicture = _pictureService.GetDefaultBlogPicture(blogId);
					if (blogDefaultPicture != null && blogDefaultPicture.PictureId != blogPictureData.PictureId)
					{
						blogDefaultPicture.IsDefault = false;
						_pictureService.UpdateBlogPicture(blogDefaultPicture);
					}
				}

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				blogPictureData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				blogPictureData.EndDate = picEndDate;

				_pictureService.UpdateBlogPicture(blogPictureData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult BlogVideoAdd(int videoId, int displayOrder, int blogId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisblog = _blogService.GetBlogById(blogId);
			if (thisblog == null)
				throw new ArgumentException("No blog found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var blogVideoData = new EF.Core.Data.BlogVideo();
			blogVideoData.VideoId = videoId;
			blogVideoData.BlogId = blogId;
			blogVideoData.DisplayOrder = displayOrder;
			blogVideoData.UserId = _userContext.CurrentUser.Id;
			blogVideoData.CreatedOn = blogVideoData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				blogVideoData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				blogVideoData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_videoService.InsertBlogVideo(blogVideoData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetBlogPictures(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstBlogPictures = new List<BlogPictureModel>();
			var result = _pictureService.GetBlogPictureByBlogId(id);
			foreach (var x in result)
			{
				var blogpic = x.ToModel();
				blogpic.PicStartDate = x.StartDate;
				blogpic.PicEndDate = x.EndDate;

				if (x.PictureId > 0)
				{
					var picture = _pictureService.GetPictureById(x.PictureId);
					blogpic.Picture = new PictureModel()
					{
						AlternateText = picture.AlternateText,
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
						Src = picture.PictureSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstBlogPictures.Add(blogpic);
			}
			return Json(lstBlogPictures, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetBlogVideos(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstBlogVideos = new List<BlogVideoModel>();
			var result = _videoService.GetBlogVideosByBlogId(id);
			foreach (var x in result)
			{
				var blogpic = x.ToModel();
				blogpic.VidStartDate = x.StartDate;
				blogpic.VidEndDate = x.EndDate;

				if (x.VideoId > 0)
				{
					var picture = _videoService.GetVideoById(x.VideoId);
					blogpic.Video = new VideoModel()
					{
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
						VideoSrc = picture.VideoSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstBlogVideos.Add(blogpic);
			}
			return Json(lstBlogVideos, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateBlogVideo(int blogId, int videoId, int displayOrder, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageBlogs"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisblog = _blogService.GetBlogById(blogId);
			if (thisblog == null)
				throw new ArgumentException("No blog found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var blogVideoData = _videoService.GetBlogVideoByVideoId(videoId);
			if (blogVideoData != null)
			{
				blogVideoData.DisplayOrder = displayOrder;
				blogVideoData.ModifiedOn = DateTime.Now;

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				blogVideoData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				blogVideoData.EndDate = picEndDate;

				_videoService.UpdateBlogVideo(blogVideoData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}
