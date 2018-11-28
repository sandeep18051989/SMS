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
	public class DataTokenController : AdminAreaController
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
		private readonly IPermissionService _permissionService;

		#endregion Fileds

		#region Constructor

		public DataTokenController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, ITemplateService templateService, ICustomPageService customPageService, IPermissionService permissionService)
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
			this._permissionService = permissionService;
		}

        #endregion

        #region Utilities

        [HttpPost]
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
	            var datatokensData = (from tempdatatokens in _templateService.GetAllDataTokens(showSystemDefined: false) select tempdatatokens);

	            //Search    
	            if (!string.IsNullOrEmpty(searchValue))
	            {
	                datatokensData = datatokensData.Where(m => m.Name.Contains(searchValue) || m.SystemName.Contains(searchValue));
	            }

	            //total number of rows count     
	            var customDataTokens = datatokensData as DataToken[] ?? datatokensData.ToArray();
	            recordsTotal = customDataTokens.Count();
	            //Paging     
	            var data = customDataTokens.Skip(skip).Take(pageSize);

	            //Returning Json Data 
	            return new JsonResult()
	            {
	                Data = new
	                {
	                    draw = draw,
	                    recordsFiltered = recordsTotal,
	                    recordsTotal = recordsTotal,
	                    data = data.Select(x => x.ToModel()).OrderBy(x => x.Name).ToList()
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
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			var model = new DataTokenModel();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			var model = new CreateTemplateModel.CreateDataTokensModel();
			var user = _userContext.CurrentUser;
			if (id == 0)
				throw new Exception("Data Token Id Missing");

			var temp = _templateService.GetDataTokenById(id);

			model = new CreateTemplateModel.CreateDataTokensModel
			{
				Name = temp.Name,
				SystemName = temp.SystemName,
				Value = temp.Value,
				Id = temp.Id,
				UserId = temp.UserId,
				IsSystemDefined = temp.IsSystemDefined
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(CreateTemplateModel.CreateDataTokensModel model)
		{
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			// Check for duplicate tokens, if any
			var token = _templateService.GetDataTokenByName(model.Name);
			if (token != null && token.Id != model.Id)
				ModelState.AddModelError("Name", "A Data Token with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var temp = _templateService.GetDataTokenById(model.Id);
				if (temp != null)
				{
					temp.CreatedOn = DateTime.Now;
					temp.Name = model.Name;
					temp.SystemName = model.SystemName;
					temp.Value = model.Value;
					temp.Id = model.Id;
					temp.ModifiedOn = DateTime.Now;
					temp.IsSystemDefined = model.IsSystemDefined;
					_templateService.Update(temp);
					return RedirectToAction("DataTokens");

				}
			}
			else
			{
				ErrorNotification("An error occured while creating event. Please try again.");
				return View(model);
			}

			SuccessNotification("Data token updated successfully");
			return RedirectToAction("DataTokens");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			var model = new CreateTemplateModel.CreateDataTokensModel();
			return View(model);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(CreateTemplateModel.CreateDataTokensModel model)
		{
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			// Check for duplicate tokens, if any
			var checktoken = _templateService.GetDataTokenByName(model.Name);
			if (checktoken != null)
				ModelState.AddModelError("Name", "A Data Token with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var token = new DataToken
				{
					Value = model.Value,
					Name = model.Name,
					SystemName = model.SystemName,
					UserId = user.Id,
					IsDeleted = false,
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now,
					IsSystemDefined = model.IsSystemDefined
				};
				_templateService.Insert(token);
			}
			else
			{
				ErrorNotification("An error occured while creating event. Please try again.");
				return View(model);
			}

			SuccessNotification("Data Token saved successfully.");
			return RedirectToAction("DataTokens");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageDataTokens"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			if (!_templateService.GetDataTokenById(id).IsSystemDefined)
			{
				_templateService.DeleteToken(id);
			}

			SuccessNotification("Data Token deleted successfully.");
			return RedirectToAction("DataTokens");
		}
	}

}
