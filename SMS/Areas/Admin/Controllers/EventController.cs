using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
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
				var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
				var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
				var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


				//Paging Size (10,20,50,100)    
				int pageSize = length != null ? Convert.ToInt32(length) : 0;
				int skip = start != null ? Convert.ToInt32(start) : 0;
				int recordsTotal = 0;

				// Getting all data    
				var eventData = (from tempevents in _eventService.GetActiveEvents() select tempevents);

				//Sorting    
				//if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
				//{
				//	eventData = eventData.AsEnumerable().OrderBy($"{sortColumn} {sortColumnDir}");
				//}
				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					eventData = eventData.Where(m => m.Title.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = eventData.Count();
				//Paging     
				var data = eventData.Skip(skip).Take(pageSize).ToList();
				//Returning Json Data    
				return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		#endregion

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var model = new List<EventModel>();
			var lstEvents = _eventService.GetAllEvents(true).Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedOn).ToList();
			if (lstEvents.Count > 0)
			{
				foreach (var eve in lstEvents)
				{
					// Fill Event Model
					var eventModel = eve.ToModel();

					if (eve.Comments.Count > 0)
					{
						foreach (var comment in eve.Comments)
						{
							eventModel.Comments.Add(comment.ToModel());
						}
					}

					foreach (var p in eve.Pictures)
					{
						var evepicture = new EventPictureModel();
						evepicture.Id = p.Id;
						evepicture.PictureId = p.PictureId;
						evepicture.DisplayOrder = p.DisplayOrder;
						evepicture.PicEndDate = p.EndDate;
						evepicture.IsDefault = p.IsDefault;
						evepicture.PicStartDate = p.StartDate;

						if (p.PictureId > 0)
						{
							var evepic = _pictureService.GetPictureById(p.PictureId);
							if (evepic != null)
								evepicture.Picture = new PictureModel()
								{
									AlternateText = evepic.AlternateText,
									CreatedOn = evepic.CreatedOn,
									Id = evepic.Id,
									Height = evepic.Height,
									ModifiedOn = evepic.ModifiedOn,
									Src = evepic.PictureSrc,
									Url = evepic.Url,
									UserId = evepic.UserId
								};
						}

						eventModel.Pictures.Add(evepicture);
					}

					if (eve.Reactions.Count > 0)
					{
						foreach (var reaction in eve.Reactions)
						{
							eventModel.Reactions.Add(reaction.ToModel());
						}
					}

					if (eve.Videos.Count > 0)
					{
						foreach (var video in eve.Videos)
						{
							eventModel.Videos.Add(video.ToModel());
						}
					}

					eventModel.Pictures = eventModel.Pictures.OrderBy(p => p.DisplayOrder).ToList();
					model.Add(eventModel);
				}
			}

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
				model = new EventModel
				{
					StartDate = eve.StartDate,
					EndDate = eve.EndDate,
					IsActive = eve.IsActive,
					Description = eve.Description,
					Title = eve.Title,
					UserId = eve.UserId,
					Venue = eve.Venue,
					SystemName = Url.RouteUrl("Event", new { name = eve.GetSystemName() }, "http"),
					Id = eve.Id
				};
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(EventModel model, FormCollection frm)
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

				eve.CreatedOn = DateTime.Now;
				eve.Title = model.Title;
				eve.Description = model.Description;
				eve.IsActive = model.IsActive;
				eve.ModifiedOn = DateTime.Now;
				eve.Title = model.Title;
				eve.UserId = user.Id;

				if (frm["uploadedfiles"] != null)
				{
					var pictureIds = frm["uploadedfiles"].ToString().Split(',');
					eve.Pictures.Clear();

					foreach (var pictureid in pictureIds)
					{
						int picId = 0;
						int.TryParse(pictureid, out picId);

						if (eve.Pictures.All(x => x.PictureId != picId))
						{
							 eve.Pictures.Add(new EventPicture()
							 {
								 PictureId = picId,
								 CreatedOn = DateTime.Now
							 });
						}
						else
						{

						}
					}
				}
				_eventService.Update(eve);

				// Save URL Record
				model.SystemName = eve.ValidateSystemName(model.SystemName, model.Title, true);
				_urlService.SaveSlug(eve, model.SystemName);
			}
			else
			{
				ErrorNotification("An error occured while updating event. Please try again.");
				return View(model);
			}

			SuccessNotification("Event updated successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var model = new EventModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(EventModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageEvents"))
				return AccessDeniedView();

			var currentUser = _userContext.CurrentUser;
			// Check for duplicate event, if any
			var _event = _eventService.GetEventByName(model.Title);
			if (_event != null)
				ModelState.AddModelError("Title", "An Event with the same name already exists. Please choose a different name.");

			model.UserId = currentUser.Id;
			if (ModelState.IsValid)
			{
				model.CreatedOn = model.ModifiedOn = DateTime.Now;
				model.AcadmicYearId = _userContext.CurrentAcadmicYear.Id;
				model.Url = "";
				var newEvent = new Event();
				newEvent = model.ToEntity();

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
				_pictureService.Delete(pictureRecord.PictureId);
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

		public JsonResult GetEventPictures(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstEventPictures = new List<EventPictureModel>();
			var result = _pictureService.GetEventPictureByEventId(id);
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
						Src = picture.PictureSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstEventPictures.Add(eventpic);
			}
			return Json(lstEventPictures, JsonRequestBehavior.AllowGet);
		}

	}
}
