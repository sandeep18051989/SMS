using System;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class FeedbackController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IEventService _eventService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IFeedbackService _feedbackService;
		private readonly IEmailService _emailService;
		private readonly ITemplateService _templateService;
		private readonly ICustomPageService _customPageService;

		#endregion Fileds

		#region Constructor

		public FeedbackController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IEventService eventService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFeedbackService feedbackService, IEmailService emailService, ITemplateService templateService, ICustomPageService customPageService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._eventService = eventService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._templateService = templateService;
			this._customPageService = customPageService;
			this._feedbackService = feedbackService;
			this._emailService = emailService;
		}

		#endregion

		// GET: Event
		public ActionResult Index()
		{
			var model = new FeedbackModel();
			return View(model);
		}

		// POST: Feedback/Create
		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Index(FeedbackModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					_feedbackService.Insert(new Feedback()
					{
						Contact = model.Contact,
						CreatedOn = DateTime.Now,
						Description = model.Description,
						Email = model.Email,
						FullName = model.FullName,
						Location = model.Location,
						ModifiedOn = DateTime.Now
					});

					var settingTeplate = _settingService.GetSettingByKey("RequestQuote");
					var template = _templateService.GetTemplateByName(settingTeplate.Value);
					if (template != null)
					{
						foreach (var dt in _templateService.GetAllDataTokensByTemplate(template.Id).Where(x => x.IsActive).ToList())
						{
							template.BodyHtml = EF.Core.CodeHelper.Replace(template.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
						}
					}

					model.SentSuccess = _emailService.SendMail(model.Email, "SMS", template != null ? template.BodyHtml : "Thanks For Sending Us The Request.");
					if (model.SentSuccess)
					{
						// Refresh Model
						model = new FeedbackModel();
						model.SentSuccess = true;
					}

					return View(model);
				}

				model.SentSuccess = false;
				return View(model);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public ActionResult Query()
		{
			var model = new QueryModel();
			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Query(string name, string description, string email, string contact, string location)
		{
			bool result = false;
			try
			{
				if (ModelState.IsValid)
				{
					_feedbackService.Insert(new Feedback()
					{
						Contact = contact,
						CreatedOn = DateTime.Now,
						Description = description,
						Email = email,
						FullName = name,
						ModifiedOn = DateTime.Now,
						Location = location
					});

					var Template = _templateService.GetAllTemplates(true).Where(x => x.Name.Contains("VisitorQueryPlaced")).FirstOrDefault();
					if (Template != null)
					{
						foreach (var dt in _templateService.GetAllDataTokensByTemplate(Template.Id).Where(x => x.IsActive).ToList())
						{
							Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
						}
					}

					result = _emailService.SendMail(email, "Artery Labs", Template != null ? Template.BodyHtml : "Thanks For Sending Us The Query.");

					// Get Email Settings
					var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
					if (_settings.Count > 0)
					{
						foreach (var setting in _settings)
						{
							if (setting.Name == "FromEmail")
							{
								if (!String.IsNullOrEmpty(setting.Value))
									_emailService.SendMail(setting.Value, "Feedback", name + " Send You A Query :" + "<br/>" + description);

								break;
							}
						}
					}

					if (result)
						return Json(new { Success = "True", Message = "Query Sent Succesfully." });
				}

				if (!result)
					return Json(new { Success = "False", Message = "An error occured while sending your query. You may contact us on our contact numbers." });
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}

			return Json(new { Success = "False", Message = "An error occured while sending your query. You may contact us on our contact numbers." });
		}

	}
}
