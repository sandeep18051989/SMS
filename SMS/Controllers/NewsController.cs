using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

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

        public ActionResult Index()
        {
            var model = new List<NewsModel>();
            var lstNews = _newsService.GetActiveNews().Where(n => DateTime.Now >= n.StartDate && DateTime.Now <= n.EndDate).OrderByDescending(x => x.ModifiedOn).Take(3).ToList();
            if (lstNews.Count > 0)
            {
                foreach (var n in lstNews)
                {
                    var newsModel = new NewsModel();
                    newsModel.ShortName = n.ShortName;
                    newsModel.StartDate = n.StartDate;
                    newsModel.EndDate = n.EndDate;
                    newsModel.Author = n.Author;
                    newsModel.Description = n.Description;
                    newsModel.Id = n.Id;
                    foreach (var p in n.Pictures)
                    {
                        var proPicture = new NewsPictureModel();
                        proPicture.Id = p.Id;
                        proPicture.PictureId = p.PictureId;
                        proPicture.DisplayOrder = p.DisplayOrder;
                        proPicture.PicEndDate = p.StartDate;
                        proPicture.IsDefault = p.IsDefault;
                        proPicture.PicStartDate = p.EndDate;

                        var picture = _pictureService.GetPictureById(p.PictureId);
                        if (picture != null)
                        {
                            proPicture.Picture = new SMS.Models.PictureModel()
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
                        newsModel.Pictures.Add(proPicture);

                    }

                    model.Add(newsModel);
                };
            }
            return PartialView(model);
        }

        // GET: Teacher/Details/5
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
