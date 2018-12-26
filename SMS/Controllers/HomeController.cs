using System.Web.Mvc;
using System.Configuration;
using System.Linq;
using EF.Services.Service;
using SMS.Models;
using System.Data.Entity;
using EF.Data;
using System.Web;
using System.IO;
using System;
using EF.Core.Data;
using System.Web.Configuration;
using EF.Core.Enums;
using System.Data.SqlClient;
using System.Threading;
using EF.Core;
using EF.Services.Http;
using MaxMind.GeoIP2;
using GoogleAnalyticsTracker.MVC5;
using System.Collections.Generic;

namespace SMS.Controllers
{
	public class HomeController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly ISettingService _settingService;
		private readonly IUserContext _userContext;
		private readonly IFeedbackService _feedbackService;
		private readonly IRoleService _roleService;
		private readonly IPermissionService _permissionService;
		private readonly ISliderService _sliderService;
		private readonly ITemplateService _templateService;
		private readonly IEmailService _emailService;
		private readonly IUrlHelper _urlHelper;

		#endregion Fileds

		#region Const

		public HomeController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISettingService settingService, IFeedbackService feedbackService, IRoleService roleService, IPermissionService permissionService, ISliderService sliderService, ITemplateService templateService, IEmailService emailService, IUrlHelper urlHelper)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._settingService = settingService;
			this._feedbackService = feedbackService;
			this._roleService = roleService;
			this._permissionService = permissionService;
			this._sliderService = sliderService;
			this._templateService = templateService;
			this._emailService = emailService;
			this._urlHelper = urlHelper;
		}

		#endregion

		#region Utilities

		[NonAction]
		protected bool SqlServerDatabaseExists(string connectionString)
		{
			try
			{
				//just try to connect
				using (var conn = new SqlConnection(connectionString))
				{
					conn.Open();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		[NonAction]
		protected string CreateDatabase(string connectionString, string collation, int triesToConnect = 10)
		{
			try
			{
				//parse database name
				var builder = new SqlConnectionStringBuilder(connectionString);
				var databaseName = builder.InitialCatalog;
				//now create connection string to 'master' dabatase. It always exists.
				builder.InitialCatalog = "master";
				var masterCatalogConnectionString = builder.ToString();
				string query = string.Format("CREATE DATABASE [{0}]", databaseName);
				if (!String.IsNullOrWhiteSpace(collation))
					query = string.Format("{0} COLLATE {1}", query, collation);
				using (var conn = new SqlConnection(masterCatalogConnectionString))
				{
					conn.Open();
					using (var command = new SqlCommand(query, conn))
					{
						command.ExecuteNonQuery();
					}
				}

				//try connect
				if (triesToConnect > 0)
				{
					//Sometimes on slow servers (hosting) there could be situations when database requires some time to be created.
					//But we have already started creation of tables and sample data.
					//As a result there is an exception thrown and the installation process cannot continue.
					//That's why we are in a cycle of "triesToConnect" times trying to connect to a database with a delay of one second.
					for (var i = 0; i <= triesToConnect; i++)
					{
						if (i == triesToConnect)
							throw new Exception("Unable to connect to the new database. Please try one more time");

						if (!this.SqlServerDatabaseExists(connectionString))
							Thread.Sleep(1000);
						else
							break;
					}
				}

				return string.Empty;
			}
			catch (Exception ex)
			{
				return string.Format(ex.Message);
			}
		}

		#endregion

		#region Home
		public ActionResult Index()
		{
			var getLogo = _pictureService.GetHomeLogo();
			var model = new HomeHeaderModel();
			if (getLogo != null && !string.IsNullOrEmpty(getLogo.Url))
			{
				model.LogoEnabled = true;
				model.pictures = new EF.Core.Data.Picture
				{
					AlternateText = getLogo.AlternateText,
					CreatedOn = getLogo.CreatedOn,
					Height = getLogo.Height,
					Id = getLogo.Id,
					IsActive = getLogo.IsActive,
					IsLogo = getLogo.IsLogo,
					IsThumb = getLogo.IsThumb,
					Size = getLogo.Size,
					Url = getLogo.Url,
					UserId = getLogo.UserId,
					Width = getLogo.Width
				};
			}

			var _user = _userContext.CurrentUser;

			return View(model);
		}

		#endregion

		//[ChildActionOnly]
		public ActionResult Logo()
		{
			var model = new HomeHeaderModel();
			var Logo = _pictureService.GetHomeLogo();
			if (Logo != null)
			{
				model.LogoEnabled = true;
				model.pictures = new EF.Core.Data.Picture
				{
					AlternateText = Logo.AlternateText,
					CreatedOn = Logo.CreatedOn,
					Height = Logo.Height,
					Id = Logo.Id,
					IsActive = Logo.IsActive,
					IsLogo = Logo.IsLogo,
					IsThumb = Logo.IsThumb,
					Size = Logo.Size,
					Url = Logo.Url,
					UserId = Logo.UserId,
					Width = Logo.Width
				};
			}

			return PartialView("_logo", model);
		}

		public ActionResult Query()
		{
			var model = new QueryModel();
			var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
			if (_settings.Count > 0)
			{
				foreach (var setting in _settings)
				{
					if (setting.Name == "Host")
					{
						model.EmailSettings.Host = setting.Value;
					}
					if (setting.Name == "Password")
					{
						model.EmailSettings.Password = setting.Value;
					}
					if (setting.Name == "EnableSSL")
					{
						model.EmailSettings.EnableSSL = setting.Value.ToLower() == "true" ? true : false;
					}
					if (setting.Name == "UseDefaultCredentials")
					{
						model.EmailSettings.UseDefaultCredentials = setting.Value.ToLower() == "true" ? true : false;
					}
					if (setting.Name == "Port")
					{
						model.EmailSettings.Port = Convert.ToInt32(setting.Value);
					}
					if (setting.Name == "Username")
					{
						model.EmailSettings.Username = setting.Value;
					}

					if (setting.Name == "FromEmail")
					{
						model.EmailSettings.FromEmail = setting.Value;
					}
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Query(QueryModel model)
		{
			var user = _userContext.CurrentUser;
			if (ModelState.IsValid)
			{
				var feedBack = new Feedback();
				feedBack.FullName = model.FullName;
				feedBack.ModifiedOn = DateTime.Now;
				feedBack.CreatedOn = DateTime.Now;
				feedBack.Email = model.Email;
				feedBack.UserId = user != null ? user.Id : 0;
				feedBack.Description = model.Description;
				feedBack.Contact = model.Contact;
				_feedbackService.Insert(feedBack);

				model = new QueryModel();
				model.Result = "Your request sent successfuly. We will get back to you soon.";

				var settingTeplate = _settingService.GetSettingByKey("VisitorQueryPlaced");
				var Template = _templateService.GetTemplateByName(settingTeplate.Value);
				if (Template != null)
				{
                    var tokens = new List<DataToken>();
                    _templateService.AddFeedbackTokens(tokens, feedBack);
                    foreach (var dt in tokens)
                    {
                        Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.SystemName + "]", dt.Value, StringComparison.InvariantCulture);
                    }

                    foreach (var dt in _templateService.GetAllDataTokensByTemplate(Template.Id).Where(x => x.IsActive).ToList())
					{
						Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.SystemName + "]", dt.Value, StringComparison.InvariantCulture);
					}
				}

				_emailService.SendMail(feedBack.Email, "SMS", Template != null ? Template.BodyHtml : "Thanks For Sending Us The Request.");

				// Get Email Settings
				var _setting = _settingService.GetSettingByKey("FromEmail");
				if (_setting != null)
				{
					if (!String.IsNullOrEmpty(_setting.Value))
						_emailService.SendMail(_setting.Value, "Feedback", feedBack.FullName + " Sends You Query :" + "<br/>" + feedBack.Description);
				}

				return View(model);
			}

			return View(model);
		}

	}
}
