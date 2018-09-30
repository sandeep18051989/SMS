using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using EF.Services;
using TrackerEnabledDbContext.Common.Models;
using EF.Core;

namespace SMS.Areas.Admin.Controllers
{
    public class AuditController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly ISystemLogService _systemLogService;
        private readonly IAuditService _auditService;

        #endregion Fileds

        #region Constructor

        public AuditController(IUserService userService, IUserContext userContext, ISettingService settingService, IPermissionService permissionService, ISystemLogService systemLogService, IAuditService auditService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._systemLogService = systemLogService;
            this._auditService = auditService;
        }

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageAudits"))
                return AccessDeniedView();

            var model = new AuditModelListModel();

            // Get All Entities
            var AllEntities = FindSubClassesOf<BaseEntity>();

            model.Entities.Add(new SelectListItem { Text = "-- Select Entity --", Value = "0", Selected = true });
            if (AllEntities.Count() > 0)
            {
                foreach (var cls in AllEntities)
                {
                    if (cls.Name.ToLower() != "feedbacks" && cls.Name.ToLower() != "installdatabase" && cls.Name.ToLower() != "irepository" && cls.Name.ToLower() != "iusercontext" && cls.Name.ToLower() != "replies" && cls.Name.ToLower() != "comments" && cls.Name.ToLower() != "scheduletask" && cls.Name.ToLower() != "slider" && cls.Name.ToLower() != "systemlog" && cls.Name.ToLower() != "videos" && cls.Name.ToLower() != "pictures" && cls.Name.ToLower() != "files")
                        model.Entities.Add(new SelectListItem { Text = cls.Name, Value = cls.FullName });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult List(AuditModelListModel model)
        {
            if (!_permissionService.Authorize("ManageAudits"))
                return AccessDeniedView();

            if (String.IsNullOrEmpty(model.SelectedEntityName))
                throw new Exception("Entity name not found.");

            var user = _userContext.CurrentUser;
            var lstAuditLogs = _auditService.GetAllAudits(model.SelectedEntityName);
            if (lstAuditLogs.Count > 0)
            {
                foreach (var eve in lstAuditLogs)
                {
                    model.AuditLogs.Add(new AuditLog
                    {
                        AuditLogId = eve.AuditLogId,
                        EventDateUTC = eve.EventDateUTC,
                        EventType = eve.EventType,
                        LogDetails = eve.LogDetails,
                        Metadata = eve.Metadata,
                        RecordId = eve.RecordId,
                        TypeFullName = eve.TypeFullName,
                        UserName = eve.UserName
                    });
                }
            }

            // Get All Entities
            model.Entities.Add(new SelectListItem { Text = "-- Select Entity --", Value = "0", Selected = true });
            var AllEntities = FindSubClassesOf<BaseEntity>();
            if (AllEntities.Count() > 0)
            {
                foreach (var cls in AllEntities)
                {
                    if (cls.Name.ToLower() != "feedbacks" && cls.Name.ToLower() != "installdatabase" && cls.Name.ToLower() != "irepository" && cls.Name.ToLower() != "iusercontext" && cls.Name.ToLower() != "replies" && cls.Name.ToLower() != "comments" && cls.Name.ToLower() != "scheduletask" && cls.Name.ToLower() != "slider" && cls.Name.ToLower() != "systemlog" && cls.Name.ToLower() != "videos" && cls.Name.ToLower() != "pictures" && cls.Name.ToLower() != "files")
                        model.Entities.Add(new SelectListItem { Text = cls.Name, Value = cls.FullName });
                }
            }

            model.Entities = model.Entities.OrderBy(x => x.Text).ToList();

            model.Entities.ToList().ForEach(u => u.Selected = false);
            model.Entities.FirstOrDefault(ss => ss.Value == model.SelectedEntityName).Selected = true;

            return View(model);
        }

        public IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var baseType = typeof(TBaseType);
            var assembly = baseType.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }


    }
}
