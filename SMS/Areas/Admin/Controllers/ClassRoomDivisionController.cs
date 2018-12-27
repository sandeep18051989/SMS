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
    public class ClassRoomDivisionController : AdminAreaController
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

        public ClassRoomDivisionController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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

        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var model = _smsService.GetAllClasses().Select(x => new AllotClassRoomsToClass()
            {
                AvailableDivisions = _smsService.GetAllDivisionsByClass(x.Id).Select(y => new DivisionModel()
                {
                    Name = y.Division.Name,
                    Id = y.DivisionId.Value,
                    AvailableClassRooms = _smsService.GetAllClassRooms().Select(z => new SelectListItem()
                    {
                        Text = z.Number,
                        Value = z.Id.ToString(),
                        Selected = y.ClassRoomId.HasValue && y.ClassRoomId.Value == z.Id
                    }).Where(z => ((y.ClassRoomId.HasValue && y.ClassRoomId.Value.ToString().Trim() == z.Value.Trim()) || (!_smsService.CheckClassRoomAlreadyAssociatedToOtherDivisionAndClass(classroomid: Convert.ToInt32(z.Value), classid: x.Id, divisionid: y.DivisionId)))).ToList()
                }).ToList(),
                Class = x.Name,
                ClassId = x.Id
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult List(IList<AllotClassRoomsToClass> model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var classRoomUpdations = frm.AllKeys.Where(pm => pm.StartsWith("classroom_")).ToList();
            if (classRoomUpdations.Count > 0)
            {
                int count = 0;
                int classid = 0;
                int divisionid = 0;
                foreach (var key in classRoomUpdations)
                {
                    count += 1;
                    classid = Convert.ToInt32(key.Split('_')[1]);
                    divisionid = Convert.ToInt32(key.Split('_')[2]);
                    if (classid > 0 && divisionid > 0 && frm["classroom_" + classid + "_" + divisionid] != null && !string.IsNullOrEmpty(frm["classroom_" + classid + "_" + divisionid].ToString()))
                    {
                        var selectedValue = Convert.ToInt32(frm["classroom_" + classid + "_" + divisionid].ToString());
                        if (selectedValue > 0)
                        {
                            var selectedClassroomDivision = _smsService.GetClassDivisions(classid: classid, divisionid: divisionid).FirstOrDefault();
                            if (selectedClassroomDivision != null)
                            {
                                selectedClassroomDivision.ClassRoomId = selectedValue;
                                selectedClassroomDivision.ModifiedOn = DateTime.Now;
                                _smsService.UpdateClassDivision(selectedClassroomDivision);
                            }
                            else
                            {
                                selectedClassroomDivision = new ClassRoomDivision();
                                selectedClassroomDivision.ClassId = classid;
                                selectedClassroomDivision.DivisionId = divisionid;
                                selectedClassroomDivision.ClassRoomId = selectedValue;
                                selectedClassroomDivision.CreatedOn = selectedClassroomDivision.ModifiedOn = DateTime.Now;
                                selectedClassroomDivision.UserId = _userContext.CurrentUser.Id;
                                _smsService.InsertClassDivision(selectedClassroomDivision);
                            }
                        }
                    }
                }
            }

            SuccessNotification("Class Rooms updated successfully.");
            return RedirectToAction("List");
        }


    }
}
