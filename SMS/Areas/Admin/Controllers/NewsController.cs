using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class NewsController : AdminAreaController
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
		private readonly INewsService _newsService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public NewsController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IPermissionService permissionService, IUrlHelper urlHelper, IUrlService urlService, ISMSService smsService, INewsService newsService)
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
			_newsService = newsService;
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
				var newsData = (from tempnewss in _newsService.GetAllNews() select tempnewss);

				//Sorting    
				//if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
				//{
				//	newsData = newsData.AsEnumerable().OrderBy($"{sortColumn} {sortColumnDir}");
				//}
				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					newsData = newsData.Where(m => m.ShortName.Contains(searchValue) || m.Description.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = newsData.Count();
				//Paging     
				var data = newsData.Skip(skip).Take(pageSize).ToList();

				//Returning Json Data 
				return new JsonResult()
				{
					Data = new
					{
						draw = draw,
						recordsFiltered = recordsTotal,
						recordsTotal = recordsTotal,
						data = data.Select(x => new NewsListModel()
						{
							Description = !string.IsNullOrEmpty(x.Description) ? x.Description : "",
							ShortName = !string.IsNullOrEmpty(x.ShortName) ? x.ShortName : "",
							CommentsCount = x.Comments.Count,
							Author = x.Author.Trim(),
							Id = x.Id,
							AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
							EndDate = x.EndDate.ToString("d MMM yyyy"),
							IsActive = x.IsActive,
							PicturesCount = x.Pictures.Count,
							ReactionsCount = x.Reactions.Count,
							StartDate = x.StartDate.ToString("d MMM yyyy"),
							VideosCount = x.Videos.Count
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
				var newsData = (from associatedpicture in _pictureService.GetNewsPictureByNewsId(id) select associatedpicture).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = newsData.Select(x => new PictureListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							IsDefault = x.IsDefault,
							NewsId = id,
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
				var newsData = (from associatedvideo in _videoService.GetNewsVideosByNewsId(id) select associatedvideo);
				return new JsonResult()
				{
					Data = new
					{
						data = newsData.Select(x => new VideoListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							NewsId = id,
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

		#region News Methods

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var model = new List<NewsModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			var model = new NewsModel();
			if (id == 0)
				throw new Exception("News Id Missing");

			var eve = _newsService.GetNewsById(id);
			if (eve != null)
			{
				model = eve.ToModel();
			}

			model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
												select new SelectListItem
												{
													Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
													Selected = (Convert.ToInt32(d) == model.NewsStatusId)
												}).ToList();

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
		public ActionResult Edit(NewsModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var newNewsPictures = new List<NewsPicture>();
			// Check for duplicate news, if any
			var _news = _newsService.GetNewsByShortName(model.ShortName);
			if (_news != null && _news.Id != model.Id)
				ModelState.AddModelError("ShortName", "An News with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var newsItem = _newsService.GetNewsById(model.Id);

				if (newsItem == null || newsItem.IsDeleted)
					return RedirectToAction("List");

				newsItem = model.ToEntity(newsItem);
				newsItem.ModifiedOn = DateTime.Now;
				newsItem.UserId = user.Id;
				_newsService.Update(newsItem);

				// Save URL Record
				model.SystemName = newsItem.ValidateSystemName(model.SystemName, model.ShortName, true);
				_urlService.SaveSlug(newsItem, model.SystemName);

				// Update Url
				newsItem.Url = Url.RouteUrl("News", new { name = newsItem.GetSystemName() }, "http");
				_newsService.Update(newsItem);
			}
			else
			{
				model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
													select new SelectListItem
													{
														Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                                        Value = Convert.ToInt32(d).ToString(),
														Selected = (Convert.ToInt32(d) == model.NewsStatusId)
													}).ToList();

				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				ErrorNotification("An error occured while updating news. Please try again.");
				return View(model);
			}

			SuccessNotification("News updated successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = model.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			var model = new NewsModel();

			model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
												select new SelectListItem
												{
													Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
													Selected = (Convert.ToInt32(d) == model.NewsStatusId)
												}).ToList();

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
		public ActionResult Create(NewsModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			var currentUser = _userContext.CurrentUser;
			// Check for duplicate news, if any
			var newNews = new News();
			var _news = _newsService.GetNewsByShortName(model.ShortName);
			if (_news != null)
				ModelState.AddModelError("ShortName", "An News with the same name already exists. Please choose a different name.");

			model.UserId = currentUser.Id;
			if (ModelState.IsValid)
			{
				newNews = model.ToEntity();
				newNews.CreatedOn = newNews.ModifiedOn = DateTime.Now;

				_newsService.Insert(newNews);

				// Save URL Record
				model.SystemName = newNews.ValidateSystemName(model.SystemName, model.ShortName, true);
				_urlService.SaveSlug(newNews, model.SystemName);

				// Update Url
				newNews.Url = Url.RouteUrl("News", new { name = newNews.GetSystemName() }, "http");
				_newsService.Update(newNews);
			}
			else
			{
				model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
													select new SelectListItem
													{
														Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                                        Value = Convert.ToInt32(d).ToString(),
														Selected = (Convert.ToInt32(d) == model.NewsStatusId)
													}).ToList();

				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				ErrorNotification("An error occured while creating news. Please try again.");
				return View(model);
			}

			SuccessNotification("News created successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = newNews.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			_newsService.Delete(id);

			SuccessNotification("News deleted successfully");
			return RedirectToAction("List");
		}

		#endregion

		#region News Picture/Video

		[HttpPost]
		public ActionResult DeleteNewsPicture(int id)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Picture id not found");

			var pictureRecord = _pictureService.GetNewsPictureByPictureId(id);
			if (pictureRecord != null)
			{
				_pictureService.DeleteNewsPicture(pictureRecord.Id);
			}
			else
			{
				var picture = _pictureService.GetPictureById(id);
				if (picture != null)
					_pictureService.Delete(picture.Id);
			}

			SuccessNotification("News picture deleted successfully");
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
		public ActionResult DeleteNewsVideo(int id)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Video id not found");

			var videoRecord = _videoService.GetNewsVideoByVideoId(id);
			if (videoRecord != null)
			{
				_videoService.DeleteNewsVideo(videoRecord.Id);
			}
			else
			{
				var video = _videoService.GetVideoById(id);
				if (video != null)
					_pictureService.Delete(video.Id);
			}

			SuccessNotification("News video deleted successfully");
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
		public virtual ActionResult NewsPictureAdd(int pictureId, int displayOrder, bool isDefault, int newsId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisnews = _newsService.GetNewsById(newsId);
			if (thisnews == null)
				throw new ArgumentException("No news found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var newsPictureData = new EF.Core.Data.NewsPicture();
			newsPictureData.PictureId = pictureId;
			newsPictureData.NewsId = newsId;
			newsPictureData.DisplayOrder = displayOrder;
			newsPictureData.IsDefault = isDefault;
			newsPictureData.UserId = _userContext.CurrentUser.Id;
			newsPictureData.CreatedOn = newsPictureData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				newsPictureData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				newsPictureData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_pictureService.InsertNewsPicture(newsPictureData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateNewsPicture(int newsId, int pictureId, int displayOrder, bool isDefault, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisnews = _newsService.GetNewsById(newsId);
			if (thisnews == null)
				throw new ArgumentException("No news found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var newsPictureData = _pictureService.GetNewsPictureByPictureId(pictureId);
			if (newsPictureData != null)
			{
				newsPictureData.DisplayOrder = displayOrder;
				newsPictureData.IsDefault = isDefault;
				newsPictureData.ModifiedOn = DateTime.Now;

				if (newsPictureData.IsDefault)
				{
					var newsDefaultPicture = _pictureService.GetDefaultNewsPicture(newsId);
					if (newsDefaultPicture != null && newsDefaultPicture.PictureId != newsPictureData.PictureId)
					{
						newsDefaultPicture.IsDefault = false;
						_pictureService.UpdateNewsPicture(newsDefaultPicture);
					}
				}

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				newsPictureData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				newsPictureData.EndDate = picEndDate;

				_pictureService.UpdateNewsPicture(newsPictureData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult NewsVideoAdd(int videoId, int displayOrder, int newsId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisnews = _newsService.GetNewsById(newsId);
			if (thisnews == null)
				throw new ArgumentException("No news found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var newsVideoData = new EF.Core.Data.NewsVideo();
			newsVideoData.VideoId = videoId;
			newsVideoData.NewsId = newsId;
			newsVideoData.DisplayOrder = displayOrder;
			newsVideoData.UserId = _userContext.CurrentUser.Id;
			newsVideoData.CreatedOn = newsVideoData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				newsVideoData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				newsVideoData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_videoService.InsertNewsVideo(newsVideoData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetNewsPictures(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstNewsPictures = new List<NewsPictureModel>();
			var result = _pictureService.GetNewsPictureByNewsId(id);
			foreach (var x in result)
			{
				var newspic = x.ToModel();
				newspic.PicStartDate = x.StartDate;
				newspic.PicEndDate = x.EndDate;

				if (x.PictureId > 0)
				{
					var picture = _pictureService.GetPictureById(x.PictureId);
					newspic.Picture = new PictureModel()
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
				lstNewsPictures.Add(newspic);
			}
			return Json(lstNewsPictures, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetNewsVideos(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstNewsVideos = new List<NewsVideoModel>();
			var result = _videoService.GetNewsVideosByNewsId(id);
			foreach (var x in result)
			{
				var newspic = x.ToModel();
				newspic.VidStartDate = x.StartDate;
				newspic.VidEndDate = x.EndDate;

				if (x.VideoId > 0)
				{
					var picture = _videoService.GetVideoById(x.VideoId);
					newspic.Video = new VideoModel()
					{
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
						VideoSrc = picture.VideoSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstNewsVideos.Add(newspic);
			}
			return Json(lstNewsVideos, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateNewsVideo(int newsId, int videoId, int displayOrder, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageNews"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisnews = _newsService.GetNewsById(newsId);
			if (thisnews == null)
				throw new ArgumentException("No news found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var newsVideoData = _videoService.GetNewsVideoByVideoId(videoId);
			if (newsVideoData != null)
			{
				newsVideoData.DisplayOrder = displayOrder;
				newsVideoData.ModifiedOn = DateTime.Now;

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				newsVideoData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				newsVideoData.EndDate = picEndDate;

				_videoService.UpdateNewsVideo(newsVideoData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}
