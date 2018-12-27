using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Data;
using EF.Services.Http;

namespace EF.Services.Service
{
	public class InstallationService : IInstallationService
	{
		#region Constructor

		private readonly IRepository<Blog> _blogRepository;
		private readonly IRepository<Comment> _commentRepository;
		private readonly IRepository<CustomPage> _customPageRepository;
		private readonly IRepository<DataToken> _dataTokenRepository;
		private readonly IRepository<Event> _eventRepository;
		private readonly IRepository<Feedback> _feedbackRepository;
		private readonly IRepository<File> _filesRepository;
		private readonly IRepository<Menus> _menuRepository;
		private readonly IRepository<PermissionRecord> _permissionRecordRepository;
		private readonly IRepository<Picture> _pictureRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Reply> _replyRepository;
		private readonly IRepository<Settings> _settingRepository;
		private readonly IRepository<Slider> _sliderRepository;
		private readonly IRepository<Template> _templateRepository;
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<UserRole> _userRoleRepository;
		private readonly IRepository<Video> _videoRepository;
		private readonly IRepository<DataToken> _tokenRepository;
		private readonly IUrlHelper _webHelper;
		private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
		private readonly IRepository<AcadmicYear> _acadmicYearRepository;
		private readonly IRepository<School> _schoolRepository;
		private readonly IRepository<CustomPageUrl> _customPageUrlRepository;
		private readonly IRepository<TimeTableSetting> _timeTableSettingRepository;
		private readonly IUrlService _urlService;
		private readonly IRepository<QuestionType> _questionTypeRepository;
        private readonly IRepository<Division> _divisionRepository;
        private readonly IRepository<ClassRoom> _classroomRepository;
        private readonly IRepository<Religion> _religionRepository;
        private readonly IRepository<Caste> _casteRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<Designation> _designationRepository;
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<House> _houseRepository;
        private readonly IRepository<MessageGroup> _messageGroupRepository;
        private readonly IRepository<Qualification> _qualificationRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Book> _bookRepository;


        public InstallationService(IRepository<Blog> blogRepository,
	  IRepository<Comment> commentRepository,
	  IRepository<CustomPage> customPageRepository,
	  IRepository<DataToken> dataTokenRepository,
	  IRepository<Event> eventRepository,
	  IRepository<Feedback> feedbackRepository,
	  IRepository<File> filesRepository,
	  IRepository<Menus> menuRepository,
	  IRepository<PermissionRecord> permissionRecordRepository,
	  IRepository<Picture> pictureRepository,
	  IRepository<Product> productRepository,
	  IRepository<Reply> replyRepository,
	  IRepository<Settings> settingRepository,
	  IRepository<Slider> sliderRepository,
	  IRepository<Template> templateRepository,
	  IRepository<User> userRepository,
	  IRepository<UserRole> userRoleRepository,
	  IRepository<Video> videoRepository,
	  IRepository<DataToken> tokenRepository,
	  IUrlHelper webHelper,
	  IRepository<ScheduleTask> scheduleTaskRepository,
	  IRepository<AcadmicYear> acadmicYearRepository,
	  IRepository<School> schoolRepository,
	  IRepository<CustomPageUrl> customPageUrlRepository,
	  IRepository<TimeTableSetting> timeTableSettingRepository,
	  IUrlService urlService,
	  IRepository<QuestionType> questionTypeRepository,
      IRepository<Division> divisionRepository,
      IRepository<ClassRoom> classroomRepository,
      IRepository<Religion> religionRepository,
      IRepository<Caste> casteRepository,
      IRepository<Category> categoryRepository,
      IRepository<ProductCategory> productCategoryRepository,
      IRepository<Designation> designationRepository,
      IRepository<Holiday> holidayRepository,
      IRepository<House> houseRepository,
      IRepository<MessageGroup> messageGroupRepository,
      IRepository<Qualification> qualificationRepository,
      IRepository<Subject> subjectRepository,
      IRepository<Vendor> vendorRepository,
      IRepository<Book> bookRepository)
		{
			this._blogRepository = blogRepository;
			this._commentRepository = commentRepository;
			this._customPageRepository = customPageRepository;
			this._dataTokenRepository = dataTokenRepository;
			this._eventRepository = eventRepository;
			this._feedbackRepository = feedbackRepository;
			this._filesRepository = filesRepository;
			this._menuRepository = menuRepository;
			this._permissionRecordRepository = permissionRecordRepository;
			this._pictureRepository = pictureRepository;
			this._productRepository = productRepository;
			this._replyRepository = replyRepository;
			this._settingRepository = settingRepository;
			this._productRepository = productRepository;
			this._sliderRepository = sliderRepository;
			this._templateRepository = templateRepository;
			this._userRepository = userRepository;
			this._userRoleRepository = userRoleRepository;
			this._videoRepository = videoRepository;
			this._tokenRepository = tokenRepository;
			this._webHelper = webHelper;
			this._scheduleTaskRepository = scheduleTaskRepository;
			this._acadmicYearRepository = acadmicYearRepository;
			this._schoolRepository = schoolRepository;
			this._customPageUrlRepository = customPageUrlRepository;
			this._timeTableSettingRepository = timeTableSettingRepository;
			this._urlService = urlService;
			this._questionTypeRepository = questionTypeRepository;
            this._divisionRepository = divisionRepository;
            this._classroomRepository = classroomRepository;
            this._religionRepository = religionRepository;
            this._casteRepository = casteRepository;
            this._categoryRepository = categoryRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._designationRepository = designationRepository;
            this._holidayRepository = holidayRepository;
            this._houseRepository = houseRepository;
            this._messageGroupRepository = messageGroupRepository;
            this._qualificationRepository = qualificationRepository;
            this._subjectRepository = subjectRepository;
            this._vendorRepository = vendorRepository;
            this._bookRepository = bookRepository;
        }

		#endregion

		public virtual void InitConnectionFactory()
		{
			var connectionFactory = new SqlConnectionFactory();
			#pragma warning disable 0618
			Database.DefaultConnectionFactory = connectionFactory;
		}

		public virtual void SetDatabaseInitializer()
		{
			var tablesToValidate = new[] { "Users", "UserRole", "UserInfo", "Settings" };

			var initializer = new CreateTablesIfNotExist<EFDbContext>(tablesToValidate);
			Database.SetInitializer(initializer);
		}

		public void InstallData(string AdminUsername, string AdminPassword, School school)
		{
			// Add Administrator User
			if (school == null)
				return;

			var user = new User();
			user.UserName = !String.IsNullOrEmpty(AdminUsername) ? AdminUsername.Trim() : "";
			user.Password = !String.IsNullOrEmpty(AdminPassword) ? AdminPassword.Trim() : "";
			user.CreatedOn = DateTime.Now;
			user.ModifiedOn = DateTime.Now;
			user.IsActive = true;
			user.UserGuid = Guid.NewGuid();
			user.IsApproved = true;
            user.Email = school.Email;

            _userRepository.Insert(user);

			var acadmicYear = new AcadmicYear();
			acadmicYear.Name = school.AcadmicYearName;
			acadmicYear.UserId = user.Id;
			acadmicYear.User = user;
			acadmicYear.IsActive = true;
			acadmicYear.IsDeleted = false;
			acadmicYear.CreatedOn = DateTime.Now;
			acadmicYear.ModifiedOn = DateTime.Now;
			_acadmicYearRepository.Insert(acadmicYear);

			#region School
			school.UserId = user.Id;
			school.SuperAdministratorId = user.Id;
			school.CreatedOn = DateTime.Now;
			school.ModifiedOn = DateTime.Now;
			school.IsActive = true;
			school.IsDeleted = false;
			school.AcadmicYearName = acadmicYear.Name;
            school.SchoolGuid = Guid.NewGuid();
            school.AcadmicYearId = acadmicYear.Id;
            school.Email = school.Email;

            _schoolRepository.Insert(school);
			#endregion

			#region Add Permissions

			var mangeUserPermission = new PermissionRecord();
			mangeUserPermission.Name = "Manage Users";
			mangeUserPermission.SystemName = "ManageUsers";
			mangeUserPermission.IsDeleted = false;
			mangeUserPermission.IsSystemDefined = true;
			mangeUserPermission.Category = "User";
			mangeUserPermission.ModifiedOn = DateTime.Now;
			mangeUserPermission.CreatedOn = DateTime.Now;
			mangeUserPermission.IsActive = true;
			mangeUserPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(mangeUserPermission);

			var manageUserProfilePermission = new PermissionRecord();
			manageUserProfilePermission.Name = "Manage User Profile";
			manageUserProfilePermission.SystemName = "ManageUserProfile";
			manageUserProfilePermission.IsDeleted = false;
			manageUserProfilePermission.IsSystemDefined = true;
			manageUserProfilePermission.Category = "User";
			manageUserProfilePermission.ModifiedOn = DateTime.Now;
			manageUserProfilePermission.CreatedOn = DateTime.Now;
			manageUserProfilePermission.IsActive = true;
			manageUserProfilePermission.UserId = user.Id;
			_permissionRecordRepository.Insert(manageUserProfilePermission);

			var managePicturePermission = new PermissionRecord();
			managePicturePermission.Name = "Manage Pictures";
			managePicturePermission.SystemName = "ManagePictures";
			managePicturePermission.IsDeleted = false;
			managePicturePermission.IsSystemDefined = true;
			managePicturePermission.Category = "Picture";
			managePicturePermission.ModifiedOn = DateTime.Now;
			managePicturePermission.CreatedOn = DateTime.Now;
			managePicturePermission.IsActive = true;
			managePicturePermission.UserId = user.Id;
			_permissionRecordRepository.Insert(managePicturePermission);

			var manageVideoPermission = new PermissionRecord();
			manageVideoPermission.Name = "Manage Videos";
			manageVideoPermission.SystemName = "ManageVideos";
			manageVideoPermission.IsDeleted = false;
			manageVideoPermission.Category = "Video";
			manageVideoPermission.IsSystemDefined = true;
			manageVideoPermission.ModifiedOn = DateTime.Now;
			manageVideoPermission.CreatedOn = DateTime.Now;
			manageVideoPermission.IsActive = true;
			manageVideoPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(manageVideoPermission);

			var _ManageEventPermission = new PermissionRecord();
			_ManageEventPermission.Name = "Manage Events";
			_ManageEventPermission.SystemName = "ManageEvents";
			_ManageEventPermission.IsDeleted = false;
			_ManageEventPermission.Category = "Event";
			_ManageEventPermission.IsSystemDefined = true;
			_ManageEventPermission.ModifiedOn = DateTime.Now;
			_ManageEventPermission.CreatedOn = DateTime.Now;
			_ManageEventPermission.IsActive = true;
			_ManageEventPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageEventPermission);

			var _ManageNewsPermission = new PermissionRecord();
			_ManageNewsPermission.Name = "Manage News";
			_ManageNewsPermission.SystemName = "ManageNews";
			_ManageNewsPermission.IsDeleted = false;
			_ManageNewsPermission.Category = "News";
			_ManageNewsPermission.IsSystemDefined = true;
			_ManageNewsPermission.ModifiedOn = DateTime.Now;
			_ManageNewsPermission.CreatedOn = DateTime.Now;
			_ManageNewsPermission.IsActive = true;
			_ManageNewsPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageNewsPermission);

