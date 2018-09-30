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
						Comments = blog.Comments.Select(x => new CommentModel()
						{
							Id = x.Id,
							CommentHtml = x.CommentHtml,
							CommentId = x.Id,
							CreatedOn = x.CreatedOn,
							ModifiedOn = x.ModifiedOn,
							DisplayOrder = x.DisplayOrder,
							Username = x.Username,
							Replies = _replyService.GetAllRepliesByComment(x.Id).Select(r => new RepliesModel()
							{
								Id = r.Id,
								DisplayOrder = r.DisplayOrder,
								CreatedOn = r.CreatedOn,
								IsModified = r.IsModified,
								ReplyHtml = r.ReplyHtml,
								User = new UserModel()
								{
									Id = r.UserId,
								},
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
						}).OrderByDescending(x => x.CreatedOn).ToList(),
						Pictures = blog.Pictures.Select(p => new BlogPictureModel()
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
						}).OrderBy(p => p.DisplayOrder).ToList(),
						Name = blog.Name,
						BlogHtml = blog.BlogHtml,
						CreatedOn = blog.CreatedOn,
						Email = blog.Email,
						Id = blog.Id,
						IpAddress = blog.IpAddress,
						Subject = blog.Subject,
						UserId = blog.UserId,
						Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(re => new ReactionModel()
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
						}).ToList()
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
					Comments = blog.Comments.Select(x => new CommentModel()
					{
						Id = x.Id,
						CommentHtml = x.CommentHtml,
						CommentId = x.Id,
						CreatedOn = x.CreatedOn,
						ModifiedOn = x.ModifiedOn,
						DisplayOrder = x.DisplayOrder,
						Username = x.Username,
						Replies = _replyService.GetAllRepliesByComment(x.Id).Select(r => new RepliesModel()
						{
							Id = r.Id,
							DisplayOrder = r.DisplayOrder,
							CreatedOn = r.CreatedOn,
							IsModified = r.IsModified,
							ReplyHtml = r.ReplyHtml,
							User = new UserModel()
							{
								Id = r.UserId,
							},
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
					}).OrderByDescending(x => x.CreatedOn).ToList(),
					Pictures = blog.Pictures.Select(p => new BlogPictureModel()
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
					}).OrderBy(p => p.DisplayOrder).ToList(),
					Name = blog.Name,
					BlogHtml = blog.BlogHtml,
					CreatedOn = blog.CreatedOn,
					Email = blog.Email,
					Id = blog.Id,
					IpAddress = blog.IpAddress,
					Subject = blog.Subject,
					UserId = blog.UserId,
					Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(re => new ReactionModel()
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
					}).ToList()
				};
			}
			else
			{
				return RedirectToAction("PageNotFound");
			}

			// Attach Child Model Default Values
			model.postCommentModel.Type = model.postCommentModel.postReplyModel.Type = "Blog";
			model.postCommentModel.EntityId = id;
			model.postCommentModel.Username = user != null ? user.UserName : "";
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
					Comments = blog.Comments.Select(x => new CommentModel()
					{
						Id = x.Id,
						CommentHtml = x.CommentHtml,
						CommentId = x.Id,
						CreatedOn = x.CreatedOn,
						ModifiedOn = x.ModifiedOn,
						DisplayOrder = x.DisplayOrder,
						Username = x.Username,
						Replies = _replyService.GetAllRepliesByComment(x.Id).Select(r => new RepliesModel()
						{
							Id = r.Id,
							DisplayOrder = r.DisplayOrder,
							CreatedOn = r.CreatedOn,
							IsModified = r.IsModified,
							ReplyHtml = r.ReplyHtml,
							User = new UserModel()
							{
								Id = r.UserId,
							},
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
					}).OrderByDescending(x => x.CreatedOn).ToList(),
					Pictures = blog.Pictures.Select(p => new BlogPictureModel()
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
					}).OrderBy(p => p.DisplayOrder).ToList(),
					Name = blog.Name,
					BlogHtml = blog.BlogHtml,
					CreatedOn = blog.CreatedOn,
					Email = blog.Email,
					Id = blog.Id,
					IpAddress = blog.IpAddress,
					Subject = blog.Subject,
					UserId = blog.UserId,
					Reactions = _smsService.SearchReactions(blogid: blog.Id).Select(re => new ReactionModel()
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
					}).ToList()
				};
			}

			// Attach Child Model Default Values
			model.postCommentModel.Type = model.postCommentModel.postReplyModel.Type = "Product";
			model.postCommentModel.EntityId = id;
			model.postCommentModel.Username = user != null ? user.UserName : "";
			return View("~/Views/Blog/Detail.cshtml", model);
		}

	}
}
