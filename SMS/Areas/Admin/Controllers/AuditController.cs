using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using EF.Services;
using TrackerEnabledDbContext.Common.Models;
using EF.Core;
using SMS.Models;

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

        #region Utilities

        public ActionResult LoadGrid(string entityname=null)
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

                if (string.IsNullOrEmpty(entityname))
                {
                    //Returning Json Data 
                    return new JsonResult()
                    {
                        Data = new
                        {
                            draw = draw,
                            recordsFiltered = recordsTotal,
                            recordsTotal = recordsTotal,
                            data = new List<AuditListModel>()
                        },
                        ContentEncoding = Encoding.Default,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }

                // Getting all data    
                var auditData = (from tempaudits in _auditService.GetAllAudits(entityname.Trim()) select tempaudits);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    auditData = auditData.Where(m => m.TypeFullName.Contains(searchValue) || m.UserName.Contains(searchValue));
                }

                //total number of rows count     
                var auditLogs = auditData as AuditLog[] ?? auditData.ToArray();
                recordsTotal = auditLogs.Count();
                //Paging     
                var data = auditLogs.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new AuditModel()
                        {
                            UserName = x.UserName.Trim(),
                            AuditLogId = x.AuditLogId,
                            EntityName = x.TypeFullName,
                            TypeFullName = x.TypeFullName,
                            EventDateString = x.EventDateUTC.ToString("U"),
                            EventType = x.EventType,
                            RecordId = x.RecordId,
                            LogDetails = x.LogDetails.Select(z => new AuditLogDetail()
                            {
                                Id = z.Id,
                                AuditLogId = z.AuditLogId,
                                NewValue = z.NewValue,
                                OriginalValue = z.OriginalValue,
                                PropertyName = z.PropertyName
                            }).ToList()
                        }).OrderByDescending(y => y.EventDateUTC).ToList()
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
            if (!_permissionService.Authorize("ManageAudits"))
                return AccessDeniedView();

            var model = new AuditListModel();

            // Get All Entities
            var allEntities = FindSubClassesOf<BaseEntity>();
            var enumerable = allEntities as Type[] ?? allEntities.ToArray();
            if (enumerable.Any())
            {
                foreach (var cls in enumerable)
                {
                    if (cls.Name.ToLower() != "feedbacks" && cls.Name.ToLower() != "installdatabase" && cls.Name.ToLower() != "irepository" && cls.Name.ToLower() != "iusercontext" && cls.Name.ToLower() != "replies" && cls.Name.ToLower() != "comments" && cls.Name.ToLower() != "scheduletask" && cls.Name.ToLower() != "slider" && cls.Name.ToLower() != "systemlog" && cls.Name.ToLower() != "videos" && cls.Name.ToLower() != "pictures" && cls.Name.ToLower() != "files")
                        model.Entities.Add(new SelectListItem { Text = cls.Name, Value = cls.FullName });
                }
            }

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
