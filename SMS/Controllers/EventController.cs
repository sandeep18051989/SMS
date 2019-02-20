using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using SMS.Mappers;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models.Widgets;
using EF.Services;
using SMS.Models;
using System;

namespace SMS.Controllers
{
	public class EventController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
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
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;
        private readonly IRoleService _roleService;

        #endregion Fileds

        #region Constructor

        public EventController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, ISMSService smsService, IAuthenticationService authenticationService, ITemplateService templateService, IEmailService emailService, IRoleService roleService)
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
            this._authenticationService = authenticationService;
            this._templateService = templateService;
            this._emailService = emailService;
            this._roleService = roleService;
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

        public ActionResult Details(int id)
        {
            var model = new EventWidgetModel();
            var selectedEvent = _eventService.GetEventById(id);
            var user = _userContext.CurrentUser;
            if (selectedEvent != null)
            {
                model = selectedEvent.ToWidgetModel();
                model.IsAuthenticated = false;

                if (user != null)
                    model.IsAuthenticated = true;

                var eventPictures = _pictureService.GetEventPicturesByEvent(selectedEvent.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultPicture = eventPictures.Any(x => x.IsDefault);

                var eventVideos = _videoService.GetEventVideosByEventId(selectedEvent.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultVideo = eventVideos.Count > 0;

                if (eventPictures.Count > 0)
                {
                    model.DefaultPictureSrc = eventPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    model.Pictures = eventPictures.Select(x => x.ToModel()).ToList();
                }

                if (eventVideos.Count > 0)
                {
                    model.DefaultVideoSrc = eventVideos.FirstOrDefault().Video.VideoSrc;
                    model.Videos = eventVideos.Select(x => x.ToModel()).ToList();
                }

                model.Reactions = _smsService.SearchReactions(eventid: selectedEvent.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                model.Comments = _commentService.GetCommentsByEvent(selectedEvent.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

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

                model.LatestPosts = _eventService.GetLatestEvents(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.LatestPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.OlderPosts = _eventService.GetOlderEvents(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.OlderPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.PopularPosts = _eventService.GetAllEvents(true).Where(x => x.Id != model.Id && (model.User != null ? x.UserId == model.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                foreach (var post in model.PopularPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.Venues = _eventService.GetDistinctVenueAndCount(true, (model.User != null ? model.User.Id : 0));
            }
            else
            {
                return RedirectToAction("PageNotFound");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Details(EventWidgetModel model, FormCollection frm)
        {
            var selectedEvent = _eventService.GetEventById(model.Id);
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

                        var comments = _commentService.GetCommentsByEvent(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.EventId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.ProductId = 0;
                        newComment.UserId = user.Id;
                        newComment.Username = user.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            selectedEvent.Comments.Add(commentEntity);

                        // Update Event
                        _eventService.Update(selectedEvent);
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
                        //var teacher = _smsService.GetTeacherByImpersonateId(userEntity.Id);
                        //var student = _smsService.GetStudentByImpersonateId(userEntity.Id);

                        //sign in customer
                        _authenticationService.SignIn(userEntity, false);
                        _userContext.CurrentUser = userEntity;

                        var comments = _commentService.GetCommentsByEvent(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.EventId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.ProductId = newComment.BlogId = 0;
                        newComment.UserId = userEntity.Id;
                        newComment.Username = userEntity.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            selectedEvent.Comments.Add(commentEntity);

                        // Update Event
                        _eventService.Update(selectedEvent);

                        // Send Notification To The User
                        //var templateSetting = _settingService.GetSettingByKey("UserSignInAttempt");
                        //if (templateSetting != null)
                        //{
                        //    var template = _templateService.GetTemplateByName(templateSetting.Value);

                        //    var tokens = new List<DataToken>();
                        //    _templateService.AddUserTokens(tokens, userEntity);

                        //    foreach (var dt in tokens)
                        //    {
                        //        template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
                        //    }

                        //    // Send Notification
                        //    _emailService.SendNotification(template, tokens, userEntity.Email, userEntity.UserName);
                        //}
                    }
                }

                SuccessNotification("Comment Added Successfully!");
            }
            else
            {
                if (selectedEvent != null)
                {
                    model = selectedEvent.ToWidgetModel();
                    model.IsAuthenticated = false;

                    if (user != null)
                        model.IsAuthenticated = true;

                    var eventPictures = _pictureService.GetEventPicturesByEvent(selectedEvent.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = eventPictures.Any(x => x.IsDefault);

                    var eventVideos = _videoService.GetEventVideosByEventId(selectedEvent.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = eventVideos.Count > 0;

                    if (eventPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = eventPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        model.Pictures = eventPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (eventVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = eventVideos.FirstOrDefault().Video.VideoSrc;
                        model.Videos = eventVideos.Select(x => x.ToModel()).ToList();
                    }

                    model.Reactions = _smsService.SearchReactions(eventid: selectedEvent.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByEvent(selectedEvent.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

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

                    model.LatestPosts = _eventService.GetLatestEvents(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.LatestPosts)
                    {
                        var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.OlderPosts = _eventService.GetOlderEvents(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.OlderPosts)
                    {
                        var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.PopularPosts = _eventService.GetAllEvents(true).Where(x => x.Id != model.Id && (model.User != null ? x.UserId == model.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                    foreach (var post in model.PopularPosts)
                    {
                        var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.Venues = _eventService.GetDistinctVenueAndCount(true, (model.User != null ? model.User.Id : 0));
                }
                else
                {
                    return RedirectToAction("PageNotFound");
                }

                return View(model);
            }

            return RedirectToRoute("Event", new { name = model.SystemName });
        }

        public ActionResult List(PagingFilteringModel command)
        {
            var model = new EventListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var events = _eventService.GetPagedEvents(keyword: command.Keyword, venue: command.Venue, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(events);
            model.PagingFilteringContext.Keyword = command.Keyword;
            model.PagingFilteringContext.Venue = command.Venue;

            foreach (var eve in events)
            {
                var objEvent = eve.ToWidgetModel();
                objEvent.ModifiedOn = eve.ModifiedOn;
                objEvent.CreatedOn = eve.CreatedOn;

                var eventPictures = _pictureService.GetEventPicturesByEvent(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                objEvent.HasDefaultPicture = eventPictures.Any(x => x.IsDefault);

                var eventVideos = _videoService.GetEventVideosByEventId(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                objEvent.HasDefaultVideo = eventVideos.Count > 0;

                if (eventPictures.Count > 0)
                {
                    objEvent.DefaultPictureSrc = eventPictures.FirstOrDefault(x => objEvent.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objEvent.Pictures = eventPictures.Select(x => x.ToModel()).ToList();
                }

                if (eventVideos.Count > 0)
                {
                    objEvent.DefaultVideoSrc = eventVideos.FirstOrDefault().Video.VideoSrc;
                    objEvent.Videos = eventVideos.Select(x => x.ToModel()).ToList();
                }

                objEvent.Reactions = _smsService.SearchReactions(eventid: eve.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objEvent.Comments = _commentService.GetCommentsByEvent(eve.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objEvent.Comments.Count > 0)
                {
                    foreach (var comment in objEvent.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                var fromUser = _userService.GetUserById(objEvent.UserId);
                if (fromUser != null)
                {
                    objEvent.User = fromUser.ToModel();
                    objEvent.Username = !string.IsNullOrEmpty(fromUser.FirstName) ? (fromUser.FirstName + (!string.IsNullOrEmpty(fromUser.LastName) ? (" " + fromUser.LastName) : "")) : !string.IsNullOrEmpty(fromUser.UserName) ? fromUser.UserName : fromUser.Email;

                    if (objEvent.User.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.User.ProfilePictureId);
                        objEvent.User.ProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.User.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.User.CoverPictureId);
                        objEvent.User.CoverPicture = coverPicture.ToModel();
                    }
                }
                var studentUser = _smsService.GetStudentByImpersonatedUser(fromUser.Id);
                var teacherUser = _smsService.GetTeacherByImpersonatedUser(fromUser.Id);
                if (studentUser != null)
                {
                    objEvent.IsStudent = true;
                    objEvent.Student = studentUser.ToModel();

                    if (objEvent.Student.StudentPictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.Student.StudentPictureId);
                        objEvent.User.StudentProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.Student.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.Student.CoverPictureId);
                        objEvent.User.StudentCoverPicture = coverPicture.ToModel();
                    }
                }

                if (teacherUser != null)
                {
                    objEvent.IsTeacher = true;
                    objEvent.Teacher = teacherUser.ToModel();

                    if (objEvent.Teacher.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.Teacher.ProfilePictureId);
                        objEvent.User.TeacherProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.Teacher.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.Teacher.CoverPictureId);
                        objEvent.User.TeacherCoverPicture = coverPicture.ToModel();
                    }
                }

                objEvent.LatestPosts = _eventService.GetLatestEvents(objEvent.Id, (objEvent.User != null ? objEvent.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objEvent.LatestPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                objEvent.OlderPosts = _eventService.GetOlderEvents(objEvent.Id, (objEvent.User != null ? objEvent.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objEvent.OlderPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                objEvent.PopularPosts = _eventService.GetAllEvents(true).Where(x => x.Id != objEvent.Id && (objEvent.User != null ? x.UserId == objEvent.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                foreach (var post in objEvent.PopularPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                //model.Pictures = eventPictures.Select(x => _pictureService.GetPictureById(x.PictureId).ToModel()).OrderByDescending(x => x.CreatedOn.HasValue ? x.CreatedOn.Value : x.ModifiedOn.Value).ToList();
                //model.Videos = eventVideos.Select(x => _videoService.GetVideoById(x.VideoId).ToModel()).OrderByDescending(x => x.CreatedOn.HasValue ? x.CreatedOn.Value : x.ModifiedOn.Value).ToList();
                model.Events.Add(objEvent);
            }

            model.Locations = _eventService.GetDistinctLocationAndCount(true);

            return View(model);
        }


        [HttpPost]
        public ActionResult List(PagingFilteringModel command, FormCollection frm)
        {
            var model = new EventListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var events = _eventService.GetPagedEvents(keyword: command.Keyword, venue: command.Venue, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(events);
            model.PagingFilteringContext.Keyword = command.Keyword;
            model.PagingFilteringContext.Venue = command.Venue;

            foreach (var eve in events)
            {
                var objEvent = eve.ToWidgetModel();
                objEvent.ModifiedOn = eve.ModifiedOn;
                objEvent.CreatedOn = eve.CreatedOn;

                var eventPictures = _pictureService.GetEventPicturesByEvent(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                objEvent.HasDefaultPicture = eventPictures.Any(x => x.IsDefault);

                var eventVideos = _videoService.GetEventVideosByEventId(eve.Id).OrderByDescending(x => x.StartDate).ToList();
                objEvent.HasDefaultVideo = eventVideos.Count > 0;

                if (eventPictures.Count > 0)
                {
                    objEvent.DefaultPictureSrc = eventPictures.FirstOrDefault(x => objEvent.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objEvent.Pictures = eventPictures.Select(x => x.ToModel()).ToList();
                }

                if (eventVideos.Count > 0)
                {
                    objEvent.DefaultVideoSrc = eventVideos.FirstOrDefault().Video.VideoSrc;
                    objEvent.Videos = eventVideos.Select(x => x.ToModel()).ToList();
                }

                objEvent.Reactions = _smsService.SearchReactions(eventid: eve.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objEvent.Comments = _commentService.GetCommentsByEvent(eve.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objEvent.Comments.Count > 0)
                {
                    foreach (var comment in objEvent.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                var fromUser = _userService.GetUserById(objEvent.UserId);
                if (fromUser != null)
                {
                    objEvent.User = fromUser.ToModel();
                    objEvent.Username = !string.IsNullOrEmpty(fromUser.FirstName) ? (fromUser.FirstName + (!string.IsNullOrEmpty(fromUser.LastName) ? (" " + fromUser.LastName) : "")) : !string.IsNullOrEmpty(fromUser.UserName) ? fromUser.UserName : fromUser.Email;

                    if (objEvent.User.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.User.ProfilePictureId);
                        objEvent.User.ProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.User.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.User.CoverPictureId);
                        objEvent.User.CoverPicture = coverPicture.ToModel();
                    }
                }
                var studentUser = _smsService.GetStudentByImpersonatedUser(fromUser.Id);
                var teacherUser = _smsService.GetTeacherByImpersonatedUser(fromUser.Id);
                if (studentUser != null)
                {
                    objEvent.IsStudent = true;
                    objEvent.Student = studentUser.ToModel();

                    if (objEvent.Student.StudentPictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.Student.StudentPictureId);
                        objEvent.User.StudentProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.Student.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.Student.CoverPictureId);
                        objEvent.User.StudentCoverPicture = coverPicture.ToModel();
                    }
                }

                if (teacherUser != null)
                {
                    objEvent.IsTeacher = true;
                    objEvent.Teacher = teacherUser.ToModel();

                    if (objEvent.Teacher.ProfilePictureId > 0)
                    {
                        var proPicture = _pictureService.GetPictureById(objEvent.Teacher.ProfilePictureId);
                        objEvent.User.TeacherProfilePicture = proPicture.ToModel();
                    }

                    if (objEvent.Teacher.CoverPictureId > 0)
                    {
                        var coverPicture = _pictureService.GetPictureById(objEvent.Teacher.CoverPictureId);
                        objEvent.User.TeacherCoverPicture = coverPicture.ToModel();
                    }
                }

                objEvent.LatestPosts = _eventService.GetLatestEvents(objEvent.Id, (objEvent.User != null ? objEvent.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objEvent.LatestPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                objEvent.OlderPosts = _eventService.GetOlderEvents(objEvent.Id, (objEvent.User != null ? objEvent.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in objEvent.OlderPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                objEvent.PopularPosts = _eventService.GetAllEvents(true).Where(x => x.Id != objEvent.Id && (objEvent.User != null ? x.UserId == objEvent.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                foreach (var post in objEvent.PopularPosts)
                {
                    var postPictures = _pictureService.GetEventPicturesByEvent(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetEventVideosByEventId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                //model.Pictures = eventPictures.Select(x => _pictureService.GetPictureById(x.PictureId).ToModel()).OrderByDescending(x => x.CreatedOn.HasValue ? x.CreatedOn.Value : x.ModifiedOn.Value).ToList();
                //model.Videos = eventVideos.Select(x => _videoService.GetVideoById(x.VideoId).ToModel()).OrderByDescending(x => x.CreatedOn.HasValue ? x.CreatedOn.Value : x.ModifiedOn.Value).ToList();
                model.Events.Add(objEvent);
            }

            model.Locations = _eventService.GetDistinctLocationAndCount(true);

            return View(model);
        }

    }
}
