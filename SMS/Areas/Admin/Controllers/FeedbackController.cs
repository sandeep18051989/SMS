using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Mappers;
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

	    #region Utilities

	    public ActionResult LoadGrid()
	    {
	        try
	        {
	            var draw = Request.Form.GetValues("draw").FirstOrDefault();
	            var start = Request.Form.GetValues("start").FirstOrDefault();
	            var length = Request.Form.GetValues("length").FirstOrDefault();
	            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
	            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();
	            var searchValue = Request.Form.GetValues("search[value]")?.FirstOrDefault();

	            //Paging Size (10,20,50,100)    
	            int pageSize = length != null ? Convert.ToInt32(length) : 0;
	            int skip = start != null ? Convert.ToInt32(start) : 0;
	            int recordsTotal = 0;

	            // Getting all data    
	            var feedbackData = (from tempfeedbacks in _feedbackService.GetFeedbacks() select tempfeedbacks);

	            //Search    
	            if (!string.IsNullOrEmpty(searchValue))
	            {
	                feedbackData = feedbackData.Where(m => m.FullName.Contains(searchValue) || m.Description.Contains(searchValue) || m.Email.Contains(searchValue) || m.Location.Contains(searchValue));
	            }

	            //total number of rows count     
	            var lstFeedbacks = feedbackData as Feedback[] ?? feedbackData.ToArray();
	            recordsTotal = lstFeedbacks.Count();
	            //Paging     
	            var data = lstFeedbacks.Skip(skip).Take(pageSize);

	            //Returning Json Data 
	            return new JsonResult()
	            {
	                Data = new
	                {
	                    draw = draw,
	                    recordsFiltered = recordsTotal,
	                    recordsTotal = recordsTotal,
	                    data = data.Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList()
	                },
	                ContentEncoding = Encoding.Default,
	                ContentType = "application/json",
	                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
	                MaxJsonLength = int.MaxValue
	            };
	        }
	        catch (Exception ex)
	        {
	            throw new Exception(ex.Message);
	        }

	    }

	    #endregion

        public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new FeedbackModel();
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