			var _ManageBlogPermission = new PermissionRecord();
			_ManageBlogPermission.Name = "Manage Blogs";
			_ManageBlogPermission.SystemName = "ManageBlogs";
			_ManageBlogPermission.Category = "Blog";
			_ManageBlogPermission.IsDeleted = false;
			_ManageBlogPermission.IsSystemDefined = true;
			_ManageBlogPermission.ModifiedOn = DateTime.Now;
			_ManageBlogPermission.CreatedOn = DateTime.Now;
			_ManageBlogPermission.IsActive = true;
			_ManageBlogPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageBlogPermission);

			var _ManageFilePermission = new PermissionRecord();
			_ManageFilePermission.Name = "Manage Files";
			_ManageFilePermission.SystemName = "ManageFiles";
			_ManageFilePermission.Category = "File";
			_ManageFilePermission.IsDeleted = false;
			_ManageFilePermission.IsSystemDefined = true;
			_ManageFilePermission.ModifiedOn = DateTime.Now;
			_ManageFilePermission.CreatedOn = DateTime.Now;
			_ManageFilePermission.IsActive = true;
			_ManageFilePermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageFilePermission);

			var _ManageSliderPermission = new PermissionRecord();
			_ManageSliderPermission.Name = "Manage Slider";
			_ManageSliderPermission.SystemName = "ManageSlider";
			_ManageSliderPermission.IsDeleted = false;
			_ManageSliderPermission.Category = "Slider";
			_ManageSliderPermission.IsSystemDefined = true;
			_ManageSliderPermission.ModifiedOn = DateTime.Now;
			_ManageSliderPermission.CreatedOn = DateTime.Now;
			_ManageSliderPermission.IsActive = true;
			_ManageSliderPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSliderPermission);

			var _ManageSettingsPermission = new PermissionRecord();
			_ManageSettingsPermission.Name = "Manage Settings";
			_ManageSettingsPermission.SystemName = "ManageSettings";
			_ManageSettingsPermission.IsDeleted = false;
			_ManageSettingsPermission.Category = "Setting";
			_ManageSettingsPermission.IsSystemDefined = true;
			_ManageSettingsPermission.ModifiedOn = DateTime.Now;
			_ManageSettingsPermission.CreatedOn = DateTime.Now;
			_ManageSettingsPermission.IsActive = true;
			_ManageSettingsPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSettingsPermission);

			var _ManageTemplatePermission = new PermissionRecord();
			_ManageTemplatePermission.Name = "Manage Templates";
			_ManageTemplatePermission.SystemName = "ManageTemplates";
			_ManageTemplatePermission.IsDeleted = false;
			_ManageTemplatePermission.Category = "Template";
			_ManageTemplatePermission.IsSystemDefined = true;
			_ManageTemplatePermission.ModifiedOn = DateTime.Now;
			_ManageTemplatePermission.CreatedOn = DateTime.Now;
			_ManageTemplatePermission.IsActive = true;
			_ManageTemplatePermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageTemplatePermission);

			var _ManageCustomPagePermission = new PermissionRecord();
			_ManageCustomPagePermission.Name = "Manage Custom Pages";
			_ManageCustomPagePermission.SystemName = "ManageCustomPages";
			_ManageCustomPagePermission.Category = "CustomPage";
			_ManageCustomPagePermission.IsDeleted = false;
			_ManageCustomPagePermission.IsSystemDefined = true;
			_ManageCustomPagePermission.ModifiedOn = DateTime.Now;
			_ManageCustomPagePermission.CreatedOn = DateTime.Now;
			_ManageCustomPagePermission.IsActive = true;
			_ManageCustomPagePermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageCustomPagePermission);

			var _ManageTokensPermission = new PermissionRecord();
			_ManageTokensPermission.Name = "Manage Data Tokens";
			_ManageTokensPermission.SystemName = "ManageDataTokens";
			_ManageTokensPermission.Category = "Token";
			_ManageTokensPermission.IsDeleted = false;
			_ManageTokensPermission.IsSystemDefined = true;
			_ManageTokensPermission.ModifiedOn = DateTime.Now;
			_ManageTokensPermission.CreatedOn = DateTime.Now;
			_ManageTokensPermission.IsActive = true;
			_ManageTokensPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageTokensPermission);

			var _ManageRolesPermission = new PermissionRecord();
			_ManageRolesPermission.Name = "Manage Roles";
			_ManageRolesPermission.Category = "User";
			_ManageRolesPermission.SystemName = "ManageRoles";
			_ManageRolesPermission.IsDeleted = false;
			_ManageRolesPermission.IsSystemDefined = true;
			_ManageRolesPermission.ModifiedOn = DateTime.Now;
			_ManageRolesPermission.CreatedOn = DateTime.Now;
			_ManageRolesPermission.IsActive = true;
			_ManageRolesPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageRolesPermission);

			var _ManageProductPermission = new PermissionRecord();
			_ManageProductPermission.Name = "Manage Products";
			_ManageProductPermission.SystemName = "ManageProducts";
			_ManageProductPermission.IsDeleted = false;
			_ManageProductPermission.IsSystemDefined = true;
			_ManageProductPermission.Category = "Product";
			_ManageProductPermission.ModifiedOn = DateTime.Now;
			_ManageProductPermission.CreatedOn = DateTime.Now;
			_ManageProductPermission.IsActive = true;
			_ManageProductPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageProductPermission);

		    var _ManageVendorPermission = new PermissionRecord();
		    _ManageVendorPermission.Name = "Manage Vendors";
		    _ManageVendorPermission.SystemName = "ManageVendors";
		    _ManageVendorPermission.IsDeleted = false;
		    _ManageVendorPermission.IsSystemDefined = true;
		    _ManageVendorPermission.Category = "Vendor";
		    _ManageVendorPermission.ModifiedOn = DateTime.Now;
		    _ManageVendorPermission.CreatedOn = DateTime.Now;
		    _ManageVendorPermission.IsActive = true;
		    _ManageVendorPermission.UserId = user.Id;
		    _permissionRecordRepository.Insert(_ManageVendorPermission);

            var _ManageCommentsPermission = new PermissionRecord();
			_ManageCommentsPermission.Name = "Manage Comments";
			_ManageCommentsPermission.SystemName = "ManageComments";
			_ManageCommentsPermission.Category = "User";
			_ManageCommentsPermission.IsDeleted = false;
			_ManageCommentsPermission.IsSystemDefined = true;
			_ManageCommentsPermission.ModifiedOn = DateTime.Now;
			_ManageCommentsPermission.CreatedOn = DateTime.Now;
			_ManageCommentsPermission.IsActive = true;
			_ManageCommentsPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageCommentsPermission);

			var _MangeDashboardPermission = new PermissionRecord();
			_MangeDashboardPermission.Name = "Manage Dashboard";
			_MangeDashboardPermission.SystemName = "ManageDashboard";
			_MangeDashboardPermission.IsDeleted = false;
			_MangeDashboardPermission.IsSystemDefined = true;
			_MangeDashboardPermission.Category = "User";
			_MangeDashboardPermission.ModifiedOn = DateTime.Now;
			_MangeDashboardPermission.CreatedOn = DateTime.Now;
			_MangeDashboardPermission.IsActive = true;
			_MangeDashboardPermission.UserId = user.Id;
			_permissionRecordRepository.Insert(_MangeDashboardPermission);

			var _MangePermissions = new PermissionRecord();
			_MangePermissions.Name = "Manage Permissions";
			_MangePermissions.SystemName = "ManagePermissions";
			_MangePermissions.IsDeleted = false;
			_MangePermissions.IsSystemDefined = true;
			_MangePermissions.Category = "User";
			_MangePermissions.ModifiedOn = DateTime.Now;
			_MangePermissions.CreatedOn = DateTime.Now;
			_MangePermissions.IsActive = true;
			_MangePermissions.UserId = user.Id;
			_permissionRecordRepository.Insert(_MangePermissions);

			var _MangeConfiguration = new PermissionRecord();
			_MangeConfiguration.Name = "Manage Configuration";
			_MangeConfiguration.SystemName = "ManageConfiguration";
			_MangeConfiguration.IsDeleted = false;
			_MangeConfiguration.IsSystemDefined = true;
			_MangeConfiguration.Category = "User";
			_MangeConfiguration.ModifiedOn = DateTime.Now;
			_MangeConfiguration.CreatedOn = DateTime.Now;
			_MangeConfiguration.IsActive = true;
			_MangeConfiguration.UserId = user.Id;
			_permissionRecordRepository.Insert(_MangeConfiguration);

			var _ManageAudit = new PermissionRecord();
			_ManageAudit.Name = "Manage Audits";
			_ManageAudit.SystemName = "ManageAudits";
			_ManageAudit.IsDeleted = false;
			_ManageAudit.IsSystemDefined = true;
			_ManageAudit.Category = "User";
			_ManageAudit.ModifiedOn = DateTime.Now;
			_ManageAudit.CreatedOn = DateTime.Now;
			_ManageAudit.IsActive = true;
			_ManageAudit.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageAudit);

			var _ManageSystemLogs = new PermissionRecord();
			_ManageSystemLogs.Name = "Manage Logs";
			_ManageSystemLogs.SystemName = "ManageLogs";
			_ManageSystemLogs.IsDeleted = false;
			_ManageSystemLogs.IsSystemDefined = true;
			_ManageSystemLogs.Category = "User";
			_ManageSystemLogs.ModifiedOn = DateTime.Now;
			_ManageSystemLogs.CreatedOn = DateTime.Now;
			_ManageSystemLogs.IsActive = true;
			_ManageSystemLogs.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSystemLogs);

			var _ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Teachers";
			_ManageSMS.SystemName = "ManageTeachers";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Teacher";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Students";
			_ManageSMS.SystemName = "ManageStudents";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Student";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Employees";
			_ManageSMS.SystemName = "ManageEmployees";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Employee";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage News";
			_ManageSMS.SystemName = "ManageNews";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "News";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Student Attendance";
			_ManageSMS.SystemName = "ManageStudentAttendance";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "StudentAttendance";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Employee Attendance";
			_ManageSMS.SystemName = "ManageEmployeeAttendance";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "EmployeeAttendance";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Fee";
			_ManageSMS.SystemName = "ManageFee";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Fee";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Payment";
			_ManageSMS.SystemName = "ManagePayment";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Payment";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Time Table";
			_ManageSMS.SystemName = "ManageTimeTable";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "TimeTable";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Message";
			_ManageSMS.SystemName = "ManageMessage";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Message";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

			_ManageSMS = new PermissionRecord();
			_ManageSMS.Name = "Manage Exam";
			_ManageSMS.SystemName = "ManageExam";
			_ManageSMS.IsDeleted = false;
			_ManageSMS.IsSystemDefined = true;
			_ManageSMS.Category = "Exam";
			_ManageSMS.ModifiedOn = DateTime.Now;
			_ManageSMS.CreatedOn = DateTime.Now;
			_ManageSMS.IsActive = true;
			_ManageSMS.UserId = user.Id;
			_permissionRecordRepository.Insert(_ManageSMS);

            var manageClass = new PermissionRecord();
            manageClass.Name = "Manage Class";
            manageClass.SystemName = "ManageClass";
            manageClass.IsDeleted = false;
            manageClass.IsSystemDefined = true;
            manageClass.Category = "Class";
            manageClass.ModifiedOn = DateTime.Now;
            manageClass.CreatedOn = DateTime.Now;
            manageClass.IsActive = true;
            manageClass.UserId = user.Id;
            _permissionRecordRepository.Insert(manageClass);

            var manageDivision = new PermissionRecord();
            manageDivision.Name = "Manage Division";
            manageDivision.SystemName = "ManageDivision";
            manageDivision.IsDeleted = false;
            manageDivision.IsSystemDefined = true;
            manageDivision.Category = "Division";
            manageDivision.ModifiedOn = DateTime.Now;
            manageDivision.CreatedOn = DateTime.Now;
            manageDivision.IsActive = true;
            manageDivision.UserId = user.Id;
            _permissionRecordRepository.Insert(manageDivision);

            var manageClassRoom = new PermissionRecord();
            manageClassRoom.Name = "Manage Class Room";
            manageClassRoom.SystemName = "ManageClassRoom";
            manageClassRoom.IsDeleted = false;
            manageClassRoom.IsSystemDefined = true;
            manageClassRoom.Category = "ClassRoom";
            manageClassRoom.ModifiedOn = DateTime.Now;
            manageClassRoom.CreatedOn = DateTime.Now;
            manageClassRoom.IsActive = true;
            manageClassRoom.UserId = user.Id;
            _permissionRecordRepository.Insert(manageClassRoom);

            var managePermit = new PermissionRecord();
            managePermit.Name = "Manage Religion";
            managePermit.SystemName = "ManageReligions";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Religion";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Designation";
            managePermit.SystemName = "ManageDesignation";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Designation";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Caste";
            managePermit.SystemName = "ManageCaste";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Caste";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Category";
            managePermit.SystemName = "ManageCategory";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Category";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Fee Category";
            managePermit.SystemName = "ManageFeeCategory";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "FeeCategory";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Qualifications";
            managePermit.SystemName = "ManageQualifications";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Qualification";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Subject";
            managePermit.SystemName = "ManageSubject";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Subject";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            #endregion

            #region User Roles

            // Adding Default Roles
            // Administrator
            var _AdminRole = new UserRole();
			_AdminRole.CreatedOn = DateTime.Now;
			_AdminRole.ModifiedOn = DateTime.Now;
			_AdminRole.RoleName = "Administrator";
			_AdminRole.IsSystemDefined = true;
			_AdminRole.IsDeleted = false;
			_AdminRole.IsActive = true;
			_AdminRole.UserId = user.Id;

			// Add Suitable Permissions
			foreach (PermissionRecord _permission in _permissionRecordRepository.Table.ToList().Where(x => x.IsDeleted == false))
			{
				_AdminRole.PermissionRecords.Add(_permission);
			}
			_userRoleRepository.Insert(_AdminRole);
			user.Roles.Add(_AdminRole);

			// Primary
			var _PrimaryRole = new UserRole();
			_PrimaryRole.CreatedOn = DateTime.Now;
			_PrimaryRole.ModifiedOn = DateTime.Now;
			_PrimaryRole.RoleName = "Primary";
			_PrimaryRole.IsSystemDefined = true;
			_PrimaryRole.IsDeleted = false;
			_PrimaryRole.IsActive = true;
			_PrimaryRole.UserId = user.Id;
			_userRoleRepository.Insert(_PrimaryRole);

			// Standard
			var _StandardRole = new UserRole();
			_StandardRole.CreatedOn = DateTime.Now;
			_StandardRole.ModifiedOn = DateTime.Now;
			_StandardRole.RoleName = "Standard";
			_StandardRole.IsSystemDefined = true;
			_StandardRole.IsDeleted = false;
			_StandardRole.IsActive = true;
			_StandardRole.UserId = user.Id;
			_userRoleRepository.Insert(_StandardRole);

			// General Role
			var _GeneralRole = new UserRole();
			_GeneralRole.CreatedOn = DateTime.Now;
			_GeneralRole.ModifiedOn = DateTime.Now;
			_GeneralRole.RoleName = "General";
			_GeneralRole.IsSystemDefined = true;
			_GeneralRole.IsDeleted = false;
			_GeneralRole.IsActive = true;
			_GeneralRole.UserId = user.Id;
			_userRoleRepository.Insert(_GeneralRole);

			#endregion

			#region Settings

			// Slider
			Slider newSlider = new Slider();
			newSlider.CreatedOn = DateTime.Now;
			newSlider.ModifiedOn = DateTime.Now;
			newSlider.IsActive = true;
		    newSlider.DisplayArea = 1;
		    newSlider.DisplayOrder = 0;
		    newSlider.MaxPictures = 5;
		    newSlider.Name = "Default Slider";
		    newSlider.ShowCaption = true;
		    newSlider.ShowNextPrevIndicators = true;
		    newSlider.ShowThumbnails = true;
		    newSlider.IsSystemDefined = true;
            newSlider.UserId = user.Id;
			newSlider.Pictures.Add(new Picture()
			{
				AlternateText = "",
				CreatedOn = DateTime.Now,
				DisplayOrder = 1,
				IsActive = true,
				IsLogo = false,
				IsThumb = false,
				ModifiedOn = DateTime.Now,
				Url = "",
				UserId = user.Id,
				PictureSrc = "/Content/images/slide1.png"
			});
			newSlider.Pictures.Add(new Picture()
			{
				AlternateText = "",
				CreatedOn = DateTime.Now,
				DisplayOrder = 2,
				IsActive = true,
				IsLogo = false,
				IsThumb = false,
				ModifiedOn = DateTime.Now,
				Url = "",
				UserId = user.Id,
				PictureSrc = "/Content/images/slide2.jpg"
			});
			_sliderRepository.Insert(newSlider);


			var setting = new Settings();
			setting.EntityId = newSlider.Id;
			setting.Value = "5";
			setting.Name = "MaxPictures";
			setting.SettingType = (int)SettingTypeEnum.SliderSetting;
			setting.TypeId = (int)SettingTypeEnum.SliderSetting;
			setting.Entity = "Slider";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = newSlider.Id;
			setting.Value = "true";
			setting.Name = "SliderCaptionOff";
			setting.SettingType = (int)SettingTypeEnum.SliderSetting;
			setting.TypeId = (int)SettingTypeEnum.SliderSetting;
			setting.Entity = "Slider";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			// MaxWidthAllowedForLargeThumbnails
			var incServersetting = new Settings();
			incServersetting.EntityId = 0;
			incServersetting.Value = "300";
			incServersetting.Name = "MaxWidthAllowedForLargeThumbnails";
			incServersetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incServersetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incServersetting.Entity = "Picture";
			incServersetting.user = user;
			incServersetting.UserId = user.Id;
			incServersetting.CreatedOn = DateTime.Now;
			incServersetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incServersetting);

			// MaxWidthAllowedForMediumThumbnails
			var incPortsetting = new Settings();
			incPortsetting.EntityId = 0;
			incPortsetting.Value = "300";
			incPortsetting.Name = "MaxWidthAllowedForMediumThumbnails";
			incPortsetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incPortsetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incPortsetting.Entity = "Picture";
			incPortsetting.user = user;
			incPortsetting.UserId = user.Id;
			incPortsetting.CreatedOn = DateTime.Now;
			incPortsetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incPortsetting);

			// MaxWidthAllowedForSmallThumbnails
			var incOutgoingServerSetting = new Settings();
			incOutgoingServerSetting.EntityId = 0;
			incOutgoingServerSetting.Value = "300";
			incOutgoingServerSetting.Name = "MaxWidthAllowedForSmallThumbnails";
			incOutgoingServerSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incOutgoingServerSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incOutgoingServerSetting.Entity = "Picture";
			incOutgoingServerSetting.user = user;
			incOutgoingServerSetting.UserId = user.Id;
			incOutgoingServerSetting.CreatedOn = DateTime.Now;
			incOutgoingServerSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incOutgoingServerSetting);

			// MaxWidthAllowedForSliderThumbnails
			var incPasswordSetting = new Settings();
			incPasswordSetting.EntityId = 0;
			incPasswordSetting.Value = "1950";
			incPasswordSetting.Name = "MaxWidthAllowedForSliderThumbnails";
			incPasswordSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incPasswordSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incPasswordSetting.Entity = "Picture";
			incPasswordSetting.user = user;
			incPasswordSetting.UserId = user.Id;
			incPasswordSetting.CreatedOn = DateTime.Now;
			incPasswordSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incPasswordSetting);

			// MaxHeightAllowedForLargeThumbnails
			var incRequireSslSetting = new Settings();
			incRequireSslSetting.EntityId = 0;
			incRequireSslSetting.Value = "300";
			incRequireSslSetting.Name = "MaxHeightAllowedForLargeThumbnails";
			incRequireSslSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incRequireSslSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incRequireSslSetting.Entity = "Picture";
			incRequireSslSetting.user = user;
			incRequireSslSetting.UserId = user.Id;
			incRequireSslSetting.CreatedOn = DateTime.Now;
			incRequireSslSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incRequireSslSetting);

			// MaxHeightAllowedForMediumThumbnails
			var incSmtpAuthenticationSetting = new Settings();
			incSmtpAuthenticationSetting.EntityId = 0;
			incSmtpAuthenticationSetting.Value = "300";
			incSmtpAuthenticationSetting.Name = "MaxHeightAllowedForMediumThumbnails";
			incSmtpAuthenticationSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incSmtpAuthenticationSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incSmtpAuthenticationSetting.Entity = "Picture";
			incSmtpAuthenticationSetting.user = user;
			incSmtpAuthenticationSetting.UserId = user.Id;
			incSmtpAuthenticationSetting.CreatedOn = DateTime.Now;
			incSmtpAuthenticationSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incSmtpAuthenticationSetting);

			// MaxHeightAllowedForSmallThumbnails
			var incSmtPportSetting = new Settings();
			incSmtPportSetting.EntityId = 0;
			incSmtPportSetting.Value = "300";
			incSmtPportSetting.Name = "MaxHeightAllowedForSmallThumbnails";
			incSmtPportSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incSmtPportSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incSmtPportSetting.Entity = "Picture";
			incSmtPportSetting.user = user;
			incSmtPportSetting.UserId = user.Id;
			incSmtPportSetting.CreatedOn = DateTime.Now;
			incSmtPportSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incSmtPportSetting);

			// MaxHeightAllowedForSliderThumbnails
			var incUsernameSetting = new Settings();
			incUsernameSetting.EntityId = 0;
			incUsernameSetting.Value = "605";
			incUsernameSetting.Name = "MaxHeightAllowedForSliderThumbnails";
			incUsernameSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.Entity = "Picture";
			incUsernameSetting.user = user;
			incUsernameSetting.UserId = user.Id;
			incUsernameSetting.CreatedOn = DateTime.Now;
			incUsernameSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incUsernameSetting);

			incUsernameSetting = new Settings();
			incUsernameSetting.EntityId = 0;
			incUsernameSetting.Value = "jpeg,jpg,gif,png,gif,png";
			incUsernameSetting.Name = "PictureTypesAllowed";
			incUsernameSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.Entity = "Picture";
			incUsernameSetting.user = user;
			incUsernameSetting.UserId = user.Id;
			incUsernameSetting.CreatedOn = DateTime.Now;
			incUsernameSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incUsernameSetting);

			incUsernameSetting = new Settings();
			incUsernameSetting.EntityId = 0;
			incUsernameSetting.Value = "5000000";
			incUsernameSetting.Name = "MaximumSizeAllowed";
			incUsernameSetting.SettingType = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.TypeId = (int)SettingTypeEnum.PictureSetting;
			incUsernameSetting.Entity = "Picture";
			incUsernameSetting.user = user;
			incUsernameSetting.UserId = user.Id;
			incUsernameSetting.CreatedOn = DateTime.Now;
			incUsernameSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incUsernameSetting);

			setting = new Settings();
			setting.EntityId = user.Id;
			setting.Value = "10";
			setting.Name = "ItemsPerPage";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = user.Id;
			setting.Value = "Bottom";
			setting.Name = "PagerLocation";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = user.Id;
			setting.Value = "ForgetPassword";
			setting.Name = "ForgotPasswordTemplate";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "CommentOnBlog";
		    setting.Name = "CommentOnBlog";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "VisitorQueryPlaced";
		    setting.Name = "VisitorQueryPlaced";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "CommentOnEvent";
		    setting.Name = "CommentOnEvent";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "CommentOnProduct";
		    setting.Name = "CommentOnProduct";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "ProductAdded";
		    setting.Name = "ProductAdded";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "ReplyOnComment";
		    setting.Name = "ReplyOnComment";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "NewUserRegister";
		    setting.Name = "NewUserRegister";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "UserSignInAttempt";
		    setting.Name = "UserSignInAttempt";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "RequestQuote";
		    setting.Name = "RequestQuote";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

		    setting = new Settings();
		    setting.EntityId = user.Id;
		    setting.Value = "RequestQuote";
		    setting.Name = "RequestQuote";
		    setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
		    setting.Entity = "Configuration";
		    setting.user = user;
		    setting.UserId = user.Id;
		    setting.CreatedOn = DateTime.Now;
		    setting.ModifiedOn = DateTime.Now;
		    _settingRepository.Insert(setting);

            setting = new Settings();
			setting.EntityId = user.Id;
			setting.Value = "true";
			setting.Name = "DatabaseInstalled";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Database";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = user.Id;
			setting.Value = _webHelper.GetLocation();
			setting.Name = "WebContextUrl";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "CMS";
			setting.user = user;
			setting.UserId = user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

            // Email Settings For Live
            var emailSetting = new Settings();
            emailSetting.Name = "FromEmail";
            emailSetting.Value = "norpeply@sms.com";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "Username";
            emailSetting.Value = "sandeep725@gmail.com";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "Port";
            emailSetting.Value = "25";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "UseDefaultCredentials";
            emailSetting.Value = "false";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "EnableSSL";
            emailSetting.Value = "false";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "Password";
            emailSetting.Value = "";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            emailSetting = new Settings();
            emailSetting.Name = "Host";
            emailSetting.Value = "relay-hosting.secureserver.net";
            emailSetting.EntityId = 0;
            emailSetting.Entity = "Email";
            emailSetting.SettingType = 7;
            emailSetting.TypeId = 7;
            emailSetting.CreatedOn = emailSetting.ModifiedOn = DateTime.Now;
            emailSetting.UserId = user.Id;
            _settingRepository.Insert(emailSetting);

            var stuCodeSetting = new Settings();
            stuCodeSetting.Name = "StudentPrefix";
            stuCodeSetting.Value = "STU";
            stuCodeSetting.EntityId = 0;
            stuCodeSetting.Entity = "Student";
            stuCodeSetting.SettingType = 8;
            stuCodeSetting.TypeId = 8;
            stuCodeSetting.CreatedOn = stuCodeSetting.ModifiedOn = DateTime.Now;
            stuCodeSetting.UserId = user.Id;
            _settingRepository.Insert(stuCodeSetting);

            stuCodeSetting = new Settings();
            stuCodeSetting.Name = "StudentCodeStart";
            stuCodeSetting.Value = "101";
            stuCodeSetting.EntityId = 0;
            stuCodeSetting.Entity = "Student";
            stuCodeSetting.SettingType = 8;
            stuCodeSetting.TypeId = 8;
            stuCodeSetting.CreatedOn = stuCodeSetting.ModifiedOn = DateTime.Now;
            stuCodeSetting.UserId = user.Id;
            _settingRepository.Insert(stuCodeSetting);

            var empCodeSetting = new Settings();
            empCodeSetting.Name = "EmployeePrefix";
            empCodeSetting.Value = "EMP";
            empCodeSetting.EntityId = 0;
            empCodeSetting.Entity = "Employee";
            empCodeSetting.SettingType = 9;
            empCodeSetting.TypeId = 9;
            empCodeSetting.CreatedOn = empCodeSetting.ModifiedOn = DateTime.Now;
            empCodeSetting.UserId = user.Id;
            _settingRepository.Insert(empCodeSetting);

            empCodeSetting = new Settings();
            empCodeSetting.Name = "EmployeeCodeStart";
            empCodeSetting.Value = "201";
            empCodeSetting.EntityId = 0;
            empCodeSetting.Entity = "Student";
            empCodeSetting.SettingType = 9;
            empCodeSetting.TypeId = 9;
            empCodeSetting.CreatedOn = empCodeSetting.ModifiedOn = DateTime.Now;
            empCodeSetting.UserId = user.Id;
            _settingRepository.Insert(empCodeSetting);

            var subCodeSetting = new Settings();
            subCodeSetting.Name = "SubjectPrefix";
            subCodeSetting.Value = "SUB";
            subCodeSetting.EntityId = 0;
            subCodeSetting.Entity = "Subject";
            subCodeSetting.SettingType = 10;
            subCodeSetting.TypeId = 10;
            subCodeSetting.CreatedOn = subCodeSetting.ModifiedOn = DateTime.Now;
            subCodeSetting.UserId = user.Id;
            _settingRepository.Insert(subCodeSetting);

            subCodeSetting = new Settings();
            subCodeSetting.Name = "SubjectCodeStart";
            subCodeSetting.Value = "3001";
            subCodeSetting.EntityId = 0;
            subCodeSetting.Entity = "Subject";
            subCodeSetting.SettingType = 10;
            subCodeSetting.TypeId = 10;
            subCodeSetting.CreatedOn = subCodeSetting.ModifiedOn = DateTime.Now;
            subCodeSetting.UserId = user.Id;
            _settingRepository.Insert(subCodeSetting);

            #endregion

            #region Data Tokens

            #region User

            var visitorToken = new DataToken();
			visitorToken.Name = "Visitor Name";
			visitorToken.SystemName = "VisitorName";
			visitorToken.Value = "Visitor";
			visitorToken.IsSystemDefined = true;
			visitorToken.CreatedOn = DateTime.Now;
			visitorToken.IsActive = true;
			visitorToken.IsDeleted = false;
			visitorToken.ModifiedOn = DateTime.Now;
			visitorToken.UserId = user.Id;
			_tokenRepository.Insert(visitorToken);

			var usernameToken = new DataToken();
			usernameToken.Name = "Username";
			usernameToken.SystemName = "UserName";
			usernameToken.Value = "#";
			usernameToken.IsSystemDefined = true;
			usernameToken.CreatedOn = DateTime.Now;
			usernameToken.IsActive = true;
			usernameToken.IsDeleted = false;
			usernameToken.ModifiedOn = DateTime.Now;
			usernameToken.UserId = user.Id;
			_tokenRepository.Insert(usernameToken);

			var usermailToken = new DataToken();
			usermailToken.SystemName = "UserMail";
			usermailToken.Name = "User Email Address";
			usermailToken.Value = "#";
			usermailToken.IsSystemDefined = true;
			usermailToken.CreatedOn = DateTime.Now;
			usermailToken.IsActive = true;
			usermailToken.IsDeleted = false;
			usermailToken.ModifiedOn = DateTime.Now;
			usermailToken.UserId = user.Id;
			_tokenRepository.Insert(usermailToken);

			var useridToken = new DataToken();
			useridToken.SystemName = "UserId";
			useridToken.Name = "User Id";
			useridToken.Value = "#";
			useridToken.IsSystemDefined = true;
			useridToken.CreatedOn = DateTime.Now;
			useridToken.IsActive = true;
			useridToken.IsDeleted = false;
			useridToken.ModifiedOn = DateTime.Now;
			useridToken.UserId = user.Id;
			_tokenRepository.Insert(useridToken);

			var userpasswordToken = new DataToken();
			userpasswordToken.SystemName = "UserPassword";
			userpasswordToken.Name = "User Password";
			userpasswordToken.Value = "#";
			userpasswordToken.IsSystemDefined = true;
			userpasswordToken.CreatedOn = DateTime.Now;
			userpasswordToken.IsActive = true;
			userpasswordToken.IsDeleted = false;
			userpasswordToken.ModifiedOn = DateTime.Now;
			userpasswordToken.UserId = user.Id;
			_tokenRepository.Insert(userpasswordToken);

			var userapprToken = new DataToken();
			userapprToken.SystemName = "UserApproved";
			userapprToken.Name = "User Approved";
			userapprToken.Value = "#";
			userapprToken.IsSystemDefined = true;
			userapprToken.CreatedOn = DateTime.Now;
			userapprToken.IsActive = true;
			userapprToken.IsDeleted = false;
			userapprToken.ModifiedOn = DateTime.Now;
			userapprToken.UserId = user.Id;
			_tokenRepository.Insert(userapprToken);

			var useractiveToken = new DataToken();
			useractiveToken.Name = "User Active/Inactive";
			useractiveToken.SystemName = "UserActive";
			useractiveToken.Value = "#";
			useractiveToken.IsSystemDefined = true;
			useractiveToken.CreatedOn = DateTime.Now;
			useractiveToken.IsActive = true;
			useractiveToken.IsDeleted = false;
			useractiveToken.ModifiedOn = DateTime.Now;
			useractiveToken.UserId = user.Id;
			_tokenRepository.Insert(useractiveToken);


			var usercreationdateToken = new DataToken();
			usercreationdateToken.SystemName = "UserCreationDate";
			usercreationdateToken.Name = "User Created On";
			usercreationdateToken.Value = "#";
			usercreationdateToken.IsSystemDefined = true;
			usercreationdateToken.CreatedOn = DateTime.Now;
			usercreationdateToken.IsActive = true;
			usercreationdateToken.IsDeleted = false;
			usercreationdateToken.ModifiedOn = DateTime.Now;
			usercreationdateToken.UserId = user.Id;
			_tokenRepository.Insert(usercreationdateToken);

			#endregion;

			#region Event

			var eventStartDateToken = new DataToken();
			eventStartDateToken.SystemName = "EventStartDate";
			eventStartDateToken.Name = "Event Start Date";
			eventStartDateToken.Value = "#";
			eventStartDateToken.IsSystemDefined = true;
			eventStartDateToken.CreatedOn = DateTime.Now;
			eventStartDateToken.IsActive = true;
			eventStartDateToken.IsDeleted = false;
			eventStartDateToken.ModifiedOn = DateTime.Now;
			eventStartDateToken.UserId = user.Id;
			_tokenRepository.Insert(eventStartDateToken);

			var eventEndDateToken = new DataToken();
			eventEndDateToken.SystemName = "EventEndDate";
			eventEndDateToken.Name = "Event End Date";
			eventEndDateToken.Value = "#";
			eventEndDateToken.IsSystemDefined = true;
			eventEndDateToken.CreatedOn = DateTime.Now;
			eventEndDateToken.IsActive = true;
			eventEndDateToken.IsDeleted = false;
			eventEndDateToken.ModifiedOn = DateTime.Now;
			eventEndDateToken.UserId = user.Id;
			_tokenRepository.Insert(eventEndDateToken);

			var eventIdToken = new DataToken();
			eventIdToken.SystemName = "EventId";
			eventIdToken.Name = "Event Id";
			eventIdToken.Value = "#";
			eventIdToken.IsSystemDefined = true;
			eventIdToken.CreatedOn = DateTime.Now;
			eventIdToken.IsActive = true;
			eventIdToken.IsDeleted = false;
			eventIdToken.ModifiedOn = DateTime.Now;
			eventIdToken.UserId = user.Id;
			_tokenRepository.Insert(eventIdToken);

			var eventVenueToken = new DataToken();
			eventVenueToken.SystemName = "EventVenue";
			eventVenueToken.Name = "Event Location";
			eventVenueToken.Value = "#";
			eventVenueToken.IsSystemDefined = true;
			eventVenueToken.CreatedOn = DateTime.Now;
			eventVenueToken.IsActive = true;
			eventVenueToken.IsDeleted = false;
			eventVenueToken.ModifiedOn = DateTime.Now;
			eventVenueToken.UserId = user.Id;
			_tokenRepository.Insert(eventVenueToken);

			var eventTitleToken = new DataToken();
			eventTitleToken.SystemName = "EventTitle";
			eventTitleToken.Name = "Event Title";
			eventTitleToken.Value = "#";
			eventTitleToken.IsSystemDefined = true;
			eventTitleToken.CreatedOn = DateTime.Now;
			eventTitleToken.IsActive = true;
			eventTitleToken.IsDeleted = false;
			eventTitleToken.ModifiedOn = DateTime.Now;
			eventTitleToken.UserId = user.Id;
			_tokenRepository.Insert(eventTitleToken);

			var eventDescToken = new DataToken();
			eventDescToken.SystemName = "EventDescription";
			eventDescToken.Name = "Event Description";
			eventDescToken.Value = "#";
			eventDescToken.IsSystemDefined = true;
			eventDescToken.CreatedOn = DateTime.Now;
			eventDescToken.IsActive = true;
			eventDescToken.IsDeleted = false;
			eventDescToken.ModifiedOn = DateTime.Now;
			eventDescToken.UserId = user.Id;
			_tokenRepository.Insert(eventDescToken);

			eventDescToken = new DataToken();
			eventDescToken.SystemName = "EventUrl";
			eventDescToken.Name = "Event Url";
			eventDescToken.Value = "#";
			eventDescToken.IsSystemDefined = true;
			eventDescToken.CreatedOn = DateTime.Now;
			eventDescToken.IsActive = true;
			eventDescToken.IsDeleted = false;
			eventDescToken.ModifiedOn = DateTime.Now;
			eventDescToken.UserId = user.Id;
			_tokenRepository.Insert(eventDescToken);

			eventDescToken = new DataToken();
			eventDescToken.SystemName = "EventActive";
			eventDescToken.Name = "Event Active";
			eventDescToken.Value = "#";
			eventDescToken.IsSystemDefined = true;
			eventDescToken.CreatedOn = DateTime.Now;
			eventDescToken.IsActive = true;
			eventDescToken.IsDeleted = false;
			eventDescToken.ModifiedOn = DateTime.Now;
			eventDescToken.UserId = user.Id;
			_tokenRepository.Insert(eventDescToken);

			eventDescToken = new DataToken();
			eventDescToken.SystemName = "EventApproved";
			eventDescToken.Name = "Event Approved";
			eventDescToken.Value = "#";
			eventDescToken.IsSystemDefined = true;
			eventDescToken.CreatedOn = DateTime.Now;
			eventDescToken.IsActive = true;
			eventDescToken.IsDeleted = false;
			eventDescToken.ModifiedOn = DateTime.Now;
			eventDescToken.UserId = user.Id;
			_tokenRepository.Insert(eventDescToken);

			#endregion

			#region Product

			var productNameToken = new DataToken();
			productNameToken.SystemName = "ProductName";
			productNameToken.Name = "Product Name";
			productNameToken.Value = "#";
			productNameToken.IsSystemDefined = true;
			productNameToken.CreatedOn = DateTime.Now;
			productNameToken.IsActive = true;
			productNameToken.IsDeleted = false;
			productNameToken.ModifiedOn = DateTime.Now;
			productNameToken.UserId = user.Id;
			_tokenRepository.Insert(productNameToken);

			var productSeoToken = new DataToken();
			productSeoToken.SystemName = "ProductUrl";
			productSeoToken.Name = "Product Url";
			productSeoToken.Value = "#";
			productSeoToken.IsSystemDefined = true;
			productSeoToken.CreatedOn = DateTime.Now;
			productSeoToken.IsActive = true;
			productSeoToken.IsDeleted = false;
			productSeoToken.ModifiedOn = DateTime.Now;
			productSeoToken.UserId = user.Id;
			_tokenRepository.Insert(productSeoToken);

			var productIdToken = new DataToken();
			productIdToken.SystemName = "ProductId";
			productIdToken.Name = "Product Id";
			productIdToken.Value = "#";
			productIdToken.IsSystemDefined = true;
			productIdToken.CreatedOn = DateTime.Now;
			productIdToken.IsActive = true;
			productIdToken.IsDeleted = false;
			productIdToken.ModifiedOn = DateTime.Now;
			productIdToken.UserId = user.Id;
			_tokenRepository.Insert(productIdToken);

			var productDescToken = new DataToken();
			productDescToken.SystemName = "ProductDescription";
			productDescToken.Name = "Product Description";
			productDescToken.Value = "#";
			productDescToken.IsSystemDefined = true;
			productDescToken.CreatedOn = DateTime.Now;
			productDescToken.IsActive = true;
			productDescToken.IsDeleted = false;
			productDescToken.ModifiedOn = DateTime.Now;
			productDescToken.UserId = user.Id;
			_tokenRepository.Insert(productDescToken);

			var productTitleToken = new DataToken();
			productTitleToken.SystemName = "ProductName";
			productTitleToken.Name = "Product Name";
			productTitleToken.Value = "#";
			productTitleToken.IsSystemDefined = true;
			productTitleToken.CreatedOn = DateTime.Now;
			productTitleToken.IsActive = true;
			productTitleToken.IsDeleted = false;
			productTitleToken.ModifiedOn = DateTime.Now;
			productTitleToken.UserId = user.Id;
			_tokenRepository.Insert(productTitleToken);

			var productUserToken = new DataToken();
			productUserToken.SystemName = "ProductUser";
			productUserToken.Name = "Product User";
			productUserToken.Value = "#";
			productUserToken.IsSystemDefined = true;
			productUserToken.CreatedOn = DateTime.Now;
			productUserToken.IsActive = true;
			productUserToken.IsDeleted = false;
			productUserToken.ModifiedOn = DateTime.Now;
			productUserToken.UserId = user.Id;
			_tokenRepository.Insert(productUserToken);

			#endregion

			#region Comment

			var commentHtmlToken = new DataToken();
			commentHtmlToken.SystemName = "CommentHtml";
			commentHtmlToken.Name = "Comment Message";
			commentHtmlToken.Value = "#";
			commentHtmlToken.IsSystemDefined = true;
			commentHtmlToken.CreatedOn = DateTime.Now;
			commentHtmlToken.IsActive = true;
			commentHtmlToken.IsDeleted = false;
			commentHtmlToken.ModifiedOn = DateTime.Now;
			commentHtmlToken.UserId = user.Id;
			_tokenRepository.Insert(commentHtmlToken);

			var commentIdToken = new DataToken();
			commentIdToken.SystemName = "CommentId";
			commentIdToken.Name = "Comment Id";
			commentIdToken.Value = "#";
			commentIdToken.IsSystemDefined = true;
			commentIdToken.CreatedOn = DateTime.Now;
			commentIdToken.IsActive = true;
			commentIdToken.IsDeleted = false;
			commentIdToken.ModifiedOn = DateTime.Now;
			commentIdToken.UserId = user.Id;
			_tokenRepository.Insert(commentIdToken);

			var commentUserToken = new DataToken();
			commentUserToken.SystemName = "CommentUser";
			commentUserToken.Name = "Comment User";
			commentUserToken.Value = "#";
			commentUserToken.IsSystemDefined = true;
			commentUserToken.CreatedOn = DateTime.Now;
			commentUserToken.IsActive = true;
			commentUserToken.IsDeleted = false;
			commentUserToken.ModifiedOn = DateTime.Now;
			commentUserToken.UserId = user.Id;
			_tokenRepository.Insert(commentUserToken);

			commentUserToken = new DataToken();
			commentUserToken.SystemName = "CommentAddedOn";
			commentUserToken.Name = "Comment Added On";
			commentUserToken.Value = "#";
			commentUserToken.IsSystemDefined = true;
			commentUserToken.CreatedOn = DateTime.Now;
			commentUserToken.IsActive = true;
			commentUserToken.IsDeleted = false;
			commentUserToken.ModifiedOn = DateTime.Now;
			commentUserToken.UserId = user.Id;
			_tokenRepository.Insert(commentUserToken);

			commentUserToken = new DataToken();
			commentUserToken.SystemName = "CommentBlockReason";
			commentUserToken.Name = "Comment Block Reason";
			commentUserToken.Value = "#";
			commentUserToken.IsSystemDefined = true;
			commentUserToken.CreatedOn = DateTime.Now;
			commentUserToken.IsActive = true;
			commentUserToken.IsDeleted = false;
			commentUserToken.ModifiedOn = DateTime.Now;
			commentUserToken.UserId = user.Id;
			_tokenRepository.Insert(commentUserToken);

			commentUserToken = new DataToken();
			commentUserToken.SystemName = "CommentBlockedByUserId";
			commentUserToken.Name = "Comment Blocked By User Id";
			commentUserToken.Value = "#";
			commentUserToken.IsSystemDefined = true;
			commentUserToken.CreatedOn = DateTime.Now;
			commentUserToken.IsActive = true;
			commentUserToken.IsDeleted = false;
			commentUserToken.ModifiedOn = DateTime.Now;
			commentUserToken.UserId = user.Id;
			_tokenRepository.Insert(commentUserToken);

			commentUserToken = new DataToken();
			commentUserToken.SystemName = "CommentBlockedByUser";
			commentUserToken.Name = "Comment Blocked By User";
			commentUserToken.Value = "#";
			commentUserToken.IsSystemDefined = true;
			commentUserToken.CreatedOn = DateTime.Now;
			commentUserToken.IsActive = true;
			commentUserToken.IsDeleted = false;
			commentUserToken.ModifiedOn = DateTime.Now;
			commentUserToken.UserId = user.Id;
			_tokenRepository.Insert(commentUserToken);

			#endregion

			#region Blog

			// Blog tokens
			var blogToken = new DataToken();
			blogToken.SystemName = "BlogCreatedOn";
			blogToken.Name = "Blog Created On";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = user.Id;
			_tokenRepository.Insert(blogToken);

			var blogEmailToken = new DataToken();
		    blogEmailToken.SystemName = "BlogEmail";
		    blogEmailToken.Name = "Blog Email";
		    blogEmailToken.Value = "#";
		    blogEmailToken.IsSystemDefined = true;
		    blogEmailToken.CreatedOn = DateTime.Now;
		    blogEmailToken.IsActive = true;
		    blogEmailToken.IsDeleted = false;
		    blogEmailToken.ModifiedOn = DateTime.Now;
		    blogEmailToken.UserId = user.Id;
			_tokenRepository.Insert(blogEmailToken);

			var blogIdToken = new DataToken();
		    blogIdToken.SystemName = "BlogId";
		    blogIdToken.Name = "Blog Id";
		    blogIdToken.Value = "#";
		    blogIdToken.IsSystemDefined = true;
		    blogIdToken.CreatedOn = DateTime.Now;
		    blogIdToken.IsActive = true;
		    blogIdToken.IsDeleted = false;
		    blogIdToken.ModifiedOn = DateTime.Now;
		    blogIdToken.UserId = user.Id;
			_tokenRepository.Insert(blogIdToken);

			var blogUserIdToken = new DataToken();
		    blogUserIdToken.SystemName = "BlogUserId";
		    blogUserIdToken.Name = "Blog User Id";
		    blogUserIdToken.Value = "#";
		    blogUserIdToken.IsSystemDefined = true;
		    blogUserIdToken.CreatedOn = DateTime.Now;
		    blogUserIdToken.IsActive = true;
		    blogUserIdToken.IsDeleted = false;
		    blogUserIdToken.ModifiedOn = DateTime.Now;
		    blogUserIdToken.UserId = user.Id;
			_tokenRepository.Insert(blogUserIdToken);

			var blogNameToken = new DataToken();
		    blogNameToken.SystemName = "BlogName";
		    blogNameToken.Name = "Blog Name";
		    blogNameToken.Value = "#";
		    blogNameToken.IsSystemDefined = true;
		    blogNameToken.CreatedOn = DateTime.Now;
		    blogNameToken.IsActive = true;
		    blogNameToken.IsDeleted = false;
		    blogNameToken.ModifiedOn = DateTime.Now;
		    blogNameToken.UserId = user.Id;
			_tokenRepository.Insert(blogNameToken);

			var blogActiveToken = new DataToken();
		    blogActiveToken.SystemName = "BlogActive";
		    blogActiveToken.Name = "Blog Active";
		    blogActiveToken.Value = "#";
		    blogActiveToken.IsSystemDefined = true;
		    blogActiveToken.CreatedOn = DateTime.Now;
		    blogActiveToken.IsActive = true;
		    blogActiveToken.IsDeleted = false;
		    blogActiveToken.ModifiedOn = DateTime.Now;
		    blogActiveToken.UserId = user.Id;
			_tokenRepository.Insert(blogActiveToken);

			var blogUrlToken = new DataToken();
		    blogUrlToken.SystemName = "BlogUrl";
		    blogUrlToken.Name = "Blog Url";
		    blogUrlToken.Value = "#";
		    blogUrlToken.IsSystemDefined = true;
		    blogUrlToken.CreatedOn = DateTime.Now;
		    blogUrlToken.IsActive = true;
		    blogUrlToken.IsDeleted = false;
		    blogUrlToken.ModifiedOn = DateTime.Now;
		    blogUrlToken.UserId = user.Id;
			_tokenRepository.Insert(blogUrlToken);

			var blogSubjectToken = new DataToken();
		    blogSubjectToken.SystemName = "BlogSubject";
		    blogSubjectToken.Name = "Blog Subject";
		    blogSubjectToken.Value = "#";
		    blogSubjectToken.IsSystemDefined = true;
		    blogSubjectToken.CreatedOn = DateTime.Now;
		    blogSubjectToken.IsActive = true;
		    blogSubjectToken.IsDeleted = false;
		    blogSubjectToken.ModifiedOn = DateTime.Now;
		    blogSubjectToken.UserId = user.Id;
			_tokenRepository.Insert(blogSubjectToken);

			var blogApprovedToken = new DataToken();
		    blogApprovedToken.SystemName = "BlogApproved";
		    blogApprovedToken.Name = "Blog Approved";
		    blogApprovedToken.Value = "#";
		    blogApprovedToken.IsSystemDefined = true;
		    blogApprovedToken.CreatedOn = DateTime.Now;
		    blogApprovedToken.IsActive = true;
		    blogApprovedToken.IsDeleted = false;
		    blogApprovedToken.ModifiedOn = DateTime.Now;
		    blogApprovedToken.UserId = user.Id;
			_tokenRepository.Insert(blogApprovedToken);

			#endregion

			#region Assessment Templates
			var assessTemplate = new Template();
			assessTemplate.Name = "AssessmentScheduled";
			assessTemplate.IsActive = true;
			assessTemplate.IsDeleted = false;
			assessTemplate.IsSystemDefined = true;
			assessTemplate.ModifiedOn = DateTime.Now;
			assessTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[AssessmentName],</td></tr><tr><td colspan='2'>Assessment Has Been Scheduled.</td></tr></tbody></table>";
			assessTemplate.CreatedOn = DateTime.Now;
			assessTemplate.UserId = user.Id;

			var stuAccessTemplate = new Template();
			stuAccessTemplate.Name = "AssessmentCompleted";
			stuAccessTemplate.IsActive = true;
			stuAccessTemplate.IsDeleted = false;
			stuAccessTemplate.IsSystemDefined = true;
			stuAccessTemplate.ModifiedOn = DateTime.Now;
			stuAccessTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[AssessmentName],</td></tr><tr><td colspan='2'>Assessment Has Been Completed.</td></tr></tbody></table>";
			stuAccessTemplate.CreatedOn = DateTime.Now;
			stuAccessTemplate.UserId = user.Id;

			#region Assessment

			var assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentStartTime";
			assessmentToken.Name = "Assessment Start Time";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentEndTime";
			assessmentToken.Name = "Assessment End Time";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentName";
			assessmentToken.Name = "Assessment Name";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentUrl";
			assessmentToken.Name = "Assessment Url";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentPassingMarks";
			assessmentToken.Name = "Assessment Passing Marks";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentMaxMarks";
			assessmentToken.Name = "Assessment Maximum Marks";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentInstructions";
			assessmentToken.Name = "Assessment Instructions";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentIsTimeBound";
			assessmentToken.Name = "Assessment Is Time Bound";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentTotalQuestions";
			assessmentToken.Name = "Assessment Total Questions";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentDuration";
			assessmentToken.Name = "Assessment Duration(In Mins.)";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			assessmentToken = new DataToken();
			assessmentToken.SystemName = "AssessmentMandatoryToSolveAll";
			assessmentToken.Name = "Assessment Mandatory To Solve All Questions";
			assessmentToken.Value = "#";
			assessmentToken.IsSystemDefined = true;
			assessmentToken.CreatedOn = DateTime.Now;
			assessmentToken.IsActive = true;
			assessmentToken.IsDeleted = false;
			assessmentToken.ModifiedOn = DateTime.Now;
			assessmentToken.UserId = user.Id;
			_tokenRepository.Insert(assessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			#endregion

			#region Student Assessment

			var stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentFirstName";
			stuassessmentToken.Name = "Assessment Student First Name";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentLastName";
			stuassessmentToken.Name = "Assessment Student Last Name";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentUserName";
			stuassessmentToken.Name = "Assessment Student User Name";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentAcadmicYear";
			stuassessmentToken.Name = "Assessment Student Acadmic Year";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentDOB";
			stuassessmentToken.Name = "Assessment Student Date Of Birth";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "StudentAssessmentStartOn";
			stuassessmentToken.Name = "Student Assessment Start On";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "StudentAssessmentEndOn";
			stuassessmentToken.Name = "Student Assessment End On";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "StudentAssessmentUrl";
			stuassessmentToken.Name = "Student Assessment Url";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "StudentAssessmentMarksObtained";
			stuassessmentToken.Name = "Student Assessment Marks Obtained";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "StudentAssessmentResultStatus";
			stuassessmentToken.Name = "Student Assessment Result";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			#endregion

			_templateRepository.Insert(assessTemplate);
			_templateRepository.Insert(stuAccessTemplate);

            #endregion

            #region Feedback

		    var fEmailToken = new DataToken();
		    fEmailToken.Name = "Feedback Email";
		    fEmailToken.SystemName = "FeedbackEmail";
		    fEmailToken.Value = "Feedback Email";
		    fEmailToken.IsSystemDefined = true;
		    fEmailToken.CreatedOn = DateTime.Now;
		    fEmailToken.IsActive = true;
		    fEmailToken.IsDeleted = false;
		    fEmailToken.ModifiedOn = DateTime.Now;
		    fEmailToken.UserId = user.Id;
		    _tokenRepository.Insert(fEmailToken);

		    var fIdToken = new DataToken();
		    fIdToken.Name = "Feedback Id";
		    fIdToken.SystemName = "FeedbackId";
		    fIdToken.Value = "Feedback Id";
		    fIdToken.IsSystemDefined = true;
		    fIdToken.CreatedOn = DateTime.Now;
		    fIdToken.IsActive = true;
		    fIdToken.IsDeleted = false;
		    fIdToken.ModifiedOn = DateTime.Now;
		    fIdToken.UserId = user.Id;
		    _tokenRepository.Insert(fIdToken);

		    var fContactToken = new DataToken();
		    fContactToken.Name = "Feedback Contact";
		    fContactToken.SystemName = "FeedbackContact";
		    fContactToken.Value = "Feedback Contact";
		    fContactToken.IsSystemDefined = true;
		    fContactToken.CreatedOn = DateTime.Now;
		    fContactToken.IsActive = true;
		    fContactToken.IsDeleted = false;
		    fContactToken.ModifiedOn = DateTime.Now;
		    fContactToken.UserId = user.Id;
		    _tokenRepository.Insert(fContactToken);

		    var fLocationToken = new DataToken();
		    fLocationToken.Name = "Feedback Location";
		    fLocationToken.SystemName = "FeedbackLocation";
		    fLocationToken.Value = "Feedback Location";
		    fLocationToken.IsSystemDefined = true;
		    fLocationToken.CreatedOn = DateTime.Now;
		    fLocationToken.IsActive = true;
		    fLocationToken.IsDeleted = false;
		    fLocationToken.ModifiedOn = DateTime.Now;
		    fLocationToken.UserId = user.Id;
		    _tokenRepository.Insert(fLocationToken);

		    var fDescriptionToken = new DataToken();
		    fDescriptionToken.Name = "Feedback Description";
		    fDescriptionToken.SystemName = "FeedbackDescription";
		    fDescriptionToken.Value = "Feedback Description";
		    fDescriptionToken.IsSystemDefined = true;
		    fDescriptionToken.CreatedOn = DateTime.Now;
		    fDescriptionToken.IsActive = true;
		    fDescriptionToken.IsDeleted = false;
		    fDescriptionToken.ModifiedOn = DateTime.Now;
		    fDescriptionToken.UserId = user.Id;
		    _tokenRepository.Insert(fDescriptionToken);

            #endregion

            #endregion

            #region Default Email Templates

            var defaultTemplate = new Template();
			defaultTemplate.Name = "VisitorQueryPlaced";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [VisitorName],</td></tr><tr><td colspan='2'>Thanks for sending us query. We will be right back to you shortly.</td></tr></tbody><tfoot><tr><td colspan='2'>Thanks.<br>Artery labs Inc.</td></tr></tfoot></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.Tokens.Add(visitorToken);
			defaultTemplate.UserId = user.Id;

            // Add Data Tokens
		    defaultTemplate.Tokens.Add(visitorToken);
		    defaultTemplate.Tokens.Add(fContactToken);
		    defaultTemplate.Tokens.Add(fDescriptionToken);
		    defaultTemplate.Tokens.Add(fEmailToken);
		    defaultTemplate.Tokens.Add(fIdToken);
		    defaultTemplate.Tokens.Add(fLocationToken);

            _templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "CommentOnEvent";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [VisitorName],</td></tr><tr><td colspan='2'>Thanks for your comment.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(eventDescToken);
			defaultTemplate.Tokens.Add(eventEndDateToken);
			defaultTemplate.Tokens.Add(eventIdToken);
			defaultTemplate.Tokens.Add(eventStartDateToken);
			defaultTemplate.Tokens.Add(eventTitleToken);
			defaultTemplate.Tokens.Add(eventVenueToken);
			defaultTemplate.Tokens.Add(commentIdToken);
			defaultTemplate.Tokens.Add(commentHtmlToken);
			defaultTemplate.Tokens.Add(commentUserToken);
			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "CommentOnProduct";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [ProductUser],</td></tr><tr><td colspan='2'>Thanks for your comment.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(productDescToken);
			defaultTemplate.Tokens.Add(productIdToken);
			defaultTemplate.Tokens.Add(productNameToken);
			defaultTemplate.Tokens.Add(productSeoToken);
			defaultTemplate.Tokens.Add(productTitleToken);
			defaultTemplate.Tokens.Add(productUserToken);
			defaultTemplate.Tokens.Add(commentIdToken);
			defaultTemplate.Tokens.Add(commentHtmlToken);
			defaultTemplate.Tokens.Add(commentUserToken);

			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "ProductAdded";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [UserName],</td></tr><tr><td colspan='2'>Product Added Successfully.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(productDescToken);
			defaultTemplate.Tokens.Add(productIdToken);
			defaultTemplate.Tokens.Add(productNameToken);
			defaultTemplate.Tokens.Add(productSeoToken);
			defaultTemplate.Tokens.Add(productTitleToken);
			defaultTemplate.Tokens.Add(productUserToken);
			defaultTemplate.Tokens.Add(usernameToken);

			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "ReplyOnComment";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] replied to a comment.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(commentIdToken);
			defaultTemplate.Tokens.Add(commentHtmlToken);
			defaultTemplate.Tokens.Add(commentUserToken);

			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "NewUserRegister";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] registered successfully.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(usercreationdateToken);
			defaultTemplate.Tokens.Add(useridToken);
			defaultTemplate.Tokens.Add(usermailToken);
			defaultTemplate.Tokens.Add(usernameToken);

			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "UserSignInAttempt";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] just signed in into the system.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

			defaultTemplate.Tokens.Add(usercreationdateToken);
			defaultTemplate.Tokens.Add(useridToken);
			defaultTemplate.Tokens.Add(usermailToken);
			defaultTemplate.Tokens.Add(usernameToken);

			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "RequestQuote";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[VisitorName] thanks for your quote.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = user.Id;

		    // Add Data Tokens
		    defaultTemplate.Tokens.Add(visitorToken);
		    defaultTemplate.Tokens.Add(usercreationdateToken);
		    defaultTemplate.Tokens.Add(useridToken);
		    defaultTemplate.Tokens.Add(usermailToken);
		    defaultTemplate.Tokens.Add(usernameToken);
            defaultTemplate.Tokens.Add(fContactToken);
		    defaultTemplate.Tokens.Add(fDescriptionToken);
		    defaultTemplate.Tokens.Add(fEmailToken);
		    defaultTemplate.Tokens.Add(fIdToken);
		    defaultTemplate.Tokens.Add(fLocationToken);
            _templateRepository.Insert(defaultTemplate);

		    defaultTemplate = new Template();
		    defaultTemplate.Name = "ForgetPassword";
		    defaultTemplate.IsActive = true;
		    defaultTemplate.IsDeleted = false;
		    defaultTemplate.IsSystemDefined = true;
		    defaultTemplate.ModifiedOn = DateTime.Now;
		    defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserEmail]</td></tr></tbody></table>";
		    defaultTemplate.CreatedOn = DateTime.Now;
		    defaultTemplate.UserId = user.Id;

		    defaultTemplate.Tokens.Add(usercreationdateToken);
		    defaultTemplate.Tokens.Add(useridToken);
		    defaultTemplate.Tokens.Add(usermailToken);
		    defaultTemplate.Tokens.Add(userpasswordToken);

		    _templateRepository.Insert(defaultTemplate);

		    defaultTemplate = new Template();
		    defaultTemplate.Name = "CommentOnBlog";
		    defaultTemplate.IsActive = true;
		    defaultTemplate.IsDeleted = false;
		    defaultTemplate.IsSystemDefined = true;
		    defaultTemplate.ModifiedOn = DateTime.Now;
		    defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[BlogName]</td></tr></tbody></table>";
		    defaultTemplate.CreatedOn = DateTime.Now;
		    defaultTemplate.UserId = user.Id;

		    defaultTemplate.Tokens.Add(blogActiveToken);
		    defaultTemplate.Tokens.Add(blogApprovedToken);
		    defaultTemplate.Tokens.Add(blogEmailToken);
		    defaultTemplate.Tokens.Add(blogIdToken);

		    defaultTemplate.Tokens.Add(blogNameToken);
		    defaultTemplate.Tokens.Add(blogSubjectToken);
		    defaultTemplate.Tokens.Add(blogUrlToken);
		    defaultTemplate.Tokens.Add(blogUserIdToken);

            _templateRepository.Insert(defaultTemplate);

            #endregion

            #region Scheduled Tasks

            _scheduleTaskRepository.Insert(new ScheduleTask
			{
				Name = "Keep alive",
				Seconds = 300,
				Type = "EF.Services.Tasks.KeepAlive, EF.Services",
				Enabled = true,
				StopOnError = false,
				CreatedOn = DateTime.Now,
				ModifiedOn = DateTime.Now,
				UserId = user.Id
			});

			#endregion

			#region Question Types

			var questionType = new QuestionType();
			questionType.Name = "Multiple Choice";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Multiple Response";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Fill in the blanks";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Match Following";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Match Matrix";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Essay";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Single Digit";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = user.Id;
			_questionTypeRepository.Insert(questionType);

			#endregion

			// Save School URL Record
			_customPageUrlRepository.Insert(new CustomPageUrl
			{
				EntityId = school.Id,
				EntityName = "School",
				CreatedOn = DateTime.Now,
				ModifiedOn = DateTime.Now,
				UserId = user.Id,
				IsActive = true,
				Slug = school.ValidateSystemName("", school.FullName, true)
			});

            // School Default Data

            #region Divisions
            var addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "A";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "B";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "C";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "D";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            _divisionRepository.Insert(addDivision);
            #endregion

            #region Classrooms

            var addClassRoom = new ClassRoom();
            addClassRoom.AcadmicYearId = acadmicYear.Id;
            addClassRoom.ModifiedOn = addClassRoom.CreatedOn = DateTime.Now;
            addClassRoom.Description = "Default";
            addClassRoom.IsActive = true;
            addClassRoom.IsDeleted = false;
            addClassRoom.Number = "1";
            addClassRoom.UserId = user.Id;
            _classroomRepository.Insert(addClassRoom);

            addClassRoom = new ClassRoom();
            addClassRoom.AcadmicYearId = acadmicYear.Id;
            addClassRoom.ModifiedOn = addClassRoom.CreatedOn = DateTime.Now;
            addClassRoom.Description = "Default";
            addClassRoom.IsActive = true;
            addClassRoom.IsDeleted = false;
            addClassRoom.Number = "2";
            addClassRoom.UserId = user.Id;
            _classroomRepository.Insert(addClassRoom);

            addClassRoom = new ClassRoom();
            addClassRoom.AcadmicYearId = acadmicYear.Id;
            addClassRoom.ModifiedOn = addClassRoom.CreatedOn = DateTime.Now;
            addClassRoom.Description = "Default";
            addClassRoom.IsActive = true;
            addClassRoom.IsDeleted = false;
            addClassRoom.Number = "3";
            addClassRoom.UserId = user.Id;
            _classroomRepository.Insert(addClassRoom);

            addClassRoom = new ClassRoom();
            addClassRoom.AcadmicYearId = acadmicYear.Id;
            addClassRoom.ModifiedOn = addClassRoom.CreatedOn = DateTime.Now;
            addClassRoom.Description = "Default";
            addClassRoom.IsActive = true;
            addClassRoom.IsDeleted = false;
            addClassRoom.Number = "4";
            addClassRoom.UserId = user.Id;
            _classroomRepository.Insert(addClassRoom);

            #endregion

            #region Religions

            var hindReligion = new Religion();
            hindReligion.CreatedOn = hindReligion.ModifiedOn = DateTime.Now;
            hindReligion.Description = "Default";
            hindReligion.Name = "Hindu";
            hindReligion.UserId = user.Id;
            _religionRepository.Insert(hindReligion);

            var muslimReligion = new Religion();
            muslimReligion.CreatedOn = muslimReligion.ModifiedOn = DateTime.Now;
            muslimReligion.Description = "Default";
            muslimReligion.Name = "Muslim";
            muslimReligion.UserId = user.Id;
            _religionRepository.Insert(muslimReligion);

            var sikhReligion = new Religion();
            sikhReligion.CreatedOn = sikhReligion.ModifiedOn = DateTime.Now;
            sikhReligion.Description = "Default";
            sikhReligion.Name = "Sikh";
            sikhReligion.UserId = user.Id;
            _religionRepository.Insert(sikhReligion);

            var christReligion = new Religion();
            christReligion.CreatedOn = christReligion.ModifiedOn = DateTime.Now;
            christReligion.Description = "Default";
            christReligion.Name = "Christian";
            christReligion.UserId = user.Id;
            _religionRepository.Insert(christReligion);

            var budhReligion = new Religion();
            budhReligion.CreatedOn = budhReligion.ModifiedOn = DateTime.Now;
            budhReligion.Description = "Default";
            budhReligion.Name = "Budhism";
            budhReligion.UserId = user.Id;
            _religionRepository.Insert(budhReligion);

            var jainReligion = new Religion();
            jainReligion.CreatedOn = jainReligion.ModifiedOn = DateTime.Now;
            jainReligion.Description = "Default";
            jainReligion.Name = "Jain";
            jainReligion.UserId = user.Id;
            _religionRepository.Insert(jainReligion);

            var otherReligion = new Religion();
            otherReligion.CreatedOn = otherReligion.ModifiedOn = DateTime.Now;
            otherReligion.Description = "Default";
            otherReligion.Name = "Other";
            otherReligion.UserId = user.Id;
            _religionRepository.Insert(otherReligion);

            #endregion

            #region Categories

            var genCategory = new Category();
            genCategory.Name = "General";
            genCategory.CreatedOn = genCategory.ModifiedOn = DateTime.Now;
            genCategory.IsActive = true;
            genCategory.IsDeleted = false;
            genCategory.UserId = user.Id;
            _categoryRepository.Insert(genCategory);

            var obcCategory = new Category();
            obcCategory.Name = "OBC";
            obcCategory.CreatedOn = obcCategory.ModifiedOn = DateTime.Now;
            obcCategory.IsActive = true;
            obcCategory.IsDeleted = false;
            obcCategory.UserId = user.Id;
            _categoryRepository.Insert(obcCategory);

            var scstCategory = new Category();
            scstCategory.Name = "SC/ST";
            scstCategory.CreatedOn = scstCategory.ModifiedOn = DateTime.Now;
            scstCategory.IsActive = true;
            scstCategory.IsDeleted = false;
            scstCategory.UserId = user.Id;
            _categoryRepository.Insert(scstCategory);

            var otherCategory = new Category();
            otherCategory.Name = "Other";
            otherCategory.CreatedOn = otherCategory.ModifiedOn = DateTime.Now;
            otherCategory.IsActive = true;
            otherCategory.IsDeleted = false;
            otherCategory.UserId = user.Id;
            _categoryRepository.Insert(otherCategory);

            #endregion

            #region Designation

            var addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Principal";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Academic Director";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Administrator";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Admissions Recruiter";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Assistant Principal";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Bus Driver";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Name = "Coach";
            addDesignation.UserId = user.Id;
            addDesignation.Description = "Default";
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Computer Science Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Family and Consumer Science Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Health/Physical Education Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Instrumental Music Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Math Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "School Counselor";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "School Librarian";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Science Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Social Studies Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Substitute Teacher";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Superintendent";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Vice Principal";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Bus Conductor";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            addDesignation = new Designation();
            addDesignation.CreatedOn = addDesignation.ModifiedOn = DateTime.Now;
            addDesignation.IsActive = true;
            addDesignation.IsDeleted = false;
            addDesignation.Description = "Default";
            addDesignation.Name = "Nursery Man";
            addDesignation.UserId = user.Id;
            _designationRepository.Insert(addDesignation);

            #endregion

            #region Holiday

            var addHoliday = new Holiday();
            addHoliday.AcadmicYearId = acadmicYear.Id;
            addHoliday.CreatedOn = addHoliday.ModifiedOn = DateTime.Now;
            addHoliday.Date = new DateTime(DateTime.Now.Year, 08, 15);
            addHoliday.DD = 15;
            addHoliday.MM = 08;
            addHoliday.YYYY = DateTime.Now.Year;
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Independence Day";
            addHoliday.UserId = user.Id;
            _holidayRepository.Insert(addHoliday);

            addHoliday = new Holiday();
            addHoliday.AcadmicYearId = acadmicYear.Id;
            addHoliday.CreatedOn = addHoliday.ModifiedOn = DateTime.Now;
            addHoliday.Date = new DateTime(DateTime.Now.Year, 01, 26);
            addHoliday.DD = 26;
            addHoliday.MM = 01;
            addHoliday.YYYY = DateTime.Now.Year;
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Republic Day";
            addHoliday.UserId = user.Id;
            _holidayRepository.Insert(addHoliday);

            addHoliday = new Holiday();
            addHoliday.AcadmicYearId = acadmicYear.Id;
            addHoliday.CreatedOn = addHoliday.ModifiedOn = DateTime.Now;
            addHoliday.Date = new DateTime(DateTime.Now.Year, 12, 25);
            addHoliday.DD = 26;
            addHoliday.MM = 01;
            addHoliday.YYYY = DateTime.Now.Year;
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Christmas Day";
            addHoliday.UserId = user.Id;
            _holidayRepository.Insert(addHoliday);

            #endregion

            #region House

            // Pictures
            var addPicture = new Picture();
            addPicture.IsActive = true;
            addPicture.AcadmicYearId = acadmicYear.Id;
            addPicture.AlternateText = "Gandhi House";
            addPicture.CreatedOn = addPicture.ModifiedOn = DateTime.Now;
            addPicture.IsThumb = false;
            addPicture.IsLogo = false;
            addPicture.Height = 115;
            addPicture.Size = 3;
            addPicture.Url = "";
            addPicture.UserId = user.Id;
            addPicture.Width = 81;
            addPicture.PictureSrc = "/Content/images/Houses/gandhi.jpg";
            _pictureRepository.Insert(addPicture);

            var addHouse = new House();
            addHouse.AcadmicYearId = acadmicYear.Id;
            addHouse.CreatedOn = addHouse.ModifiedOn = DateTime.Now;
            addHouse.IsActive = true;
            addHouse.IsDeleted = false;
            addHouse.Description = "Default";
            addHouse.Name = "Gandhi House";
            addHouse.UserId = user.Id;
            addHouse.PictureId = addPicture.Id;
            _houseRepository.Insert(addHouse);

            addPicture = new Picture();
            addPicture.IsActive = true;
            addPicture.AcadmicYearId = acadmicYear.Id;
            addPicture.AlternateText = "Nehru House";
            addPicture.CreatedOn = addPicture.ModifiedOn = DateTime.Now;
            addPicture.IsThumb = false;
            addPicture.IsLogo = false;
            addPicture.Height = 115;
            addPicture.Size = 3;
            addPicture.Url = "";
            addPicture.UserId = user.Id;
            addPicture.Width = 81;
            addPicture.PictureSrc = "/Content/images/Houses/nehru.jpg";
            _pictureRepository.Insert(addPicture);

            addHouse = new House();
            addHouse.AcadmicYearId = acadmicYear.Id;
            addHouse.CreatedOn = addHouse.ModifiedOn = DateTime.Now;
            addHouse.IsActive = true;
            addHouse.IsDeleted = false;
            addHouse.Description = "Default";
            addHouse.Name = "Nehru House";
            addHouse.UserId = user.Id;
            addHouse.PictureId = addPicture.Id;
            _houseRepository.Insert(addHouse);

            addPicture = new Picture();
            addPicture.IsActive = true;
            addPicture.AcadmicYearId = acadmicYear.Id;
            addPicture.AlternateText = "Patel House";
            addPicture.CreatedOn = addPicture.ModifiedOn = DateTime.Now;
            addPicture.IsThumb = false;
            addPicture.IsLogo = false;
            addPicture.Height = 115;
            addPicture.Size = 3;
            addPicture.Url = "";
            addPicture.UserId = user.Id;
            addPicture.Width = 81;
            addPicture.PictureSrc = "/Content/images/Houses/patel.jpg";
            _pictureRepository.Insert(addPicture);

            addHouse = new House();
            addHouse.AcadmicYearId = acadmicYear.Id;
            addHouse.CreatedOn = addHouse.ModifiedOn = DateTime.Now;
            addHouse.IsActive = true;
            addHouse.IsDeleted = false;
            addHouse.Description = "Default";
            addHouse.Name = "Patel House";
            addHouse.UserId = user.Id;
            addHouse.PictureId = addPicture.Id;
            _houseRepository.Insert(addHouse);

            addPicture = new Picture();
            addPicture.IsActive = true;
            addPicture.AcadmicYearId = acadmicYear.Id;
            addPicture.AlternateText = "Tagore House";
            addPicture.CreatedOn = addPicture.ModifiedOn = DateTime.Now;
            addPicture.IsThumb = false;
            addPicture.IsLogo = false;
            addPicture.Height = 115;
            addPicture.Size = 3;
            addPicture.Url = "";
            addPicture.UserId = user.Id;
            addPicture.Width = 81;
            addPicture.PictureSrc = "/Content/images/Houses/tagore.jpg";
            _pictureRepository.Insert(addPicture);

            addHouse = new House();
            addHouse.AcadmicYearId = acadmicYear.Id;
            addHouse.CreatedOn = addHouse.ModifiedOn = DateTime.Now;
            addHouse.IsActive = true;
            addHouse.IsDeleted = false;
            addHouse.Description = "Default";
            addHouse.Name = "Tagore House";
            addHouse.UserId = user.Id;
            addHouse.PictureId = addPicture.Id;
            _houseRepository.Insert(addHouse);

            #endregion

            #region Message Group

            var addMsgGroup = new MessageGroup();
            addMsgGroup.Name = "Default";
            addMsgGroup.CreatedOn = addMsgGroup.ModifiedOn = DateTime.Now;
            addMsgGroup.Description = "Default";
            addMsgGroup.UserId = user.Id;
            _messageGroupRepository.Insert(addMsgGroup);

            #endregion

            #region Qualification

            var addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "Metric";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "12th";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "B.A";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "BSc.";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "B.Com";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "M.Com";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "M.A";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "B.Tech";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "BCA";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "MCA";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            addQualification = new Qualification();
            addQualification.CreatedOn = addQualification.ModifiedOn = DateTime.Now;
            addQualification.Description = "Default";
            addQualification.Name = "Other";
            addQualification.UserId = user.Id;
            _qualificationRepository.Insert(addQualification);

            #endregion

            #region Subject

            var addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "Mathematics";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "A01";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "Hindi";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "A02";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "Science";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "A03";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "English";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "A03";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "Social Studies";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "A04";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            addSubject = new Subject();
            addSubject.AcadmicYearId = acadmicYear.Id;
            addSubject.CreatedOn = addSubject.ModifiedOn = DateTime.Now;
            addSubject.IsActive = true;
            addSubject.IsDeleted = false;
            addSubject.Name = "Physical Education";
            addSubject.Remarks = "";
            addSubject.UserId = user.Id;
            addSubject.Code = "B01";
            addSubject.SubjectUniqueId = Guid.NewGuid();
            _subjectRepository.Insert(addSubject);

            #endregion

            #region Vendor

            var addVendor = new Vendor();
            addVendor.AcadmicYearId = acadmicYear.Id;
            addVendor.Address = "Default";
            addVendor.CreatedOn = addVendor.ModifiedOn = DateTime.Now;
            addVendor.IsActive = true;
            addVendor.IsDeleted = false;
            addVendor.MobileContact = "0123456789";
            addVendor.Name = "Default Vendor";
            addVendor.OfficeContact = "0123456789";
            addVendor.RegNumber = "0123";
            addVendor.UserId = user.Id;
            addVendor.Date = DateTime.Now;
            _vendorRepository.Insert(addVendor);

            #endregion

            #region Book

            var addBook = new Book();
            addBook.AcadmicYearId = acadmicYear.Id;
            addBook.Author = "Default";
            addBook.BookStatusId = 1;
            addBook.CreatedOn = addBook.ModifiedOn = DateTime.Now;
            addBook.IsActive = true;
            addBook.IsDeleted = false;
            addBook.Name = "Default Book";
            addBook.Price = 0;
            addBook.UserId = user.Id;
            _bookRepository.Insert(addBook);


            #endregion

            // Save Timetable Settings - Default
            var timeTableSetting = new TimeTableSetting();
			timeTableSetting.AcadmicYearId = acadmicYear.Id;
			timeTableSetting.CreatedOn = DateTime.Now;
			timeTableSetting.LectureTime = 40.0;
			timeTableSetting.ModifiedOn = DateTime.Now;
			timeTableSetting.NoBreak = false;
			timeTableSetting.RecessTimeMin = 15.0;
			timeTableSetting.SchoolStartTime = DateTime.Now;
			timeTableSetting.SchoolEndTime = DateTime.Now;
			timeTableSetting.UserId = user.Id;
			_timeTableSettingRepository.Insert(timeTableSetting);

			// Update user at last
			_userRepository.Update(user);
		}
	}
}
