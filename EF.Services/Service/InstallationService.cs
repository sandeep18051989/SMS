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
	  IRepository<QuestionType> questionTypeRepository)
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

			var _user = new User();
			_user.UserName = !String.IsNullOrEmpty(AdminUsername) ? AdminUsername.Trim() : "";
			_user.Password = !String.IsNullOrEmpty(AdminPassword) ? AdminPassword.Trim() : "";
			_user.CreatedOn = DateTime.Now;
			_user.ModifiedOn = DateTime.Now;
			_user.IsActive = true;
			_user.UserGuid = Guid.NewGuid();
			_user.IsApproved = true;
			_userRepository.Insert(_user);

			var acadmicYear = new AcadmicYear();
			acadmicYear.Name = school.AcadmicYearName;
			acadmicYear.UserId = _user.Id;
			acadmicYear.User = _user;
			acadmicYear.IsActive = true;
			acadmicYear.IsDeleted = false;
			acadmicYear.CreatedOn = DateTime.Now;
			acadmicYear.ModifiedOn = DateTime.Now;
			_acadmicYearRepository.Insert(acadmicYear);

			#region School
			school.UserId = _user.Id;
			school.SuperAdministratorId = _user.Id;
			school.CreatedOn = DateTime.Now;
			school.ModifiedOn = DateTime.Now;
			school.IsActive = true;
			school.IsDeleted = false;
			school.AcadmicYearName = acadmicYear.Name;
			school.AcadmicYearId = acadmicYear.Id;
			_schoolRepository.Insert(school);
			#endregion

			#region Add Permissions

			var _MangeUserPermission = new PermissionRecord();
			_MangeUserPermission.Name = "Manage Users";
			_MangeUserPermission.SystemName = "ManageUsers";
			_MangeUserPermission.IsDeleted = false;
			_MangeUserPermission.IsSystemDefined = true;
			_MangeUserPermission.Category = "User";
			_MangeUserPermission.ModifiedOn = DateTime.Now;
			_MangeUserPermission.CreatedOn = DateTime.Now;
			_MangeUserPermission.IsActive = true;
			_permissionRecordRepository.Insert(_MangeUserPermission);

			var _ManageUserProfilePermission = new PermissionRecord();
			_ManageUserProfilePermission.Name = "Manage User Profile";
			_ManageUserProfilePermission.SystemName = "ManageUserProfile";
			_ManageUserProfilePermission.IsDeleted = false;
			_ManageUserProfilePermission.IsSystemDefined = true;
			_ManageUserProfilePermission.Category = "User";
			_ManageUserProfilePermission.ModifiedOn = DateTime.Now;
			_ManageUserProfilePermission.CreatedOn = DateTime.Now;
			_ManageUserProfilePermission.IsActive = true;
			_permissionRecordRepository.Insert(_ManageUserProfilePermission);

			var _ManagePicturePermission = new PermissionRecord();
			_ManagePicturePermission.Name = "Manage Pictures";
			_ManagePicturePermission.SystemName = "ManagePictures";
			_ManagePicturePermission.IsDeleted = false;
			_ManagePicturePermission.IsSystemDefined = true;
			_ManagePicturePermission.Category = "Picture";
			_ManagePicturePermission.ModifiedOn = DateTime.Now;
			_ManagePicturePermission.CreatedOn = DateTime.Now;
			_ManagePicturePermission.IsActive = true;
			_permissionRecordRepository.Insert(_ManagePicturePermission);

			var _ManageVideoPermission = new PermissionRecord();
			_ManageVideoPermission.Name = "Manage Videos";
			_ManageVideoPermission.SystemName = "ManageVideos";
			_ManageVideoPermission.IsDeleted = false;
			_ManageVideoPermission.Category = "Video";
			_ManageVideoPermission.IsSystemDefined = true;
			_ManageVideoPermission.ModifiedOn = DateTime.Now;
			_ManageVideoPermission.CreatedOn = DateTime.Now;
			_ManageVideoPermission.IsActive = true;
			_permissionRecordRepository.Insert(_ManageVideoPermission);

			var _ManageEventPermission = new PermissionRecord();
			_ManageEventPermission.Name = "Manage Events";
			_ManageEventPermission.SystemName = "ManageEvents";
			_ManageEventPermission.IsDeleted = false;
			_ManageEventPermission.Category = "Event";
			_ManageEventPermission.IsSystemDefined = true;
			_ManageEventPermission.ModifiedOn = DateTime.Now;
			_ManageEventPermission.CreatedOn = DateTime.Now;
			_ManageEventPermission.IsActive = true;
			_permissionRecordRepository.Insert(_ManageEventPermission);

			var _ManageBlogPermission = new PermissionRecord();
			_ManageBlogPermission.Name = "Manage Blogs";
			_ManageBlogPermission.SystemName = "ManageBlogs";
			_ManageBlogPermission.Category = "Blog";
			_ManageBlogPermission.IsDeleted = false;
			_ManageBlogPermission.IsSystemDefined = true;
			_ManageBlogPermission.ModifiedOn = DateTime.Now;
			_ManageBlogPermission.CreatedOn = DateTime.Now;
			_ManageBlogPermission.IsActive = true;
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
			_permissionRecordRepository.Insert(_ManageProductPermission);

			var _ManageCommentsPermission = new PermissionRecord();
			_ManageCommentsPermission.Name = "Manage Comments";
			_ManageCommentsPermission.SystemName = "ManageComments";
			_ManageCommentsPermission.Category = "User";
			_ManageCommentsPermission.IsDeleted = false;
			_ManageCommentsPermission.IsSystemDefined = true;
			_ManageCommentsPermission.ModifiedOn = DateTime.Now;
			_ManageCommentsPermission.CreatedOn = DateTime.Now;
			_ManageCommentsPermission.IsActive = true;
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
			_permissionRecordRepository.Insert(_ManageSMS);

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
			_AdminRole.UserId = _user.Id;

			// Add Suitable Permissions
			foreach (PermissionRecord _permission in _permissionRecordRepository.Table.ToList().Where(x => x.IsDeleted == false))
			{
				_AdminRole.PermissionRecords.Add(_permission);
			}
			_userRoleRepository.Insert(_AdminRole);
			_user.Roles.Add(_AdminRole);

			// Primary
			var _PrimaryRole = new UserRole();
			_PrimaryRole.CreatedOn = DateTime.Now;
			_PrimaryRole.ModifiedOn = DateTime.Now;
			_PrimaryRole.RoleName = "Primary";
			_PrimaryRole.IsSystemDefined = true;
			_PrimaryRole.IsDeleted = false;
			_PrimaryRole.IsActive = true;
			_PrimaryRole.UserId = _user.Id;
			_userRoleRepository.Insert(_PrimaryRole);

			// Standard
			var _StandardRole = new UserRole();
			_StandardRole.CreatedOn = DateTime.Now;
			_StandardRole.ModifiedOn = DateTime.Now;
			_StandardRole.RoleName = "Standard";
			_StandardRole.IsSystemDefined = true;
			_StandardRole.IsDeleted = false;
			_StandardRole.IsActive = true;
			_StandardRole.UserId = _user.Id;
			_userRoleRepository.Insert(_StandardRole);

			// General Role
			var _GeneralRole = new UserRole();
			_GeneralRole.CreatedOn = DateTime.Now;
			_GeneralRole.ModifiedOn = DateTime.Now;
			_GeneralRole.RoleName = "General";
			_GeneralRole.IsSystemDefined = true;
			_GeneralRole.IsDeleted = false;
			_GeneralRole.IsActive = true;
			_GeneralRole.UserId = _user.Id;
			_userRoleRepository.Insert(_GeneralRole);

			#endregion

			#region Settings
			// Slider
			Slider newSlider = new Slider();
			newSlider.CreatedOn = DateTime.Now;
			newSlider.ModifiedOn = DateTime.Now;
			newSlider.IsActive = true;
			newSlider.UserId = _user.Id;
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
				UserId = _user.Id,
				PictureSrc = "/Content/images/slide1.jpg"
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
				UserId = _user.Id,
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
			setting.user = _user;
			setting.UserId = _user.Id;
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
			setting.user = _user;
			setting.UserId = _user.Id;
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
			incServersetting.user = _user;
			incServersetting.UserId = _user.Id;
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
			incPortsetting.user = _user;
			incPortsetting.UserId = _user.Id;
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
			incOutgoingServerSetting.user = _user;
			incOutgoingServerSetting.UserId = _user.Id;
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
			incPasswordSetting.user = _user;
			incPasswordSetting.UserId = _user.Id;
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
			incRequireSslSetting.user = _user;
			incRequireSslSetting.UserId = _user.Id;
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
			incSmtpAuthenticationSetting.user = _user;
			incSmtpAuthenticationSetting.UserId = _user.Id;
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
			incSmtPportSetting.user = _user;
			incSmtPportSetting.UserId = _user.Id;
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
			incUsernameSetting.user = _user;
			incUsernameSetting.UserId = _user.Id;
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
			incUsernameSetting.user = _user;
			incUsernameSetting.UserId = _user.Id;
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
			incUsernameSetting.user = _user;
			incUsernameSetting.UserId = _user.Id;
			incUsernameSetting.CreatedOn = DateTime.Now;
			incUsernameSetting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(incUsernameSetting);

			setting = new Settings();
			setting.EntityId = _user.Id;
			setting.Value = "10";
			setting.Name = "ItemsPerPage";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = _user;
			setting.UserId = _user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = _user.Id;
			setting.Value = "Bottom";
			setting.Name = "PagerLocation";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = _user;
			setting.UserId = _user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = _user.Id;
			setting.Value = "ForgotPassword";
			setting.Name = "ForgotPasswordTemplate";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Configuration";
			setting.user = _user;
			setting.UserId = _user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = _user.Id;
			setting.Value = "true";
			setting.Name = "DatabaseInstalled";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "Database";
			setting.user = _user;
			setting.UserId = _user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);

			setting = new Settings();
			setting.EntityId = _user.Id;
			setting.Value = _webHelper.GetLocation();
			setting.Name = "WebContextUrl";
			setting.SettingType = (int)SettingTypeEnum.ConfigurationSetting;
			setting.TypeId = (int)SettingTypeEnum.ConfigurationSetting;
			setting.Entity = "CMS";
			setting.user = _user;
			setting.UserId = _user.Id;
			setting.CreatedOn = DateTime.Now;
			setting.ModifiedOn = DateTime.Now;
			_settingRepository.Insert(setting);


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
			visitorToken.UserId = _user.Id;
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
			usernameToken.UserId = _user.Id;
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
			usermailToken.UserId = _user.Id;
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
			useridToken.UserId = _user.Id;
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
			userpasswordToken.UserId = _user.Id;
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
			userapprToken.UserId = _user.Id;
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
			useractiveToken.UserId = _user.Id;
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
			usercreationdateToken.UserId = _user.Id;
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
			eventStartDateToken.UserId = _user.Id;
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
			eventEndDateToken.UserId = _user.Id;
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
			eventIdToken.UserId = _user.Id;
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
			eventVenueToken.UserId = _user.Id;
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
			eventTitleToken.UserId = _user.Id;
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
			eventDescToken.UserId = _user.Id;
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
			eventDescToken.UserId = _user.Id;
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
			eventDescToken.UserId = _user.Id;
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
			eventDescToken.UserId = _user.Id;
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
			productNameToken.UserId = _user.Id;
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
			productSeoToken.UserId = _user.Id;
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
			productIdToken.UserId = _user.Id;
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
			productDescToken.UserId = _user.Id;
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
			productTitleToken.UserId = _user.Id;
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
			productUserToken.UserId = _user.Id;
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
			commentHtmlToken.UserId = _user.Id;
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
			commentIdToken.UserId = _user.Id;
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
			commentUserToken.UserId = _user.Id;
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
			commentUserToken.UserId = _user.Id;
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
			commentUserToken.UserId = _user.Id;
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
			commentUserToken.UserId = _user.Id;
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
			commentUserToken.UserId = _user.Id;
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
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogEmail";
			blogToken.Name = "Blog Email";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogId";
			blogToken.Name = "Blog Id";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogUserId";
			blogToken.Name = "Blog User Id";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogName";
			blogToken.Name = "Blog Name";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogActive";
			blogToken.Name = "Blog Active";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogUrl";
			blogToken.Name = "Blog Url";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogSubject";
			blogToken.Name = "Blog Subject";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

			blogToken = new DataToken();
			blogToken.SystemName = "BlogApproved";
			blogToken.Name = "Blog Approved";
			blogToken.Value = "#";
			blogToken.IsSystemDefined = true;
			blogToken.CreatedOn = DateTime.Now;
			blogToken.IsActive = true;
			blogToken.IsDeleted = false;
			blogToken.ModifiedOn = DateTime.Now;
			blogToken.UserId = _user.Id;
			_tokenRepository.Insert(blogToken);

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
			assessTemplate.UserId = _user.Id;

			var stuAccessTemplate = new Template();
			stuAccessTemplate.Name = "AssessmentCompleted";
			stuAccessTemplate.IsActive = true;
			stuAccessTemplate.IsDeleted = false;
			stuAccessTemplate.IsSystemDefined = true;
			stuAccessTemplate.ModifiedOn = DateTime.Now;
			stuAccessTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[AssessmentName],</td></tr><tr><td colspan='2'>Assessment Has Been Completed.</td></tr></tbody></table>";
			stuAccessTemplate.CreatedOn = DateTime.Now;
			stuAccessTemplate.UserId = _user.Id;

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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			assessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			stuassessmentToken = new DataToken();
			stuassessmentToken.SystemName = "AssessmentStudentRollNumber";
			stuassessmentToken.Name = "Assessment Student Roll Number";
			stuassessmentToken.Value = "#";
			stuassessmentToken.IsSystemDefined = true;
			stuassessmentToken.CreatedOn = DateTime.Now;
			stuassessmentToken.IsActive = true;
			stuassessmentToken.IsDeleted = false;
			stuassessmentToken.ModifiedOn = DateTime.Now;
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
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
			stuassessmentToken.UserId = _user.Id;
			_tokenRepository.Insert(stuassessmentToken);
			assessTemplate.Tokens.Add(assessmentToken);
			stuAccessTemplate.Tokens.Add(assessmentToken);

			#endregion

			_templateRepository.Insert(assessTemplate);
			_templateRepository.Insert(stuAccessTemplate);

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
			defaultTemplate.UserId = _user.Id;
			_templateRepository.Insert(defaultTemplate);

			defaultTemplate = new Template();
			defaultTemplate.Name = "CommentOnEvent";
			defaultTemplate.IsActive = true;
			defaultTemplate.IsDeleted = false;
			defaultTemplate.IsSystemDefined = true;
			defaultTemplate.ModifiedOn = DateTime.Now;
			defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [VisitorName],</td></tr><tr><td colspan='2'>Thanks for your comment.</td></tr></tbody></table>";
			defaultTemplate.CreatedOn = DateTime.Now;
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

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
			defaultTemplate.UserId = _user.Id;

			defaultTemplate.Tokens.Add(usercreationdateToken);
			defaultTemplate.Tokens.Add(useridToken);
			defaultTemplate.Tokens.Add(usermailToken);
			defaultTemplate.Tokens.Add(usernameToken);

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
				UserId = _user.Id
			});

			#endregion

			#region Question Types

			var questionType = new QuestionType();
			questionType.Name = "Multiple Choice";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Multiple Response";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Fill in the blanks";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Match Following";
			questionType.CreatedOn = DateTime.Now;
			questionType.ModifiedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Match Matrix";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Essay";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			questionType = new QuestionType();
			questionType.Name = "Single Digit";
			questionType.CreatedOn = DateTime.Now;
			questionType.IsSystemDefined = true;
			questionType.ModifiedOn = DateTime.Now;
			questionType.UserId = _user.Id;
			_questionTypeRepository.Insert(questionType);

			#endregion

			// Save School URL Record
			_customPageUrlRepository.Insert(new CustomPageUrl
			{
				EntityId = school.Id,
				EntityName = "School",
				CreatedOn = DateTime.Now,
				ModifiedOn = DateTime.Now,
				UserId = _user.Id,
				IsActive = true,
				Slug = school.ValidateSystemName("", school.FullName, true)
			});

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
			timeTableSetting.UserId = _user.Id;
			_timeTableSettingRepository.Insert(timeTableSetting);

			// Update user at last
			_userRepository.Update(_user);
		}
	}
}
