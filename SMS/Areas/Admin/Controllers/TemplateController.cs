using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Models;
using System.Text;

namespace SMS.Areas.Admin.Controllers
{
    public class TemplateController : AdminAreaController
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

        public TemplateController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, ITemplateService templateService, ICustomPageService customPageService, IPermissionService permissionService)
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
                var templateData = (from templates in _templateService.GetAllTemplates() select templates);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    templateData = templateData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstUsers = templateData as Template[] ?? templateData.ToArray();
                recordsTotal = lstUsers.Count();
                //Paging     
                var data = lstUsers.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new TemplateModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Name = x.Name.Trim(),
                            IsSystemDefined = x.IsSystemDefined,
                            dataTokens = x.Tokens.Select(y => new DataTokenModel()
                            {
                                Name = y.Name.Trim()
                            }).ToList(),
                            Id = x.Id
                        }).OrderBy(x => x.Name).ToList()
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

        [HttpGet]
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            var model = new TemplateModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new CreateTemplateModel();
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Template Id Missing");

            var temp = _templateService.GetTemplateById(id);
            // Get All Data Tokens
            var dtTokens = _templateService.GetAllDataTokens().ToList();
            if (temp != null)
            {
                var usedTokens = _templateService.GetAllDataTokensByTemplate(temp.Id);
                model = new CreateTemplateModel
                {
                    InsertDataTokensModel = dtTokens.Select(x => new CreateTemplateModel.CreateDataTokensModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        IsSystemDefined = x.IsSystemDefined,
                        Name = x.Name,
                        Value = x.Value,
                        IsActive = x.IsActive,
                        SystemName = x.SystemName,
                        Selected = usedTokens.Any(y => y.Id == x.Id)
                    }).ToList(),
                    Name = temp.Name,
                    IsActive = temp.IsActive,
                    BodyHtml = temp.BodyHtml,
                    Id = temp.Id,
                    UserId = temp.UserId,
                    ModifiedOn = temp.ModifiedOn,
                    CreatedOn = temp.CreatedOn,
                    IsSystemDefined = temp.IsSystemDefined
                };
            }

            if (model.InsertDataTokensModel == null)
                model.InsertDataTokensModel = new List<CreateTemplateModel.CreateDataTokensModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CreateTemplateModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            // Check for duplicate templates, if any
            var _template = _templateService.GetTemplateByName(model.Name);
            if (_template != null && _template.Id != model.Id)
                ModelState.AddModelError("Name", "A Template with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var temp = _templateService.GetTemplateById(model.Id);
                if (temp != null)
                {
                    if (frm.Keys.Count > 0)
                    {
                        foreach (var k in frm.AllKeys.Where(x => x.Contains("token_")))
                        {
                            var dtTokenId = Convert.ToInt32(frm[k].ToString());
                            if (temp.Tokens.FirstOrDefault(x => x.Id == dtTokenId) == null)
                            {
                                var tkn = _templateService.GetDataTokenById(dtTokenId);
                                if (tkn != null)
                                {
                                    var dtToken = new DataToken()
                                    {
                                        Id = tkn.Id,
                                        IsSystemDefined = tkn.IsSystemDefined,
                                        Name = tkn.Name,
                                        SystemName = tkn.SystemName,
                                        UserId = tkn.UserId,
                                        Value = tkn.Value,
                                        CreatedOn = tkn.CreatedOn,
                                        ModifiedOn = tkn.ModifiedOn,
                                        IsActive = tkn.IsActive,
                                        IsDeleted = false
                                    };
                                    temp.Tokens.Add(dtToken);
                                }
                            }
                        }
                    }

                    var alltokens = _templateService.GetAllDataTokensByTemplate(temp.Id);
                    foreach (DataToken dToken in alltokens)
                    {
                        if (!model.BodyHtml.Contains("[" + dToken.SystemName + "]"))
                        {
                            temp.Tokens.Remove(dToken);
                        }
                    }

                    temp.Name = model.Name;
                    temp.BodyHtml = model.BodyHtml;
                    temp.IsActive = model.IsActive;
                    temp.Id = model.Id;
                    temp.Url = "";
                    temp.ModifiedOn = DateTime.Now;
                    temp.UserId = _userContext.CurrentUser.Id;
                    _templateService.Update(temp);


                }
            }

            SuccessNotification("Template updated successfully.");
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            var model = new CreateTemplateModel();
            // Get All Data Tokens
            var dtTokens = _templateService.GetAllDataTokens().ToList();
            model.InsertDataTokensModel = dtTokens.Select(x => new CreateTemplateModel.CreateDataTokensModel()
            {
                Id = x.Id,
                UserId = x.UserId,
                IsSystemDefined = x.IsSystemDefined,
                Name = x.Name,
                Value = x.Value,
                IsActive = x.IsActive
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CreateTemplateModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            // Check for duplicate templates, if any
            var _template = _templateService.GetTemplateByName(model.Name);
            if (_template != null)
                ModelState.AddModelError("Name", "A Template with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var template = new Template
                {
                    BodyHtml = model.BodyHtml,
                    IsActive = model.IsActive,
                    Name = model.Name,
                    UserId = _userContext.CurrentUser.Id,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Url = "",
                    IsSystemDefined = false
                };

                var dtTokens = _templateService.GetAllDataTokens().ToList();
                if (dtTokens.Count > 0)
                {
                    foreach (DataToken dToken in dtTokens)
                    {
                        if (template.BodyHtml.Contains("[" + dToken.Name + "]"))
                        {
                            var dtToken = new DataToken()
                            {
                                Id = dToken.Id,
                                Name = dToken.Name,
                                UserId = dToken.UserId,
                                Value = dToken.Value,
                                CreatedOn = dToken.CreatedOn,
                                ModifiedOn = dToken.ModifiedOn,
                                IsActive = dToken.IsActive,
                                IsDeleted = false
                            };
                            template.Tokens.Add(dtToken);
                        }
                    }
                }

                _templateService.Insert(template);
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Template created successfully.");
            return RedirectToAction("List");
        }
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            var _isSystemDefine = _templateService.GetTemplateById(id).IsSystemDefined;

            if (!_isSystemDefine)
                _templateService.DeleteTemplate(id);

            SuccessNotification("Template deleted successfully.");
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ToggleTemplate(string id)
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            if (String.IsNullOrEmpty(id))
                throw new Exception("Id Not Found");

            var template = _templateService.GetTemplateById(Convert.ToInt32(id));

            if (template != null)
                _templateService.ToggleTemplate(Convert.ToInt32(id));

            if (template != null && template.IsActive)
            {
                SuccessNotification("Template activated successfully.");
            }
            else
            {
                SuccessNotification("Template de-activated successfully.");
            }
            return View("List");
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageTemplates"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _templateService.DeleteTemplates(_templateService.GetTemplatesByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

    }
}
