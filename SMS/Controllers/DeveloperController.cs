using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Controllers
{
    public class DeveloperController : PublicHttpController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;
        private readonly ISMSService _smsService;
        private readonly INewsService _newsService;
        private readonly IVideoService _videoService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IBlogService _blogService;

        #endregion Fileds

        #region Constructor

        public DeveloperController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, ISMSService smsService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, INewsService newsService)
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
            this._newsService = newsService;
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Banner()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Biography()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Services()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Counter()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Portfolio()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Team()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Testimonial()
        {
            var model = new SliderModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Blogs()
        {
            var model = new BlogModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Contact()
        {
            var model = new FeedbackModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new SchoolModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Copyright()
        {
            var school = _smsService.GetSchoolById(1);

            if(school == null)
                throw new Exception("id");

            var model = school.ToModel();
            return PartialView(model);
        }

    }
}
