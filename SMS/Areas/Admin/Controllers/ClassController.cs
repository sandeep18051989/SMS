using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;
using EF.Services;

namespace SMS.Areas.Admin.Controllers
{
    public class ClassController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly ISettingService _settingService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly ISMSService _smsService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;

        #endregion Fileds

        #region Constructor

        public ClassController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
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
                var classData = (from tempclasses in _smsService.GetAllClasses() select tempclasses);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    classData = classData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstClasses = classData as Class[] ?? classData.ToArray();
                recordsTotal = lstClasses.Count();
                //Paging     
                var data = lstClasses.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ClassModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Name = x.Name.Trim(),
                            AcadmicYearId = x.AcadmicYearId,
                            CreatedOnString = x.CreatedOn.ToString("U"),
                            ModifiedOnString = x.ModifiedOn.ToString("U"),
                            Id = x.Id,
                            DisplayOrder = x.DisplayOrder
                        }).OrderBy(x => x.DisplayOrder).ToList()
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

        public JsonResult CheckClassNameExists(string name, int? id)
        {
            return new JsonResult()
            {
                Data = _smsService.CheckClassExists(name, id),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public ActionResult LoadDivisionGrid(int id)
        {
            try
            {
                var classData = (from associateddivision in _smsService.GetAllDivisionsByClass(id) select associateddivision).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = classData.Select(x => new DivisionModel()
                        {
                            Id = x.Id,
                            IsActive = x.Division.IsActive,
                            UserId = x.UserId,
                            CreatedOnString = x.Division.CreatedOn.ToString("U"),
                            ModifiedOnString = x.Division.ModifiedOn.ToString("U"),
                            Description = !string.IsNullOrEmpty(x.Division.Description) ? x.Division.Description.Trim() : "",
                            Name = !string.IsNullOrEmpty(x.Division.Name) ? x.Division.Name.Trim() : "",
                            AcadmicYearId = x.Division.AcadmicYearId,
                            DisplayOrder = x.Division.DisplayOrder
                        }).OrderBy(x => x.DisplayOrder).ToList()
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
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            var model = new ClassModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new ClassModel();
            var objClass = _smsService.GetClassById(id);
            if (objClass != null)
            {
                model = objClass.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ClassModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate class, if any
            var checkClass = _smsService.CheckClassExists(model.Name, model.Id);
            if (checkClass)
                ModelState.AddModelError("Name", "A Class with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objClass = _smsService.GetClassById(model.Id);
                if (objClass != null)
                {
                    model.CreatedOn = objClass.CreatedOn;
                    objClass = model.ToEntity(objClass);
                    objClass.ModifiedOn = DateTime.Now;
                    _smsService.UpdateClass(objClass);
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }

            SuccessNotification("Class updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            var model = new ClassModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ClassModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            // Check for duplicate class, if any
            var checkClass = _smsService.CheckClassExists(model.Name, model.Id);
            if (checkClass)
                ModelState.AddModelError("Name", "A Class with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objClass = model.ToEntity();
                objClass.CreatedOn = objClass.ModifiedOn = DateTime.Now;
                objClass.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertClass(objClass);

                SuccessNotification("Class created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objClass.Id });
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteClass(id);

            SuccessNotification("Class deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusClass(id);
            ViewBag.Result = "Class updated Successfully";

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult UpdateDivisionsForClass(int id, int[] divisions)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var user = _userContext.CurrentUser;
            var objClass = _smsService.GetClassById(id);
            if (objClass != null)
            {
                var objDivisions = _smsService.GetDivisionsByClass(id);
                if (divisions != null && divisions.Length > 0)
                {
                    foreach (var divisionid in divisions)
                    {
                        var checkRecords = _smsService.GetClassDivisions(classid: id, divisionid: divisionid);
                        if (checkRecords.Count == 0)
                        {
                            _smsService.InsertClassDivision(new ClassRoomDivision()
                            {
                                ClassId = id,
                                DivisionId = divisionid,
                                ClassRoomId = 1, // Default
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                UserId = user.Id
                            });
                        }
                    }
                }
                else
                {
                    foreach (var record in objDivisions)
                    {
                        var objClassDivisions = _smsService.GetClassDivisions(classid: id, divisionid: record.Id);
                        if (objClassDivisions != null && objClassDivisions.Count > 0)
                        {
                            foreach (var classdiv in objClassDivisions)
                            {
                                _smsService.RemoveDivisionFromClass(id, classdiv.DivisionId.Value);
                            }
                        }
                    }
                }
            }
            ViewBag.Result = "Class updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveClassDivision(int id, int divisionid)
        {
            if (!_permissionService.Authorize("ManageClass"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Class id not found");

            _smsService.RemoveDivisionFromClass(id, divisionid);

            SuccessNotification("Class division removed successfully");
            return new JsonResult()
            {
                Data = true,
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

    }
}
