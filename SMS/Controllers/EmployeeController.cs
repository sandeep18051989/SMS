using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class EmployeeController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly ISMSService _smsService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;

		#endregion Fileds

		#region Constructor

		public EmployeeController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._smsService = smsService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
		}

		#endregion

		public ActionResult Index()
		{
			var model = new List<TeacherModel>();
			var lstTeachers = _smsService.SearchTeachers(true, 0, 0, 0, null).ToList();
			if (lstTeachers.Count > 0)
			{
				foreach (var t in lstTeachers)
				{
					var e = _smsService.GetEmployeeById(t.EmployeeId);

					model.Add(new TeacherModel()
					{
						AcadmicYear = t.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(t.AcadmicYearId) : null,
						Subjects = t.Subjects.Select(s => new SubjectModel()
						{
							Id = s.Id,
							Name = s.Name
						}).ToList(),
						Name = t.Name,
						Id = t.Id,
						Employee = e != null ? new EmployeeModel()
						{
							AadharCardNo = !String.IsNullOrEmpty(e.AadharCardNo) ? e.AadharCardNo : "",
							AcadmicYear = t.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(t.AcadmicYearId) : null,
							EmployeePicture = new PictureModel()
							{
								AlternateText = !String.IsNullOrEmpty(e.EmployeePicture.AlternateText) ? e.EmployeePicture.AlternateText : "",
								IsActive = e.EmployeePicture.IsActive,
								Id = e.EmployeePicture.Id,
								Src = e.EmployeePicture.PictureSrc,
								Url = e.EmployeePicture.Url
							}
						} : null,
						FacebookLink = !String.IsNullOrEmpty(t.FacebookLink) ? t.FacebookLink : "",
						GooglePlusLink = !String.IsNullOrEmpty(t.GooglePlusLink) ? t.GooglePlusLink : "",
						Hi5Link = !String.IsNullOrEmpty(t.Hi5Link) ? t.Hi5Link : "",
						TweeterLink = !String.IsNullOrEmpty(t.TweeterLink) ? t.TweeterLink : "",
						InstagramLink = !String.IsNullOrEmpty(t.InstagramLink) ? t.InstagramLink : "",
						LinkedInLink = !String.IsNullOrEmpty(t.LinkedInLink) ? t.LinkedInLink : ""

					});
				}
			}
			return PartialView(model);
		}

		// GET: Teacher/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Teacher/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Teacher/Create
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

		// POST: Teacher/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Teacher/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Teacher/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
