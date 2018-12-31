using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

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

		// GET: Blog
		public ActionResult Index()
		{
			var model = new List<BlogModel>();
			var lstBlogs = _blogService.GetAllBlogs(true).OrderByDescending(x => x.CreatedOn).ToList();
			if (lstBlogs.Count > 0)
			{
				foreach (var blog in lstBlogs)
				{
					model.Add(new BlogModel()
					{
						Name = blog.Name,
						BlogHtml = blog.BlogHtml,
						CreatedOn = blog.CreatedOn,
						Email = blog.Email,
						Id = blog.Id,
						IpAddress = blog.IpAddress,
						Subject = blog.Subject,
						UserId = blog.UserId,
					});
				}
			}
			return PartialView(model);
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
