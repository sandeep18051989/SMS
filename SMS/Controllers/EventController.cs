using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using SMS.Mappers;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models.Widgets;

namespace SMS.Controllers
{
	public class EventController : PublicHttpController
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
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public EventController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, ISMSService smsService)
		{
			_userService = userService;
			_pictureService = pictureService;
			_userContext = userContext;
			_sliderService = sliderService;
			_settingService = settingService;
			_eventService = eventService;
			_videoService = videoService;
			_commentService = commentService;
			_replyService = replyService;
			_blogService = blogService;
			_smsService = smsService;
		}

		#endregion

		// GET: Event
		[ChildActionOnly]
		public ActionResult Index()
		{
			var widgetModel = new List<EventWidgetModel>();
			var lstEvents = _eventService.GetActiveEvents().OrderByDescending(x => x.CreatedOn).Take(4).ToList();
			if (lstEvents.Count > 0)
			{
				foreach (var eve in lstEvents)
				{
					var model = new EventWidgetModel();
                    model.StartDate = eve.StartDate;
                    model.EndDate = eve.EndDate;
                    model.Description = eve.Description;
                    model.Id = eve.Id;
                    model.Title = eve.Title;
                    model.Venue = eve.Venue;
                    model.SystemName = eve.GetSystemName();
                    model.AcadmicYear = _smsService.GetAcadmicYearById(eve.AcadmicYearId).Name;
                    model.Headline = eve.Headline;

                    var eventPictures = _pictureService.GetEventPicturesByEvent(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = eventPictures.Any(x => x.IsDefault);

                    var eventVideos = _videoService.GetEventVideosByEventId(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = eventVideos.Count > 0;

                    if(eventPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = eventPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    }

                    if(eventVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = eventVideos.FirstOrDefault().Video.VideoSrc;
                    }

                    model.IsActive = eve.IsActive;
                    model.IsApproved = eve.IsApproved;
                    model.Pictures = eventPictures.Select(x => x.ToModel()).ToList();
                    model.Videos = eventVideos.Select(x => x.ToModel()).ToList();
                    model.Reactions = _smsService.SearchReactions(eventid: eve.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByEvent(eve.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                    if(model.Comments.Count > 0)
                    {
                        foreach(var comment in model.Comments)
                        {
                            comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                        }
                    }

                    widgetModel.Add(model);
				}
			}
			return PartialView(widgetModel);
		}

		// GET: Event/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Event/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Event/Create
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

		// POST: Event/Edit/5
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

		// GET: Event/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Event/Delete/5
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
	}
}
