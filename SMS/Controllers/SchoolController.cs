using System.Web.Mvc;
using EF.Core;
using System.Linq;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class SchoolController : PublicHttpController
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
		private readonly IEventService _eventService;
		private readonly INewsService _newsService;

		#endregion Fileds

		#region Constructor

		public SchoolController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, INewsService newsService, IEventService eventService)
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
			this._eventService = eventService;
			this._newsService = newsService;
		}

		#endregion

		public ActionResult Details(int id)
		{
			var model = new SchoolModel();
			var school = _smsService.GetSchoolById(id);

			if (school != null)
			{
				var coverpic = school.CoverPictureId > 0 ? _pictureService.GetPictureById(school.CoverPictureId) : null;
				var profilepic = school.ProfilePictureId > 0 ? _pictureService.GetPictureById(school.ProfilePictureId) : null;

				model.Id = school.Id;
				model.AcadmicYear = _smsService.GetAcadmicYearById(school.AcadmicYearId);
				model.AffiliationNumber = school.AffiliationNumber;
				model.City = school.City;
				model.Country = school.Country;
				model.CoverPictureId = school.CoverPictureId;
				if (coverpic != null)
				{
					model.CoverPicture = new PictureModel()
					{
						AlternateText = coverpic.AlternateText,
						Src = coverpic.PictureSrc,
						Url = coverpic.Url
					};
				}
				else
				{
					model.CoverPicture = null;
				}

				model.FacebookLink = school.FacebookLink;
				model.FreelancerLink = school.FreelancerLink;
				model.FullName = school.FullName;
				model.GooglePlusLink = school.GooglePlusLink;
				model.GuruLink = school.GuruLink;
				model.InstagramLink = school.InstagramLink;
				model.IsActive = school.IsActive;
				model.IsApproved = school.IsApproved;
				model.Landmark = school.Landmark;
				model.Latitude = school.Latitude;
				model.LinkedInLink = school.LinkedInLink;
				model.Longitude = school.Longitude;
				model.PInterestLink = school.PInterestLink;
				model.ProfilePictureId = school.ProfilePictureId;

				if (profilepic != null)
				{
					model.ProfilePicture = new PictureModel()
					{
						AlternateText = profilepic.AlternateText,
						Src = profilepic.PictureSrc,
						Url = profilepic.Url
					};
				}
				else
				{
					model.ProfilePicture = null;
				}

				model.RegistrationNumber = school.RegistrationNumber;
				model.State = school.State;
				model.Street1 = school.Street1;
				model.Street2 = school.Street2;
				model.SuperAdministratorId = school.SuperAdministratorId;
				model.TweeterLink = school.TweeterLink;
				model.UpworkLink = school.UpworkLink;
				model.UserId = school.UserId;
				model.UserName = school.UserName;
				model.ZipCode = school.ZipCode;
				model.TotalStudents = _smsService.GetTotalStudents();
				model.TotalTeachers = _smsService.GetTotalTeachers();
				model.Events = _eventService.GetAllEvents(true).Select(e => new EventModel()
				{
					Title = e.Title,
					StartDate = e.StartDate,
					EndDate = e.EndDate,
					Id = e.Id,
					IsClosed = e.IsClosed,
					Venue = e.Venue
				}).ToList();
				model.Blogs = _blogService.GetAllBlogs(true).Select(b => new BlogModel()
				{
					CreatedOn = b.CreatedOn,
					Email = b.Email,
					Name = b.Name,
					Subject = b.Subject,
					UserId = b.UserId,
					Id = b.Id
				}).ToList();
				model.News = _newsService.GetActiveNews().Select(n => new NewsModel()
				{
					ShortName = n.ShortName,
					Author = n.Author,
					Date = n.CreatedOn,
					StartDate = n.StartDate,
					EndDate = n.EndDate,
					Id = n.Id,
					NewsStatusId = n.NewsStatusId,
					UserId = n.UserId
				}).ToList();
			}
			return View(model);
		}


	}
}
