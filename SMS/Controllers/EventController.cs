using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;

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
            this._smsService = smsService;
        }

        #endregion

        // GET: Event
        public ActionResult Index()
        {
            var model = new List<EventModel>();
            var lstEvents = _eventService.GetActiveEvents();
            if (lstEvents.Count > 0)
            {
                foreach (var eve in lstEvents)
                {
                    var eventModel = new EventModel();
                    eventModel.StartDate = eve.StartDate;
                    eventModel.EndDate = eve.EndDate;
                    eventModel.Description = eve.Description;
                    eventModel.Id = eve.Id;
                    eventModel.Title = eve.Title;
                    eventModel.UserId = eve.UserId;
                    eventModel.Venue = eve.Venue;
                    eventModel.Url = Url.RouteUrl("Event", new { name = eve.GetSystemName() });

                    foreach (var x in eve.Comments)
                    {
                        var commentModel = new CommentModel()
                        {
                            CommentHtml = x.CommentHtml,
                            CommentId = x.Id,
                            CreatedOn = x.CreatedOn,
                            ModifiedOn = x.ModifiedOn,
                            DisplayOrder = x.DisplayOrder,
                            Username = x.Username,
                            Replies = _replyService.GetAllRepliesByComment(x.Id).Select(r => new RepliesModel()
                            {
                                DisplayOrder = r.DisplayOrder,
                                CreatedOn = r.CreatedOn,
                                IsModified = r.IsModified,
                                ReplyHtml = r.ReplyHtml,
                                Reactions = _smsService.SearchReactions(replyid: r.Id).Select(re => new ReactionModel()
                                {
                                    Id = re.Id,
                                    CommentId = re.CommentId,
                                    CreatedOn = re.CreatedOn,
                                    IsAngry = re.IsAngry,
                                    IsDislike = re.IsDislike,
                                    IsHappy = re.IsHappy,
                                    IsLike = re.IsLike,
                                    IsLol = re.IsLOL,
                                    IsSad = re.IsSad,
                                    ModifiedOn = re.ModifiedOn,
                                    Rating = re.Rating,
                                    UserId = re.UserId,
                                    Username = re.Username
                                }).ToList(),
                            }).ToList(),
                            Reactions = _smsService.SearchReactions(commentid: x.Id).Select(re => new ReactionModel()
                            {
                                Id = re.Id,
                                CommentId = re.CommentId,
                                CreatedOn = re.CreatedOn,
                                IsAngry = re.IsAngry,
                                IsDislike = re.IsDislike,
                                IsHappy = re.IsHappy,
                                IsLike = re.IsLike,
                                IsLol = re.IsLOL,
                                IsSad = re.IsSad,
                                ModifiedOn = re.ModifiedOn,
                                Rating = re.Rating,
                                UserId = re.UserId,
                                Username = re.Username
                            }).ToList(),
                        };
                        eventModel.Comments.Add(commentModel);

                        foreach (var p in eve.Pictures)
                        {
                            var pictureModel = new EventPictureModel()
                            {
                                Id = p.Id,
                                PictureId = p.PictureId,
                                Picture = new SMS.Models.PictureModel()
                                {
                                    Id = p.Picture.Id,
                                    IsActive = p.Picture.IsActive,
                                    Url = p.Picture.Url,
                                    AlternateText = p.Picture.AlternateText,
                                    Src = p.Picture.PictureSrc
                                },
                                DisplayOrder = p.DisplayOrder,
                                PicEndDate = p.EndDate,
                                IsDefault = p.IsDefault,
                                PicStartDate = p.StartDate,
                            };

                            eventModel.Pictures.Add(pictureModel);
                        }

                        foreach (var v in eve.Videos)
                        {
                            var videoModel = new VideoModel()
                            {
                                IsActive = v.IsActive,
                                Size = v.Size,
                                Url = v.Url,
                                Id = v.Id
                            };

                            eventModel.Videos.Add(videoModel);
                        }
                    }

                    model.Add(eventModel);
                }
            }
            return PartialView(model);
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
