using System;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class EmailController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        public readonly ISettingService _settingService;
        public readonly IPermissionService _permissionService;

        #endregion Fileds

        #region Constructor

        public EmailController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IPermissionService permissionService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._permissionService = permissionService;
        }

        #endregion

        public ActionResult EmailSettings()
        {
            if (!_permissionService.Authorize("ManageConfiguration"))
                return AccessDeniedView();

            var model = new EmailSettingsModel();
            model.ActiveSettings = "EmailSettings";

            var emailSettings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting).ToList();
            if (emailSettings.Count > 0)
            {
                foreach (var setting in emailSettings)
                {
                    if (setting.Name == "Host")
                    {
                        model.Host = setting.Value;
                    }
                    if (setting.Name == "Password")
                    {
                        model.Password = setting.Value;
                    }
                    if (setting.Name == "EnableSSL")
                    {
                        model.EnableSSL = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name == "UseDefaultCredentials")
                    {
                        model.UseDefaultCredentials = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name == "Port")
                    {
                        model.Port = Convert.ToInt32(setting.Value);
                    }
                    if (setting.Name == "Username")
                    {
                        model.Username = setting.Value;
                    }
                    if (setting.Name == "FromEmail")
                    {
                        model.FromEmail = setting.Value;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailSettings(EmailSettingsModel model)
        {
            if (!_permissionService.Authorize("ManageConfiguration"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            if (ModelState.IsValid)
            {
                var emailSettings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting).ToList();
                if (emailSettings.Count > 0)
                {
                    foreach (Settings setting in emailSettings)
                    {
                        setting.ModifiedOn = DateTime.Now;
                        setting.UserId = user.Id;
                        if (setting.Name == "Host")
                        {
                            setting.Value = model.Host;
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "Password")
                        {
                            setting.Value = model.Password;
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "EnableSSL")
                        {
                            setting.Value = model.EnableSSL == true ? "true" : "false";
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "UseDefaultCredentials")
                        {
                            setting.Value = model.UseDefaultCredentials == true ? "true" : "false";
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "Port")
                        {
                            setting.Value = model.Port.ToString();
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "Username")
                        {
                            setting.Value = model.Username;
                            _settingService.Update(setting);
                        }
                        if (setting.Name == "FromEmail")
                        {
                            setting.Value = model.FromEmail;
                            _settingService.Update(setting);
                        }
                    }
                }
                else
                {
                    // Host
                    var _incServersetting = new Settings();
                    _incServersetting.EntityId = 0;
                    _incServersetting.Value = model.Host;
                    _incServersetting.Name = "Host";
                    _incServersetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incServersetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incServersetting.Entity = "Email";
                    _incServersetting.user = user;
                    _incServersetting.UserId = user.Id;
                    _incServersetting.CreatedOn = DateTime.Now;
                    _incServersetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incServersetting);

                    // Password
                    var _incPasswordSetting = new Settings();
                    _incPasswordSetting.EntityId = 0;
                    _incPasswordSetting.Value = model.Password;
                    _incPasswordSetting.Name = "Password";
                    _incPasswordSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incPasswordSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incPasswordSetting.Entity = "Email";
                    _incPasswordSetting.user = user;
                    _incPasswordSetting.UserId = user.Id;
                    _incPasswordSetting.CreatedOn = DateTime.Now;
                    _incPasswordSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incPasswordSetting);

                    // EnableSSL
                    var _incRequireSSLSetting = new Settings();
                    _incRequireSSLSetting.EntityId = 0;
                    _incRequireSSLSetting.Value = model.EnableSSL ? "true" : "false";
                    _incRequireSSLSetting.Name = "EnableSSL";
                    _incRequireSSLSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incRequireSSLSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incRequireSSLSetting.Entity = "Email";
                    _incRequireSSLSetting.user = user;
                    _incRequireSSLSetting.UserId = user.Id;
                    _incRequireSSLSetting.CreatedOn = DateTime.Now;
                    _incRequireSSLSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incRequireSSLSetting);

                    // UseDefaultCredentials
                    var _incSMTPAuthenticationSetting = new Settings();
                    _incSMTPAuthenticationSetting.EntityId = 0;
                    _incSMTPAuthenticationSetting.Value = model.UseDefaultCredentials ? "true" : "false";
                    _incSMTPAuthenticationSetting.Name = "UseDefaultCredentials";
                    _incSMTPAuthenticationSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incSMTPAuthenticationSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incSMTPAuthenticationSetting.Entity = "Email";
                    _incSMTPAuthenticationSetting.user = user;
                    _incSMTPAuthenticationSetting.UserId = user.Id;
                    _incSMTPAuthenticationSetting.CreatedOn = DateTime.Now;
                    _incSMTPAuthenticationSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incSMTPAuthenticationSetting);

                    // Port
                    var _incSMTPportSetting = new Settings();
                    _incSMTPportSetting.EntityId = 0;
                    _incSMTPportSetting.Value = model.Port.ToString();
                    _incSMTPportSetting.Name = "Port";
                    _incSMTPportSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incSMTPportSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incSMTPportSetting.Entity = "Email";
                    _incSMTPportSetting.user = user;
                    _incSMTPportSetting.UserId = user.Id;
                    _incSMTPportSetting.CreatedOn = DateTime.Now;
                    _incSMTPportSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incSMTPportSetting);

                    // Username
                    var _incUsernameSetting = new Settings();
                    _incUsernameSetting.EntityId = 0;
                    _incUsernameSetting.Value = model.Username;
                    _incUsernameSetting.Name = "Username";
                    _incUsernameSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incUsernameSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incUsernameSetting.Entity = "Email";
                    _incUsernameSetting.user = user;
                    _incUsernameSetting.UserId = user.Id;
                    _incUsernameSetting.CreatedOn = DateTime.Now;
                    _incUsernameSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incUsernameSetting);

                    // FromEmail
                    var _incFromEmailSetting = new Settings();
                    _incFromEmailSetting.EntityId = 0;
                    _incFromEmailSetting.Value = model.FromEmail;
                    _incFromEmailSetting.Name = "FromEmail";
                    _incFromEmailSetting.SettingType = (int)SettingTypeEnum.EmailSetting;
                    _incFromEmailSetting.TypeId = (int)SettingTypeEnum.EmailSetting;
                    _incFromEmailSetting.Entity = "Email";
                    _incFromEmailSetting.user = user;
                    _incFromEmailSetting.UserId = user.Id;
                    _incFromEmailSetting.CreatedOn = DateTime.Now;
                    _incFromEmailSetting.ModifiedOn = DateTime.Now;
                    _settingService.Insert(_incFromEmailSetting);

                }
            }

            SuccessNotification("Email Settings Saved Successfully.");
            model.ActiveSettings = "EmailSettings";
            return View(model);
        }

    }
}
