using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using SMS.Mappers;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;
using SMS.Models.Widgets;
using System;
using EF.Services;
using EF.Core.Data;

namespace SMS.Controllers
{
    public class BlogController : PublicHttpController
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

        public BlogController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, ISMSService smsService, IAuthenticationService authenticationService, ITemplateService templateService, IEmailService emailService, IRoleService roleService)
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

        [ChildActionOnly]
        public ActionResult Index()
        {
            var widgetModel = new List<BlogWidgetModel>();
            var lstBlogs = _blogService.GetAllBlogs(true).OrderByDescending(x => x.ModifiedOn).Take(3).ToList();
            if (lstBlogs.Count > 0)
            {
                foreach (var blog in lstBlogs)
                {
                    var model = new BlogWidgetModel();
                    model.Id = blog.Id;
                    model.AcadmicYear = blog.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(blog.AcadmicYearId).Name : "";
                    model.BlogHtml = blog.BlogHtml;
                    model.CreatedOn = blog.CreatedOn;
                    model.Email = blog.Email;
                    model.IpAddress = blog.IpAddress;
                    model.SystemName = blog.GetSystemName();
                    model.IsActive = blog.IsActive;
                    model.IsApproved = blog.IsApproved;
                    model.ModifiedOn = blog.ModifiedOn;
                    model.Name = blog.Name;
                    model.Subject = blog.Subject;

                    var blogPictures = _pictureService.GetBlogPictureByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = blogPictures.Any(x => x.IsDefault);

                    var blogVideos = _videoService.GetBlogVideosByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = blogVideos.Count > 0;

                    if (blogPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = blogPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        model.Pictures = blogPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (blogVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = blogVideos.FirstOrDefault().Video.VideoSrc;
                        model.Videos = blogVideos.Select(x => x.ToModel()).ToList();
                    }

                    model.Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByBlog(blog.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                    if (model.Comments.Count > 0)
                    {
                        foreach (var comment in model.Comments)
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
            var model = new BlogWidgetModel();
            var blog = _blogService.GetBlogById(id);
            var user = _userContext.CurrentUser;
            if (blog != null)
            {
                model = blog.ToWidgetModel();
                model.IsAuthenticated = false;

                if (user != null)
                    model.IsAuthenticated = true;

                var blogPictures = _pictureService.GetBlogPictureByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultPicture = blogPictures.Any(x => x.IsDefault);

                var blogVideos = _videoService.GetBlogVideosByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultVideo = blogVideos.Count > 0;

                if (blogPictures.Count > 0)
                {
                    model.DefaultPictureSrc = blogPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    model.Pictures = blogPictures.Select(x => x.ToModel()).ToList();
                }

                if (blogVideos.Count > 0)
                {
                    model.DefaultVideoSrc = blogVideos.FirstOrDefault().Video.VideoSrc;
                    model.Videos = blogVideos.Select(x => x.ToModel()).ToList();
                }

                model.Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                model.Comments = _commentService.GetCommentsByBlog(blog.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

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

                model.LatestPosts = _blogService.GetLatestBlogs(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.LatestPosts)
                {
                    var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.OlderPosts = _blogService.GetOlderBlogs(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                foreach (var post in model.OlderPosts)
                {
                    var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.PopularPosts = _blogService.GetAllBlogs(true).Where(x => x.Id != model.Id && (model.User != null ? x.UserId == model.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                foreach (var post in model.PopularPosts)
                {
                    var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                    post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                    var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                model.Subjects = _blogService.GetDistinctSubjectAndCount(true, (model.User != null ? model.User.Id : 0));
            }
            else
            {
                return RedirectToAction("PageNotFound");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Details(BlogWidgetModel model, FormCollection frm)
        {
            var blog = _blogService.GetBlogById(model.Id);
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

                        var comments = _commentService.GetCommentsByBlog(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.BlogId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.ProductId = 0;
                        newComment.UserId = user.Id;
                        newComment.Username = user.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            blog.Comments.Add(commentEntity);

                        // Update Blog
                        _blogService.Update(blog);
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

                        var comments = _commentService.GetCommentsByBlog(model.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.BlogId = model.Id;
                        newComment.CommentHtml = textComments;
                        newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.ProductId = 0;
                        newComment.UserId = userEntity.Id;
                        newComment.Username = userEntity.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            blog.Comments.Add(commentEntity);

                        // Update Blog
                        _blogService.Update(blog);

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
                if (blog != null)
                {
                    model = blog.ToWidgetModel();
                    model.IsAuthenticated = false;

                    if (user != null)
                        model.IsAuthenticated = true;

                    var blogPictures = _pictureService.GetBlogPictureByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = blogPictures.Any(x => x.IsDefault);

                    var blogVideos = _videoService.GetBlogVideosByBlogId(blog.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = blogVideos.Count > 0;

                    if (blogPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = blogPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        model.Pictures = blogPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (blogVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = blogVideos.FirstOrDefault().Video.VideoSrc;
                        model.Videos = blogVideos.Select(x => x.ToModel()).ToList();
                    }

                    model.Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByBlog(blog.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

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

                    model.LatestPosts = _blogService.GetLatestBlogs(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.LatestPosts)
                    {
                        var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.OlderPosts = _blogService.GetOlderBlogs(model.Id, (model.User != null ? model.User.Id : 0)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
                    foreach (var post in model.OlderPosts)
                    {
                        var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.PopularPosts = _blogService.GetAllBlogs(true).Where(x => x.Id != model.Id && (model.User != null ? x.UserId == model.User.Id : true)).Select(x => x.ToWidgetModel()).OrderByDescending(x => x.Comments.Count).Take(3).ToList();
                    foreach (var post in model.PopularPosts)
                    {
                        var postPictures = _pictureService.GetBlogPictureByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
                        post.HasDefaultPicture = postPictures.Any(x => x.IsDefault);

                        var postVideos = _videoService.GetBlogVideosByBlogId(post.Id).OrderByDescending(x => x.StartDate).ToList();
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

                    model.Subjects = _blogService.GetDistinctSubjectAndCount(true, (model.User != null ? model.User.Id : 0));
                }
                else
                {
                    return RedirectToAction("PageNotFound");
                }

                return View(model);
            }

            return RedirectToRoute("Blog", new { name = model.SystemName });
        }

        public ActionResult PostCommentDetail(int id, bool success)
        {
            var user = _userContext.CurrentUser;
            var model = new BlogModel();
            var blog = _blogService.GetBlogById(id);
            if (blog != null)
            {
                model = new BlogModel()
                {
                    Name = blog.Name,
                    BlogHtml = blog.BlogHtml,
                    CreatedOn = blog.CreatedOn,
                    Email = blog.Email,
                    Id = blog.Id,
                    IpAddress = blog.IpAddress,
                    Subject = blog.Subject,
                    UserId = blog.UserId,
                };
            }

            return View("~/Views/Blog/Detail.cshtml", model);
        }

    }
}
