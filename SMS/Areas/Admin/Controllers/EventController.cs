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
	public class EventController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IEventService _eventService;
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

		public EventController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IPermissionService permissionService, IUrlHelper urlHelper, IUrlService urlService, ISMSService smsService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._eventService = eventService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._permissionService = permissionService;
			this._urlHelper = urlHelper;
			this._urlService = urlService;
			this._smsService = smsService;
		}

		#endregion

		#region Utilities

		public DateTime GetCustomFormattedDate(DateTime date)
		{
			DateTime validDateValue;
			var ValidDate = DateTime.TryParse(date.ToString("yyyy-MM-dd"), out validDateValue) ? validDateValue : (DateTime?)null;

			return validDateValue;
		}

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
				var eventData = (from tempevents in _eventService.GetAllEvents() select tempevents);

				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					eventData = eventData.Where(m => m.Title.Contains(searchValue) || m.Description.Contains(searchValue) || m.Venue.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = eventData.Count();
				//Paging     
				var data = eventData.Skip(skip).Take(pageSize).ToList();

				//Returning Json Data 
				return new JsonResult()
				{
					Data = new
					{
						draw = draw,
						recordsFiltered = recordsTotal,
						recordsTotal = recordsTotal,
						data = data.Select(x => new EventListModel()
						{
							Headline = !string.IsNullOrEmpty(x.Headline) ? x.Headline : "",
							Venue = !string.IsNullOrEmpty(x.Venue) ? x.Venue : "",
							CommentsCount = x.Comments.Count,
							Id = x.Id,
							AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
							Url = Url.RouteUrl("Event", new { name = x.GetSystemName() }, "http"),
							EndDate = x.EndDate?.ToString("d MMM yyyy") ?? "",
							IsActive = x.IsActive,
							IsApproved = x.IsApproved,
							PicturesCount = x.Pictures.Count,
							ReactionsCount = x.Reactions.Count,
							StartDate = x.StartDate?.ToString("d MMM yyyy") ?? "",
							Title = x.Title,
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
				var eventData = (from associatedpicture in _pictureService.GetEventPicturesByEvent(id) select associatedpicture).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = eventData.Select(x => new PictureListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							IsDefault = x.IsDefault,
							EventId = id,
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
				var eventData = (from associatedvideo in _videoService.GetEventVideosByEventId(id) select associatedvideo);
				return new JsonResult()
				{
					Data = new
					{
						data = eventData.Select(x => new VideoListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							EventId = id,
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

		#region Event Methods

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

            var model = new List<EventModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var model = new EventModel();
			if (id == 0)
				throw new Exception("Event Id Missing");

			var eve = _eventService.GetEventById(id);
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
		public ActionResult Edit(EventModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var newEventPictures = new List<EventPicture>();
			// Check for duplicate event, if any
			var _event = _eventService.GetEventByName(model.Title);
			if (_event != null && _event.Id != model.Id)
				ModelState.AddModelError("Title", "An Event with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var eve = _eventService.GetEventById(model.Id);

				if (eve == null || eve.IsDeleted)
					return RedirectToAction("List");

				eve = model.ToEntity(eve);
				eve.ModifiedOn = DateTime.Now;
				_eventService.Update(eve);

				// Save URL Record
				model.SystemName = eve.ValidateSystemName(model.SystemName, model.Title, true);
				_urlService.SaveSlug(eve, model.SystemName);
			}
			else
			{
				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				ErrorNotification("An error occured while updating event. Please try again.");
				return View(model);
			}

			SuccessNotification("Event updated successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = model.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var model = new EventModel();
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
		public ActionResult Create(EventModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			// Check for duplicate event, if any
			var newEvent = new Event();
			var _event = _eventService.GetEventByName(model.Title);
			if (_event != null)
				ModelState.AddModelError("Title", "An Event with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				newEvent = model.ToEntity();
				newEvent.CreatedOn = newEvent.ModifiedOn = DateTime.Now;
				newEvent.UserId = _userContext.CurrentUser.Id;

				_eventService.Insert(newEvent);

				// Save URL Record
				model.SystemName = newEvent.ValidateSystemName(model.SystemName, model.Title, true);
				_urlService.SaveSlug(newEvent, model.SystemName);
			}
			else
			{
				ErrorNotification("An error occured while creating event. Please try again.");
				return View(model);
			}

			SuccessNotification("Event created successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = newEvent.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			_eventService.Delete(id);

			SuccessNotification("Event deleted successfully");
			return RedirectToAction("List");
		}

        public ActionResult ToggleActiveStatusEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            _eventService.ToggleActiveStatusEvent(id);
            SuccessNotification("Event updated successfully");

            return Json(new { Result = true });
        }

        public ActionResult ToggleApproveStatusEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            _eventService.ToggleApproveStatusEvent(id);
            SuccessNotification("Event updated successfully");

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageEvents"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _eventService.DeleteEvents(_eventService.GetEventsByIds(selectedIds.ToArray()).ToList());
            }

            SuccessNotification("Events deleted successfully.");
            return RedirectToAction("List");
        }


        #endregion

        #region Event Picture

        [HttpPost]
		public ActionResult DeleteEventPicture(int id)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Picture id not found");

			var pictureRecord = _pictureService.GetEventPictureByPictureId(id);
			if (pictureRecord != null)
			{
				_pictureService.DeleteEventPicture(pictureRecord.Id);
			}
			else
			{
				var picture = _pictureService.GetPictureById(id);
				if (picture != null)
					_pictureService.Delete(picture.Id);
			}

			SuccessNotification("Event picture deleted successfully");
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
		public ActionResult DeleteEventVideo(int id)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Video id not found");

			var videoRecord = _videoService.GetEventVideoByVideoId(id);
			if (videoRecord != null)
			{
				_videoService.DeleteEventVideo(videoRecord.Id);
			}
			else
			{
				var video = _videoService.GetVideoById(id);
				if (video != null)
					_pictureService.Delete(video.Id);
			}

			SuccessNotification("Event video deleted successfully");
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
		public ActionResult EventPicture(int pictureid, int? eventid = null)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (pictureid == 0)
				throw new Exception("Picture id not found");

			if (eventid.HasValue || eventid.Value > 0)
			{
				// Edit Case
			}
			else
			{
				// Create Case
			}

			//var pictureRecord = _pictureService.GetEventPictureByPictureId(id);
			//if (pictureRecord != null)
			//{
			//	_pictureService.DeleteEventPicture(pictureRecord.Id);
			//}
			//else
			//{
			//	var picture = _pictureService.GetPictureById(id);
			//	if (picture != null)
			//		_pictureService.Delete(picture.Id);
			//}

			SuccessNotification("Event picture deleted successfully");
			return Json(true);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult EventPictureAdd(int pictureId, int displayOrder, bool isDefault, int eventId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisevent = _eventService.GetEventById(eventId);
			if (thisevent == null)
				throw new ArgumentException("No event found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var eventPictureData = new EF.Core.Data.EventPicture();
			eventPictureData.PictureId = pictureId;
			eventPictureData.EventId = eventId;
			eventPictureData.DisplayOrder = displayOrder;
			eventPictureData.IsDefault = isDefault;
			eventPictureData.UserId = _userContext.CurrentUser.Id;
			eventPictureData.CreatedOn = eventPictureData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				eventPictureData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				eventPictureData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_pictureService.InsertEventPicture(eventPictureData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateEventPicture(int eventId, int pictureId, int displayOrder, bool isDefault, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisevent = _eventService.GetEventById(eventId);
			if (thisevent == null)
				throw new ArgumentException("No event found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var eventPictureData = _pictureService.GetEventPictureByPictureId(pictureId);
			if (eventPictureData != null)
			{
				eventPictureData.DisplayOrder = displayOrder;
				eventPictureData.IsDefault = isDefault;
				eventPictureData.ModifiedOn = DateTime.Now;

				if (eventPictureData.IsDefault)
				{
					var eventDefaultPicture = _pictureService.GetDefaultEventPicture(eventId);
					if (eventDefaultPicture != null && eventDefaultPicture.PictureId != eventPictureData.PictureId)
					{
						eventDefaultPicture.IsDefault = false;
						_pictureService.UpdateEventPicture(eventDefaultPicture);
					}
				}

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				eventPictureData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				eventPictureData.EndDate = picEndDate;

				_pictureService.UpdateEventPicture(eventPictureData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult EventVideoAdd(int videoId, int displayOrder, int eventId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisevent = _eventService.GetEventById(eventId);
			if (thisevent == null)
				throw new ArgumentException("No event found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var eventVideoData = new EF.Core.Data.EventVideo();
			eventVideoData.VideoId = videoId;
			eventVideoData.EventId = eventId;
			eventVideoData.DisplayOrder = displayOrder;
			eventVideoData.UserId = _userContext.CurrentUser.Id;
			eventVideoData.CreatedOn = eventVideoData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				eventVideoData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				eventVideoData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_videoService.InsertEventVideo(eventVideoData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetEventPictures(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstEventPictures = new List<EventPictureModel>();
			var result = _pictureService.GetEventPicturesByEvent(id);
			foreach (var x in result)
			{
				var eventpic = x.ToModel();
				eventpic.PicStartDate = x.StartDate;
				eventpic.PicEndDate = x.EndDate;

				if (x.PictureId > 0)
				{
					var picture = _pictureService.GetPictureById(x.PictureId);
					eventpic.Picture = new PictureModel()
					{
						AlternateText = picture.AlternateText,
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
                        PictureSrc = picture.PictureSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstEventPictures.Add(eventpic);
			}
			return Json(lstEventPictures, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetEventVideos(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstEventVideos = new List<EventVideoModel>();
			var result = _videoService.GetEventVideosByEventId(id);
			foreach (var x in result)
			{
				var eventpic = x.ToModel();
				eventpic.VidStartDate = x.StartDate;
				eventpic.VidEndDate = x.EndDate;

				if (x.VideoId > 0)
				{
					var picture = _videoService.GetVideoById(x.VideoId);
					eventpic.Video = new VideoModel()
					{
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
						VideoSrc = picture.VideoSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstEventVideos.Add(eventpic);
			}
			return Json(lstEventVideos, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateEventVideo(int eventId, int videoId, int displayOrder, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisevent = _eventService.GetEventById(eventId);
			if (thisevent == null)
				throw new ArgumentException("No event found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var eventVideoData = _videoService.GetEventVideoByVideoId(videoId);
			if (eventVideoData != null)
			{
				eventVideoData.DisplayOrder = displayOrder;
				eventVideoData.ModifiedOn = DateTime.Now;

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				eventVideoData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				eventVideoData.EndDate = picEndDate;

				_videoService.UpdateEventVideo(eventVideoData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}
