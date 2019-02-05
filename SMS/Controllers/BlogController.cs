using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using SMS.Mappers;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;
using SMS.Models.Widgets;

namespace SMS.Controllers
{
	public class BlogController : PublicHttpController
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

		public BlogController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, ISMSService smsService)
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

        public ActionResult Detail(int id)
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
			else
			{
				return RedirectToAction("PageNotFound");
			}

			return View(model);
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
