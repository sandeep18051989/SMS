using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class FeedbackController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IProductService _productService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IFileService _fileService;
		private readonly ITemplateService _templateService;
		private readonly ICustomPageService _customPageService;
		private readonly IFeedbackService _feedbackService;
		private readonly IPermissionService _permissionService;

		#endregion Fileds

		#region Constructor

		public FeedbackController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, ITemplateService templateService, ICustomPageService customPageService, IFeedbackService feedbackService, IPermissionService permissionService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._productService = productService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._fileService = fileService;
			this._templateService = templateService;
			this._customPageService = customPageService;
			this._feedbackService = feedbackService;
			this._permissionService = permissionService;
		}

		#endregion

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new List<FeedbackModel>();
			var lstFeedbacks = _feedbackService.GetFeedbacks().OrderByDescending(x => x.CreatedOn).ToList();
			if (lstFeedbacks.Count > 0)
			{
				foreach (var feed in lstFeedbacks)
				{
					var feedModel = new FeedbackModel
					{
						Description = feed.Description,
						EmailAddress = feed.Email,
						ContactNumber = feed.Contact,
						Id = feed.Id,
						Location = feed.Location,
						Date = feed.CreatedOn,
						Name = feed.FullName
					};
					model.Add(feedModel);
				}
			}
			return View(model);

		}

		[HttpPost]
		public ActionResult DeleteSelected(ICollection<int> selectedIds)
		{
			if (selectedIds != null)
			{

				_feedbackService.DeleteQueries(_feedbackService.GetQueriesByIds(selectedIds.ToArray()).ToList());
			}

			SuccessNotification("Records deleted successfully.");
			return RedirectToAction("List");
		}

	}
}
