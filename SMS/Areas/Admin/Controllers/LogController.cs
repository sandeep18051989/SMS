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

namespace SMS.Areas.Admin.Controllers
{
    public class LogController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly ISystemLogService _systemLogService;

        #endregion Fileds

        #region Constructor

        public LogController(IUserService userService, IUserContext userContext, ISettingService settingService, IPermissionService permissionService, ISystemLogService systemLogService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._systemLogService = systemLogService;
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
                var logData = (from templogs in _systemLogService.GetAllSystemLogs() select templogs);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    logData = logData.Where(m => m.EntityTypeName.Contains(searchValue) || m.Message.Contains(searchValue) || m.StackTrace.Contains(searchValue));
                }

                //total number of rows count     
                var lstLogs = logData as SystemLog[] ?? logData.ToArray();
                recordsTotal = lstLogs.Count();
                //Paging     
                var data = lstLogs.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(eve => new SystemLogModel
                        {
                            Id = eve.Id,
                            EntityId = eve.EntityId,
                            EntityType = eve.EntityType,
                            EntityTypeName = eve.EntityTypeName,
                            ErrorId = eve.ErrorId,
                            IpAddress = eve.IpAddress,
                            IsException = eve.IsException,
                            IsFixed = eve.IsFixed,
                            Level = eve.Level,
                            LogLevel = eve.LogLevel,
                            Message = eve.Message,
                            StackTrace = eve.StackTrace,
                            Url = eve.Url,
                            Date = eve.CreatedOn.Value,
                            LogDateString = eve.CreatedOn.Value.ToString("U")
                        }).OrderByDescending(x => x.CreatedOn).ToList()
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
            if (!_permissionService.Authorize("ManageLogs"))
                return AccessDeniedView();

            var model = new SystemLogModel();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageLogs"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            var _logEntry = _systemLogService.GetSystemLogById(id);

            if (_logEntry != null)
                _systemLogService.DeleteLog(_logEntry);

            SuccessNotification("System Log Entry Deleted Successfully.");
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageLogs"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _systemLogService.DeleteLogs(_systemLogService.GetSystemLogByIds(selectedIds.ToArray()).ToList());
            }


            SuccessNotification("System Log Entries Deleted Successfully.");
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ClearLog()
        {
            if (!_permissionService.Authorize("ManageLogs"))
                return AccessDeniedView();

            _systemLogService.ClearLog();

            SuccessNotification("System Log Entries Cleared Successfully.");
            return RedirectToAction("List");
        }

    }
}
