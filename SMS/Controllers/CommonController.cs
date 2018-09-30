using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class CommonController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ITemplateService _templateService;
		private readonly ICustomPageService _customPageService;

		#endregion Fileds

		#region Constructor

		public CommonController(IUserService userService, IPictureService pictureService, IUserContext userContext, ITemplateService templateService, ICustomPageService customPageService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._templateService = templateService;
			this._customPageService = customPageService;
		}

		#endregion

		public ActionResult Contact()
		{
			var model = new CustomPageModel();
			var customPage = _customPageService.GetCustomPageByName("Contact");
			if (customPage != null)
			{
				model.BodyHtml = customPage.Template.BodyHtml;
				model.Name = customPage.Name;
				model.Id = customPage.Id;
				model.TemplateId = customPage.TemplateId;
			}

			foreach (var dt in _templateService.GetAllDataTokensByTemplate(model.TemplateId).Where(x => x.IsActive).ToList())
			{
				model.BodyHtml = EF.Core.CodeHelper.Replace(model.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
			}

			return View(model);
		}

		//page not found
		public ActionResult PageNotFound()
		{
			this.Response.StatusCode = 404;
			this.Response.TrySkipIisCustomErrors = true;
			this.Response.ContentType = "text/html";

			return View();
		}

		//robots.txt file
		public ActionResult RobotsTextFile()
		{
			var sb = new StringBuilder();

			//if robots.custom.txt exists, let's use it instead of hard-coded data below
			string robotsFilePath = System.IO.Path.Combine(CommonHelper.MapPath("~/"), "robots.custom.txt");
			if (System.IO.File.Exists(robotsFilePath))
			{
				//the robots.txt file exists
				string robotsFileContent = System.IO.File.ReadAllText(robotsFilePath);
				sb.Append(robotsFileContent);
			}
			else
			{
				var disallowPaths = new List<string>
					 {
						  "/bin/",
						  "/content/",
						  "/install"
					 };
				var localizableDisallowPaths = new List<string>
					 {
						"admin"
					 };


				const string newLine = "\r\n"; //Environment.NewLine
				sb.Append("User-agent: *");
				sb.Append(newLine);

				//usual paths
				foreach (var path in disallowPaths)
				{
					sb.AppendFormat("Disallow: {0}", path);
					sb.Append(newLine);
				}
				// paths
				foreach (var path in localizableDisallowPaths)
				{
					sb.AppendFormat("Disallow: {0}", path);
					sb.Append(newLine);
				}

				//load and add robots.txt additions to the end of file.
				string robotsAdditionsFile = System.IO.Path.Combine(CommonHelper.MapPath("~/"), "robots.additions.txt");
				if (System.IO.File.Exists(robotsAdditionsFile))
				{
					string robotsFileContent = System.IO.File.ReadAllText(robotsAdditionsFile);
					sb.Append(robotsFileContent);
				}
			}


			Response.ContentType = MimeTypes.TextPlain;
			Response.Write(sb.ToString());
			return null;
		}

		public ActionResult About()
		{
			var model = new CustomPageModel();
			var customPage = _customPageService.GetCustomPageByName("About");
			if (customPage != null)
			{
				model.BodyHtml = customPage.Template.BodyHtml;
				model.Name = customPage.Name;
				model.Id = customPage.Id;
				model.TemplateId = customPage.TemplateId;
			}

			foreach (var dt in _templateService.GetAllDataTokensByTemplate(model.TemplateId).Where(x => x.IsActive).ToList())
			{
				model.BodyHtml = EF.Core.CodeHelper.Replace(model.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
			}

			return View(model);
		}

		public ActionResult ThirdPartyManufacturing()
		{
			var model = new CustomPageModel();
			var customPage = _customPageService.GetCustomPageByName("Manufacturing");
			if (customPage != null)
			{
				model.BodyHtml = customPage.Template.BodyHtml;
				model.Name = customPage.Name;
				model.Id = customPage.Id;
				model.TemplateId = customPage.TemplateId;
			}

			foreach (var dt in _templateService.GetAllDataTokensByTemplate(model.TemplateId).Where(x => x.IsActive).ToList())
			{
				model.BodyHtml = EF.Core.CodeHelper.Replace(model.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
			}

			return View(model);
		}

		public ActionResult Pharma()
		{
			var model = new CustomPageModel();
			var customPage = _customPageService.GetCustomPageByName("Pharma");
			if (customPage != null)
			{
				model.BodyHtml = customPage.Template.BodyHtml;
				model.Name = customPage.Name;
				model.Id = customPage.Id;
				model.TemplateId = customPage.TemplateId;
			}

			foreach (var dt in _templateService.GetAllDataTokensByTemplate(model.TemplateId).Where(x => x.IsActive).ToList())
			{
				model.BodyHtml = EF.Core.CodeHelper.Replace(model.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
			}

			return View(model);
		}

		// GET: Common/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Common/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Common/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// To render user links
		[ChildActionOnly]
		public ActionResult UserLinks()
		{
			var user = _userContext.CurrentUser;

			if (user == null)
			{
				var model = new UserLinksModel
				{
					IsAuthenticated = false
				};

				return PartialView(model);
			}
			else
			{
				var model = new UserLinksModel
				{
					IsAuthenticated = true,
					user = user,
					CustomerUsername = user.UserName,
					RoleId = user.Roles.FirstOrDefault().Id,
					RoleName = user.Roles.FirstOrDefault().RoleName
				};

				return PartialView(model);
			}
		}

		[ChildActionOnly]
		public ActionResult CommonLinks()
		{
			var user = _userContext.CurrentUser;

			if (user == null)
			{
				var model = new UserLinksModel
				{
					IsAuthenticated = false
				};

				return PartialView(model);
			}
			else
			{
				var model = new UserLinksModel
				{
					IsAuthenticated = true,
					user = user,
					CustomerUsername = user.UserName,
					RoleId = user.Roles.FirstOrDefault().Id,
					RoleName = user.Roles.FirstOrDefault().RoleName
				};

				return PartialView(model);
			}
		}

		public ActionResult AccessDenied()
		{
			return View();
		}

		public ActionResult GenericUrl()
		{
			return InvokeHttp404();
		}
	}
}
