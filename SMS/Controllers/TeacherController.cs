using System.Web.Mvc;
using EF.Core;
using System.Linq;
using EF.Services.Service;
using SMS.Models;
using SMS.Mappers;
using SMS.Models.Widgets;
using System.Collections.Generic;

namespace SMS.Controllers
{
	public class TeacherController : PublicHttpController
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

		public TeacherController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, INewsService newsService, IEventService eventService)
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
			var model = new TeacherModel();
			var teacher = _smsService.GetTeacherById(id);

			if (teacher != null)
			{
				var coverpic = teacher.CoverPictureId > 0 ? _pictureService.GetPictureById(teacher.CoverPictureId) : null;
				var profilepic = teacher.ProfilePictureId > 0 ? _pictureService.GetPictureById(teacher.ProfilePictureId) : null;
			}
			return View(model);
		}

        // GET: Event
        [ChildActionOnly]
        public ActionResult Index()
        {
            var widgetModel = new List<TeacherWidgetModel>();
            var lstteachers = _smsService.GetAllTeachers(true).OrderByDescending(x => x.ModifiedOn).Take(4).ToList();
            if (lstteachers.Count > 0)
            {
                foreach (var teacher in lstteachers)
                {
                    var model = teacher.ToWidgetModel();
                    model.ProfilePicture = model.ProfilePictureId > 0 ? _pictureService.GetPictureById(model.ProfilePictureId).PictureSrc : "";
                    var teacherSubjects = _smsService.GetAllSubjectsByTeacher(teacher.Id).OrderByDescending(x => x.CreatedOn).ToList();
                    if (teacherSubjects.Count > 0)
                    {
                        model.Subjects = teacherSubjects.Select(x => x.ToModel()).ToList();
                    }
                    widgetModel.Add(model);
                }
            }
            return PartialView(widgetModel);
        }
    }
}
