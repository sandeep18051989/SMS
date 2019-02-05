using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using SMS.Mappers;
using EF.Services.Service;
using SMS.Models;
using SMS.Models.Widgets;
using System;
using EF.Services;
using EF.Core.Enums;

namespace SMS.Controllers
{
    public class NewsController : PublicHttpController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;
        private readonly ISMSService _smsService;
        private readonly INewsService _newsService;
        private readonly IVideoService _videoService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IBlogService _blogService;

        #endregion Fileds

        #region Constructor

        public NewsController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, INewsService newsService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._smsService = smsService;
            this._videoService = videoService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._blogService = blogService;
            this._newsService = newsService;
        }

        #endregion

        [ChildActionOnly]
        public ActionResult Index()
        {
            var returnResult = new List<NewsWidgetModel>();
            var widgetModel = _newsService.GetAllNews(true).Where(x => DateTime.Now >= x.StartDate && DateTime.Now <= x.EndDate).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(4).ToList();
            if (widgetModel.Count > 0)
            {
                foreach (var n in widgetModel)
                {
                    var model = new NewsWidgetModel();
                    model.Status = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(n.NewsStatusId));

                    DateTime now = DateTime.Now;
                    var lastOneHour = now.AddHours(-1);
                    if (n.StartDate != null)
                    {
                        if (n.StartDate.Value.Date == DateTime.Now.Date)
                        {
                            model.NewsStatusId = 5;
                            model.Status = "Latest";
                        }
                        else if (n.StartDate.Value > lastOneHour && n.StartDate.Value <= now)
                        {
                            model.NewsStatusId = 4;
                            model.Status = "Fresh";
                        }
                    }

                    if (n.EndDate != null && n.EndDate.Value < now.AddHours(-1))
                    {
                        model.NewsStatusId = 6;
                        model.Status = "Outdated";
                    }

                    var newsPictures = _pictureService.GetNewsPictureByNewsId(n.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = newsPictures.Any(x => x.IsDefault);

                    var newsVideos = _videoService.GetNewsVideosByNewsId(n.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = newsVideos.Count > 0;

                    if (newsPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = newsPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        model.Pictures = newsPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (newsVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = newsVideos.FirstOrDefault().Video.VideoSrc;
                        model.Videos = newsVideos.Select(x => x.ToModel()).ToList();
                    }

                    model.Reactions = _smsService.SearchReactions(newsid: n.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByNews(n.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();
                    model.Author = n.Author;
                    model.Description = n.Description;
                    model.EndDate = n.EndDate;
                    model.Id = n.Id;
                    model.NewsStatusId = n.NewsStatusId;
                    model.ShortName = n.ShortName;
                    model.StartDate = n.StartDate;
                    model.SystemName = n.SystemName;

                    if (model.Comments.Count > 0)
                    {
                        foreach (var comment in model.Comments)
                        {
                            comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                        }
                    }

                    returnResult.Add(model);
                };
            }
            return PartialView(returnResult);
        }

        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
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

        // POST: Teacher/Edit/5
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

        // GET: Teacher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teacher/Delete/5
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
