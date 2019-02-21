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
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;

        #endregion Fileds

        #region Constructor

        public NewsController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, INewsService newsService, IAuthenticationService authenticationService, IRoleService roleService)
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
            this._newsService = newsService;
            this._authenticationService = authenticationService;
            this._roleService = roleService;
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

        public ActionResult Details(int id)
        {
            var model = new NewsWidgetModel();
            var news = _newsService.GetNewsById(id);
            var user = _userContext.CurrentUser;
            if (news != null)
            {
                model = news.ToWidgetModel();
                model.IsAuthenticated = false;

                if (user != null)
                    model.IsAuthenticated = true;

                var newsPictures = _pictureService.GetNewsPictureByNewsId(news.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultPicture = newsPictures.Any(x => x.IsDefault);

                var newsVideos = _videoService.GetNewsVideosByNewsId(news.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.Reactions = _smsService.SearchReactions(newsid: news.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                model.Comments = _commentService.GetCommentsByNews(news.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (model.Comments.Count > 0)
                {
                    foreach (var comment in model.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                var fromUser = _userService.GetUserById(model.UserId);
                if (fromUser != null)
                {
                    model.User = fromUser.ToModel();
                    model.Username = !string.IsNullOrEmpty(fromUser.FirstName) ? (fromUser.FirstName + (!string.IsNullOrEmpty(fromUser.LastName) ? (" " + fromUser.LastName) : "")) : !string.IsNullOrEmpty(fromUser.UserName) ? fromUser.UserName : fromUser.Email;

                    if (model.User.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(model.User.ProfilePictureId);
                        model.User.ProfilePicture = proPicture.ToModel();
                    }

                    if (model.User.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(model.User.CoverPictureId);
                        model.User.CoverPicture = coverPicture.ToModel();
                    }
                }
                var studentUser = _smsService.GetStudentByImpersonatedUser(fromUser.Id);
                var teacherUser = _smsService.GetTeacherByImpersonatedUser(fromUser.Id);
                if (studentUser != null)
                {
                    model.IsStudent = true;
                    model.Student = studentUser.ToModel();

                    if (model.Student.StudentPictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(model.Student.StudentPictureId);
                        model.User.StudentProfilePicture = proPicture.ToModel();
                    }

                    if (model.Student.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(model.Student.CoverPictureId);
                        model.User.StudentCoverPicture = coverPicture.ToModel();
                    }
                }

                if (teacherUser != null)
                {
                    model.IsTeacher = true;
                    model.Teacher = teacherUser.ToModel();

                    if (model.Teacher.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(model.Teacher.ProfilePictureId);
                        model.User.TeacherProfilePicture = proPicture.ToModel();
                    }

                    if (model.Teacher.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(model.Teacher.CoverPictureId);
                        model.User.TeacherCoverPicture = coverPicture.ToModel();
                    }
                }

                model.LatestPosts = _newsService.GetLatestNews(model.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.LatestPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                model.OlderPosts = _newsService.GetOlderNews(model.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.OlderPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                model.Pictures.Clear();
                foreach (var picture in newsPictures)
                {
                    if (!model.Pictures.Any(x => x.Id == picture.Id))
                    {
                        var picModel = picture.ToModel();
                        picModel.CreatedOn = picture.CreatedOn;
                        picModel.ModifiedOn = picture.ModifiedOn;
                        picModel.Picture = _pictureService.GetPictureById(picture.PictureId).ToModel();
                        model.Pictures.Add(picModel);
                    }
                }

                model.Videos.Clear();
                foreach (var video in newsVideos)
                {
                    if (!model.Videos.Any(x => x.Id == video.Id))
                    {
                        var vidModel = video.ToModel();
                        vidModel.CreatedOn = video.CreatedOn;
                        vidModel.ModifiedOn = video.ModifiedOn;
                        vidModel.Video = _videoService.GetVideoById(video.VideoId).ToModel();
                        model.Videos.Add(vidModel);
                    }
                }
            }
            else
            {
                return RedirectToAction("PageNotFound");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Details(NewsWidgetModel model, FormCollection frm)
        {
            var news = _newsService.GetNewsById(model.Id);
            var user = _userContext.CurrentUser;

            if (user != null)
                model.IsAuthenticated = true;

            if (ModelState.IsValid)
            {
                if (model.IsAuthenticated)
                {
                    string textComments = "";
                    var keyComment = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "txtcomment");
                    if (keyComment != null && !string.IsNullOrEmpty(frm[keyComment].ToString()))
                    {
                        textComments = frm[keyComment].ToString();

                        var comments = _commentService.GetCommentsByNews(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.NewsId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.BlogId = newComment.ProductId = 0;
                        newComment.UserId = user.Id;
                        newComment.Username = user.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            news.Comments.Add(commentEntity);

                        // Update News
                        _newsService.Update(news);
                    }
                }
                else
                {
                    var newUser = new UserModel();
                    string textComments = "";
                    var keyUsername = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "username");
                    if (keyUsername != null && !string.IsNullOrEmpty(frm[keyUsername].ToString()))
                        newUser.Username = frm[keyUsername].ToString();

                    var keyEmail = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "email");
                    if (keyEmail != null && !string.IsNullOrEmpty(frm[keyEmail].ToString()))
                        newUser.Email = frm[keyEmail].ToString();

                    var keyPassword = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "password");
                    if (keyPassword != null && !string.IsNullOrEmpty(frm[keyPassword].ToString()))
                        newUser.Password = frm[keyPassword].ToString();

                    var keyComment = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "txtcomment");
                    if (keyComment != null && !string.IsNullOrEmpty(frm[keyComment].ToString()))
                        textComments = frm[keyComment].ToString();

                    if (!string.IsNullOrEmpty(newUser.Username)
                        && !string.IsNullOrEmpty(newUser.Email)
                        && !string.IsNullOrEmpty(newUser.Password)
                        && !string.IsNullOrEmpty(textComments))
                    {
                        newUser.IsActive = true;
                        newUser.IsApproved = true;
                        newUser.IsBlocked = false;
                        newUser.UserGuid = Guid.NewGuid();
                        var userEntity = newUser.ToEntity();
                        userEntity.CreatedOn = userEntity.ModifiedOn = DateTime.Now;

                        var defaultRole = _roleService.GetRoleByName("General");
                        userEntity.Roles.Add(defaultRole);
                        _userService.Insert(userEntity);

                        // Impersonate Relation
                        var teacher = _smsService.GetTeacherByImpersonateId(userEntity.Id);
                        var student = _smsService.GetStudentByImpersonateId(userEntity.Id);

                        //sign in customer
                        _authenticationService.SignIn(userEntity, false);
                        _userContext.CurrentUser = userEntity;

                        var comments = _commentService.GetCommentsByNews(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.NewsId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.BlogId = newComment.ProductId = 0;
                        newComment.UserId = userEntity.Id;
                        newComment.Username = userEntity.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            news.Comments.Add(commentEntity);

                        // Update News
                        _newsService.Update(news);
                    }
                }

                SuccessNotification("Comment Added Successfully!");
            }
            else
            {
                if (news != null)
                {
                    model = news.ToWidgetModel();
                    model.IsAuthenticated = false;

                    if (user != null)
                        model.IsAuthenticated = true;

                    var newsPictures = _pictureService.GetNewsPictureByNewsId(news.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = newsPictures.Any(x => x.IsDefault);

                    var newsVideos = _videoService.GetNewsVideosByNewsId(news.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.Reactions = _smsService.SearchReactions(newsid: news.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByNews(news.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                    if (model.Comments.Count > 0)
                    {
                        foreach (var comment in model.Comments)
                        {
                            comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                        }
                    }

                    var fromUser = _userService.GetUserById(model.UserId);
                    if (fromUser != null)
                    {
                        model.User = fromUser.ToModel();
                        model.Username = !string.IsNullOrEmpty(fromUser.FirstName) ? (fromUser.FirstName + (!string.IsNullOrEmpty(fromUser.LastName) ? (" " + fromUser.LastName) : "")) : !string.IsNullOrEmpty(fromUser.UserName) ? fromUser.UserName : fromUser.Email;

                        if (model.User.ProfilePictureId > 0)
                        {
                            var proPicture = _pictureService.GetPictureById(model.User.ProfilePictureId);
                            model.User.ProfilePicture = proPicture.ToModel();
                        }

                        if (model.User.CoverPictureId > 0)
                        {
                            var coverPicture = _pictureService.GetPictureById(model.User.CoverPictureId);
                            model.User.CoverPicture = coverPicture.ToModel();
                        }
                    }
                    var studentUser = _smsService.GetStudentByImpersonatedUser(fromUser.Id);
                    var teacherUser = _smsService.GetTeacherByImpersonatedUser(fromUser.Id);
                    if (studentUser != null)
                    {
                        model.IsStudent = true;
                        model.Student = studentUser.ToModel();

                        if (model.Student.StudentPictureId > 0)
                        {
                            var proPicture = _pictureService.GetPictureById(model.Student.StudentPictureId);
                            model.User.StudentProfilePicture = proPicture.ToModel();
                        }

                        if (model.Student.CoverPictureId > 0)
                        {
                            var coverPicture = _pictureService.GetPictureById(model.Student.CoverPictureId);
                            model.User.StudentCoverPicture = coverPicture.ToModel();
                        }
                    }

                    if (teacherUser != null)
                    {
                        model.IsTeacher = true;
                        model.Teacher = teacherUser.ToModel();

                        if (model.Teacher.ProfilePictureId > 0)
                        {
                            var proPicture = _pictureService.GetPictureById(model.Teacher.ProfilePictureId);
                            model.User.TeacherProfilePicture = proPicture.ToModel();
                        }

                        if (model.Teacher.CoverPictureId > 0)
                        {
                            var coverPicture = _pictureService.GetPictureById(model.Teacher.CoverPictureId);
                            model.User.TeacherCoverPicture = coverPicture.ToModel();
                        }
                    }

                    model.LatestPosts = _newsService.GetLatestNews(model.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.LatestPosts)
                    {
                        var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultVideo = postVideos.Count > 0;

                        if (postPictures.Count > 0)
                        {
                            post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                            post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                        }

                        if (postVideos.Count > 0)
                        {
                            post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                            post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                        }
                    }

                    model.OlderPosts = _newsService.GetOlderNews(model.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.OlderPosts)
                    {
                        var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultVideo = postVideos.Count > 0;

                        if (postPictures.Count > 0)
                        {
                            post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                            post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                        }

                        if (postVideos.Count > 0)
                        {
                            post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                            post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                        }
                    }

                    model.Pictures.Clear();
                    foreach (var picture in newsPictures)
                    {
                        if (!model.Pictures.Any(x => x.Id == picture.Id))
                        {
                            var picModel = picture.ToModel();
                            picModel.CreatedOn = picture.CreatedOn;
                            picModel.ModifiedOn = picture.ModifiedOn;
                            picModel.Picture = _pictureService.GetPictureById(picture.PictureId).ToModel();
                            model.Pictures.Add(picModel);
                        }
                    }

                    model.Videos.Clear();
                    foreach (var video in newsVideos)
                    {
                        if (!model.Videos.Any(x => x.Id == video.Id))
                        {
                            var vidModel = video.ToModel();
                            vidModel.CreatedOn = video.CreatedOn;
                            vidModel.ModifiedOn = video.ModifiedOn;
                            vidModel.Video = _videoService.GetVideoById(video.VideoId).ToModel();
                            model.Videos.Add(vidModel);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("PageNotFound");
                }

                return View(model);
            }

            return RedirectToRoute("News", new { name = model.SystemName });
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

        public ActionResult List(PagingFilteringModel command)
        {
            var model = new NewsListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var news = _newsService.GetPagedNews(keyword: command.Keyword, author: command.Author, statusid: command.NewsStatusId, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(news);
            model.PagingFilteringContext.Keyword = command.Keyword;
            model.PagingFilteringContext.Author = command.Author;
            model.PagingFilteringContext.NewsStatusId = command.NewsStatusId;

            foreach (var record in news)
            {
                var objNews = record.ToWidgetModel();
                objNews.CreatedOn = record.CreatedOn;
                objNews.ModifiedOn = record.ModifiedOn;

                var newsPictures = _pictureService.GetNewsPictureByNewsId(record.Id).OrderByDescending(x => x.StartDate).ToList();
                objNews.HasDefaultPicture = newsPictures.Any(x => x.IsDefault);

                var newsVideos = _videoService.GetNewsVideosByNewsId(record.Id).OrderByDescending(x => x.StartDate).ToList();
                objNews.HasDefaultVideo = newsVideos.Count > 0;

                if (newsPictures.Count > 0)
                {
                    objNews.DefaultPictureSrc = newsPictures.FirstOrDefault(x => objNews.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objNews.Pictures = newsPictures.Select(x => x.ToModel()).ToList();
                }

                if (newsVideos.Count > 0)
                {
                    objNews.DefaultVideoSrc = newsVideos.FirstOrDefault().Video.VideoSrc;
                    objNews.Videos = newsVideos.Select(x => x.ToModel()).ToList();
                }

                objNews.Reactions = _smsService.SearchReactions(newsid: record.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objNews.Comments = _commentService.GetCommentsByNews(record.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objNews.Comments.Count > 0)
                {
                    foreach (var comment in objNews.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                objNews.LatestPosts = _newsService.GetLatestNews(objNews.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objNews.LatestPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                objNews.OlderPosts = _newsService.GetOlderNews(objNews.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objNews.OlderPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                model.News.Add(objNews);
            }

            model.Authors = _newsService.GetDistinctAuthorAndCount(true);
            model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
                                       select new SelectListItem
                                       {
                                           Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                           Value = Convert.ToInt32(d).ToString()
                                       }).ToList();
            return View(model);
        }


        [HttpPost]
        public ActionResult List(PagingFilteringModel command, FormCollection frm)
        {
            var model = new NewsListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var news = _newsService.GetPagedNews(keyword: command.Keyword, author: command.Author, statusid: command.NewsStatusId, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(news);
            model.PagingFilteringContext.Keyword = command.Keyword;
            model.PagingFilteringContext.Author = command.Author;
            model.PagingFilteringContext.NewsStatusId = command.NewsStatusId;

            foreach (var record in news)
            {
                var objNews = record.ToWidgetModel();
                objNews.CreatedOn = record.CreatedOn;
                objNews.ModifiedOn = record.ModifiedOn;

                var newsPictures = _pictureService.GetNewsPictureByNewsId(record.Id).OrderByDescending(x => x.StartDate).ToList();
                objNews.HasDefaultPicture = newsPictures.Any(x => x.IsDefault);

                var newsVideos = _videoService.GetNewsVideosByNewsId(record.Id).OrderByDescending(x => x.StartDate).ToList();
                objNews.HasDefaultVideo = newsVideos.Count > 0;

                if (newsPictures.Count > 0)
                {
                    objNews.DefaultPictureSrc = newsPictures.FirstOrDefault(x => objNews.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objNews.Pictures = newsPictures.Select(x => x.ToModel()).ToList();
                }

                if (newsVideos.Count > 0)
                {
                    objNews.DefaultVideoSrc = newsVideos.FirstOrDefault().Video.VideoSrc;
                    objNews.Videos = newsVideos.Select(x => x.ToModel()).ToList();
                }

                objNews.Reactions = _smsService.SearchReactions(newsid: record.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objNews.Comments = _commentService.GetCommentsByNews(record.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objNews.Comments.Count > 0)
                {
                    foreach (var comment in objNews.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                objNews.LatestPosts = _newsService.GetLatestNews(objNews.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objNews.LatestPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                objNews.OlderPosts = _newsService.GetOlderNews(objNews.Id).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objNews.OlderPosts)
                {
                    var postPictures = _pictureService.GetNewsPictureByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetNewsVideosByNewsId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultVideo = postVideos.Count > 0;

                    if (postPictures.Count > 0)
                    {
                        post.DefaultPictureSrc = postPictures.FirstOrDefault(x => post.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        post.Pictures = postPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (postVideos.Count > 0)
                    {
                        post.DefaultVideoSrc = postVideos.FirstOrDefault().Video.VideoSrc;
                        post.Videos = postVideos.Select(x => x.ToModel()).ToList();
                    }
                }

                model.News.Add(objNews);
            }

            model.Authors = _newsService.GetDistinctAuthorAndCount(true);
            model.AvailableStatuses = (from NewsStatus d in Enum.GetValues(typeof(NewsStatus))
                                       select new SelectListItem
                                       {
                                           Text = EnumExtensions.GetDescriptionByValue<NewsStatus>(Convert.ToInt32(d)),
                                           Value = Convert.ToInt32(d).ToString(),
                                           Selected = (model.PagingFilteringContext.NewsStatusId == Convert.ToInt32(d))
                                       }).ToList();
            return View(model);
        }
    }
}
