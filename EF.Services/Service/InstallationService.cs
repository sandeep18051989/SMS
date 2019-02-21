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
        private readonly IRepository<SocialProvider> _socialProviderRepository;


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
     IRepository<Book> bookRepository,
     IRepository<SocialProvider> socialProviderRepository)
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
            this._socialProviderRepository = socialProviderRepository;
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
            user.UserId = 1;

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
            managePermit.Name = "Manage Designations";
            managePermit.SystemName = "ManageDesignations";
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

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Acadmic Year";
            managePermit.SystemName = "ManageAcadmicYear";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "AcadmicYear";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Book";
            managePermit.SystemName = "ManageBook";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Book";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Holiday";
            managePermit.SystemName = "ManageHoliday";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Holiday";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Homework";
            managePermit.SystemName = "ManageHomework";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Homework";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage House";
            managePermit.SystemName = "ManageHouse";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "House";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Question";
            managePermit.SystemName = "ManageQuestion";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Question";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Assessment";
            managePermit.SystemName = "ManageAssessment";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Assessment";
            managePermit.ModifiedOn = DateTime.Now;
            managePermit.CreatedOn = DateTime.Now;
            managePermit.IsActive = true;
            managePermit.UserId = user.Id;
            _permissionRecordRepository.Insert(managePermit);

            managePermit = new PermissionRecord();
            managePermit.Name = "Manage Social Settings";
            managePermit.SystemName = "ManageSocialSettings";
            managePermit.IsDeleted = false;
            managePermit.IsSystemDefined = true;
            managePermit.Category = "Social Settings";
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

            // Student Role
            var studentRole = new UserRole();
            studentRole.CreatedOn = DateTime.Now;
            studentRole.ModifiedOn = DateTime.Now;
            studentRole.RoleName = "Student";
            studentRole.IsSystemDefined = true;
            studentRole.IsDeleted = false;
            studentRole.IsActive = true;
            studentRole.UserId = user.Id;
            _userRoleRepository.Insert(studentRole);

            // Teacher Role
            var teacherRole = new UserRole();
            teacherRole.CreatedOn = DateTime.Now;
            teacherRole.ModifiedOn = DateTime.Now;
            teacherRole.RoleName = "Teacher";
            teacherRole.IsSystemDefined = true;
            teacherRole.IsDeleted = false;
            teacherRole.IsActive = true;
            teacherRole.UserId = user.Id;
            _userRoleRepository.Insert(teacherRole);

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
            setting.Name = "ForgotPassword";
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
            setting.Value = "EmailVerification";
            setting.Name = "Email Verification";
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
            setting.Value = "ResetPassword";
            setting.Name = "Reset Password";
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
            emailSetting.Name = "DisplayName";
            emailSetting.Value = "SMS";
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

            var userverifylinkToken = new DataToken();
            userverifylinkToken.SystemName = "VerificationLink";
            userverifylinkToken.Name = "User Verification Link";
            userverifylinkToken.Value = "#";
            userverifylinkToken.IsSystemDefined = true;
            userverifylinkToken.CreatedOn = DateTime.Now;
            userverifylinkToken.IsActive = true;
            userverifylinkToken.IsDeleted = false;
            userverifylinkToken.ModifiedOn = DateTime.Now;
            userverifylinkToken.UserId = user.Id;
            _tokenRepository.Insert(userverifylinkToken);

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
            assessTemplate.Subject = "Assessment Scheduled";
            assessTemplate.UserId = user.Id;

            var stuAccessTemplate = new Template();
            stuAccessTemplate.Name = "AssessmentCompleted";
            stuAccessTemplate.IsActive = true;
            stuAccessTemplate.IsDeleted = false;
            stuAccessTemplate.IsSystemDefined = true;
            stuAccessTemplate.ModifiedOn = DateTime.Now;
            stuAccessTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[AssessmentName],</td></tr><tr><td colspan='2'>Assessment Has Been Completed.</td></tr></tbody></table>";
            stuAccessTemplate.CreatedOn = DateTime.Now;
            assessTemplate.Subject = "Assessment Completed";
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

            #region Logos

            var dtheaderLogo = new DataToken();
            dtheaderLogo.Name = "Header Logo(Only Enter Base 64 String, If Using Image)";
            dtheaderLogo.SystemName = "HeaderLogo";
            dtheaderLogo.Value = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MjY0QjM3MDUxNDZCMTFFODgzREZGMUVCMERBODM4MUIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MjY0QjM3MDYxNDZCMTFFODgzREZGMUVCMERBODM4MUIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoyNjRCMzcwMzE0NkIxMUU4ODNERkYxRUIwREE4MzgxQiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDoyNjRCMzcwNDE0NkIxMUU4ODNERkYxRUIwREE4MzgxQiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PgLCzP0AABLkSURBVHjaxFp7fFTV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";
            dtheaderLogo.IsSystemDefined = true;
            dtheaderLogo.CreatedOn = DateTime.Now;
            dtheaderLogo.IsActive = true;
            dtheaderLogo.IsDeleted = false;
            dtheaderLogo.ModifiedOn = DateTime.Now;
            dtheaderLogo.UserId = user.Id;
            _tokenRepository.Insert(dtheaderLogo);

            var dtfooterLogo = new DataToken();
            dtfooterLogo.Name = "Footer Logo(Only Enter Base 64 String, If Using Image)";
            dtfooterLogo.SystemName = "FooterLogo";
            dtfooterLogo.Value = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6N0U5MEM0MDcxNDZBMTFFOEFDRjg5NzA3Q0I4MTlGNzUiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6N0U5MEM0MDgxNDZBMTFFOEFDRjg5NzA3Q0I4MTlGNzUiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo3RTkwQzQwNTE0NkExMUU4QUNGODk3MDdDQjgxOUY3NSIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo3RTkwQzQwNjE0NkExMUU4QUNGODk3MDdDQjgxOUY3NSIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PhYt22cAACMiSURBVHja3HwJeFRV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";
            dtfooterLogo.IsSystemDefined = true;
            dtfooterLogo.CreatedOn = DateTime.Now;
            dtfooterLogo.IsActive = true;
            dtfooterLogo.IsDeleted = false;
            dtfooterLogo.ModifiedOn = DateTime.Now;
            dtfooterLogo.UserId = user.Id;
            _tokenRepository.Insert(dtfooterLogo);

            #endregion

            #endregion

            #region Default Email Templates

            var defaultTemplate = new Template();
            defaultTemplate.Name = "VisitorQueryPlaced";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [VisitorName],</td></tr><tr><td colspan='2'>Thanks for sending us query. We will be right back to you shortly.</td></tr></tbody><tfoot><tr><td colspan='2'>Thanks.<br>SMS Admin</td></tr></tfoot></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Visitor Query";
            defaultTemplate.Tokens.Add(visitorToken);
            defaultTemplate.UserId = user.Id;

            // Add Data Tokens
            defaultTemplate.Tokens.Add(visitorToken);
            defaultTemplate.Tokens.Add(fContactToken);
            defaultTemplate.Tokens.Add(fDescriptionToken);
            defaultTemplate.Tokens.Add(fEmailToken);
            defaultTemplate.Tokens.Add(fIdToken);
            defaultTemplate.Tokens.Add(fLocationToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);
            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "CommentOnEvent";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [VisitorName],</td></tr><tr><td colspan='2'>Thanks for your comment.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Comment On Event";
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
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "CommentOnProduct";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [ProductUser],</td></tr><tr><td colspan='2'>Thanks for your comment.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Comment On Product";
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
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "ProductAdded";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Hello [UserName],</td></tr><tr><td colspan='2'>Product Added Successfully.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Product Added";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(productDescToken);
            defaultTemplate.Tokens.Add(productIdToken);
            defaultTemplate.Tokens.Add(productNameToken);
            defaultTemplate.Tokens.Add(productSeoToken);
            defaultTemplate.Tokens.Add(productTitleToken);
            defaultTemplate.Tokens.Add(productUserToken);
            defaultTemplate.Tokens.Add(usernameToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "ReplyOnComment";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] replied to a comment.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Reply On Comment";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(commentIdToken);
            defaultTemplate.Tokens.Add(commentHtmlToken);
            defaultTemplate.Tokens.Add(commentUserToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "NewUserRegister";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] registered successfully.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "New User Registration";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(usercreationdateToken);
            defaultTemplate.Tokens.Add(useridToken);
            defaultTemplate.Tokens.Add(usermailToken);
            defaultTemplate.Tokens.Add(usernameToken);
            defaultTemplate.Tokens.Add(userverifylinkToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "UserSignInAttempt";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserName] just signed in into the system.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "User Signed In";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(usercreationdateToken);
            defaultTemplate.Tokens.Add(useridToken);
            defaultTemplate.Tokens.Add(usermailToken);
            defaultTemplate.Tokens.Add(usernameToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "RequestQuote";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[VisitorName] thanks for your quote.</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "New Quote";
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
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "ForgetPassword";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[UserEmail]</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Forgot Password";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(usercreationdateToken);
            defaultTemplate.Tokens.Add(useridToken);
            defaultTemplate.Tokens.Add(usermailToken);
            defaultTemplate.Tokens.Add(userpasswordToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            defaultTemplate = new Template();
            defaultTemplate.Name = "CommentOnBlog";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>[BlogName]</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Comment On Blog";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(blogActiveToken);
            defaultTemplate.Tokens.Add(blogApprovedToken);
            defaultTemplate.Tokens.Add(blogEmailToken);
            defaultTemplate.Tokens.Add(blogIdToken);
            defaultTemplate.Tokens.Add(blogNameToken);
            defaultTemplate.Tokens.Add(blogSubjectToken);
            defaultTemplate.Tokens.Add(blogUrlToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);
            defaultTemplate.Tokens.Add(blogUserIdToken);

            _templateRepository.Insert(defaultTemplate);

            // User Verification Template
            var verificationTemplate = new Template();
            verificationTemplate.Name = "EmailVerification";
            verificationTemplate.IsActive = true;
            verificationTemplate.IsDeleted = false;
            verificationTemplate.IsSystemDefined = true;
            verificationTemplate.ModifiedOn = DateTime.Now;
            verificationTemplate.BodyHtml = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /><title>Reset Password</title></head><body><div style='padding-left: 50px;padding-right:50px;padding-bottom:15px;padding-top:15px; width: 100%; border-bottom: 1px solid #f1f1f1;color:gray'><div class='companyLogo'>[Logo]</div><br /><h4>Hi, {User}<br /></h4><div><span>You recently requested to change your password for Invoice Grazitti. Click the link below to Reset your password.</span></div><div style='text-align: center; padding: 20px;'><a href='{Url}' style='color: #fff; background-color: #5bc0de; border-color: #46b8da; display: inline-block; padding: 6px 12px; margin-bottom: 0; font-size: 14px; font-weight: 400; line-height: 1.42857143; text-align: center;text-transform:none'>Reset Your Password</a></div><div style='padding-bottom:15px;padding-top:15px;width: 100%; '><span>If you are having trouble clicking the Reset Password button, copy and paste the URL below into your browser </span><div style='padding: 20px'><a href='{Url}'>{Url}</a></div></div></div></body></html>";
            verificationTemplate.CreatedOn = DateTime.Now;
            verificationTemplate.Subject = "Email Verification";
            verificationTemplate.UserId = user.Id;

            verificationTemplate.Tokens.Add(usernameToken);
            verificationTemplate.Tokens.Add(userpasswordToken);
            defaultTemplate.Tokens.Add(userverifylinkToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(verificationTemplate);

            // Reset Password Template
            verificationTemplate = new Template();
            verificationTemplate.Name = "ResetPassword";
            verificationTemplate.IsActive = true;
            verificationTemplate.IsDeleted = false;
            verificationTemplate.IsSystemDefined = true;
            verificationTemplate.ModifiedOn = DateTime.Now;
            verificationTemplate.BodyHtml = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /><title>Reset Password</title></head><body>[RedirectUrl]<div style='padding-left: 50px;padding-right:50px;padding-bottom:15px;padding-top:15px; width: 100%; border-bottom: 1px solid #f1f1f1;color:gray'><div class='companyLogo'>[Logo]</div><br /><h4>Hi, {User}<br /></h4><div><span>You recently requested to change your password for Invoice Grazitti. Click the link below to Reset your password.</span></div><div style='text-align: center; padding: 20px;'><a href='{Url}' style='color: #fff; background-color: #5bc0de; border-color: #46b8da; display: inline-block; padding: 6px 12px; margin-bottom: 0; font-size: 14px; font-weight: 400; line-height: 1.42857143; text-align: center;text-transform:none'>Reset Your Password</a></div><div style='padding-bottom:15px;padding-top:15px;width: 100%; '><span>If you are having trouble clicking the Reset Password button, copy and paste the URL below into your browser </span><div style='padding: 20px'><a href='{Url}'>{Url}</a></div></div></div></body></html>";
            verificationTemplate.CreatedOn = DateTime.Now;
            verificationTemplate.Subject = "Password Reset Request";
            verificationTemplate.UserId = user.Id;

            verificationTemplate.Tokens.Add(usernameToken);
            verificationTemplate.Tokens.Add(userpasswordToken);
            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(verificationTemplate);

            // Terms And Conditions Template
            defaultTemplate = new Template();
            defaultTemplate.Name = "TermsAndConditions";
            defaultTemplate.IsActive = true;
            defaultTemplate.IsDeleted = false;
            defaultTemplate.IsSystemDefined = true;
            defaultTemplate.ModifiedOn = DateTime.Now;
            defaultTemplate.BodyHtml = "<table cell-padding='10' border-spacing='2' Width='100%' ><tbody><tr><td colspan='2'>Terms And Conditions</td></tr></tbody></table>";
            defaultTemplate.CreatedOn = DateTime.Now;
            defaultTemplate.Subject = "Terms & Conditions";
            defaultTemplate.UserId = user.Id;

            defaultTemplate.Tokens.Add(dtheaderLogo);
            defaultTemplate.Tokens.Add(dtfooterLogo);

            _templateRepository.Insert(defaultTemplate);

            // Default Custom Pages
            var termAndCondition = new CustomPage();
            termAndCondition.TemplateId = defaultTemplate.Id;
            termAndCondition.AcadmicYearId = acadmicYear.Id;
            termAndCondition.BodyHtml = "";
            termAndCondition.CreatedOn = termAndCondition.ModifiedOn = DateTime.Now;
            termAndCondition.DisplayOrder = 1;
            termAndCondition.IncludeInFooterColumn1 = false;
            termAndCondition.IncludeInFooterColumn2 = false;
            termAndCondition.IncludeInFooterColumn3 = false;
            termAndCondition.IncludeInFooterMenu = false;
            termAndCondition.IncludeInTopMenu = false;
            termAndCondition.IsActive = true;
            termAndCondition.IsDeleted = false;
            termAndCondition.IsSystemDefined = true;
            termAndCondition.MetaDescription = "";
            termAndCondition.MetaKeywords = "";
            termAndCondition.MetaTitle = "";
            termAndCondition.Name = "Terms & Conditions";
            termAndCondition.PermissionOriented = false;
            termAndCondition.SystemName = "TermsAndConditions";
            termAndCondition.Url = "";
            termAndCondition.UserId = user.Id;
            _customPageRepository.Insert(termAndCondition);

            // Save URL Record
            termAndCondition.SystemName = termAndCondition.ValidateSystemName(termAndCondition.SystemName, termAndCondition.Name, true);
            _urlService.SaveSlug(termAndCondition, termAndCondition.SystemName);

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
            questionType.Name = "Essay";
            questionType.CreatedOn = DateTime.Now;
            questionType.IsSystemDefined = true;
            questionType.ModifiedOn = DateTime.Now;
            questionType.UserId = user.Id;
            _questionTypeRepository.Insert(questionType);

            questionType = new QuestionType();
            questionType.Name = "Choose Answer";
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
            addDivision.DisplayOrder = 0;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "B";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            addDivision.DisplayOrder = 1;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "C";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            addDivision.DisplayOrder = 2;
            _divisionRepository.Insert(addDivision);

            addDivision = new Division();
            addDivision.AcadmicYearId = acadmicYear.Id;
            addDivision.Name = "D";
            addDivision.Description = "Default";
            addDivision.IsActive = true;
            addDivision.IsDeleted = false;
            addDivision.ModifiedOn = addDivision.CreatedOn = DateTime.Now;
            addDivision.UserId = user.Id;
            addDivision.DisplayOrder = 3;
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
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Independence Day";
            addHoliday.UserId = user.Id;
            _holidayRepository.Insert(addHoliday);

            addHoliday = new Holiday();
            addHoliday.AcadmicYearId = acadmicYear.Id;
            addHoliday.CreatedOn = addHoliday.ModifiedOn = DateTime.Now;
            addHoliday.Date = new DateTime(DateTime.Now.Year, 01, 26);
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Republic Day";
            addHoliday.UserId = user.Id;
            _holidayRepository.Insert(addHoliday);

            addHoliday = new Holiday();
            addHoliday.AcadmicYearId = acadmicYear.Id;
            addHoliday.CreatedOn = addHoliday.ModifiedOn = DateTime.Now;
            addHoliday.Date = new DateTime(DateTime.Now.Year, 12, 25);
            addHoliday.IsDeleted = false;
            addHoliday.Name = "Christmas";
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
            addSubject.Code = "A04";
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
            addSubject.Code = "A05";
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
            addSubject.Code = "A029";
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

            var socialEngine = new SocialProvider();
            socialEngine.AuthenticationMethodService = "FacebookSocialAuthMethod";
            socialEngine.AuthenticationMethodServiceNamespace = "EF.Facebook";
            socialEngine.CreatedOn = socialEngine.ModifiedOn = DateTime.Now;
            socialEngine.IsActive = true;
            socialEngine.ProviderSystemName = "Facebook";
            socialEngine.UserId = user.Id;
            socialEngine.Version = "1.0";
            socialEngine.DisplayIdentifier = "Facebook";
            socialEngine.AssemblyName = "EF.Facebook";
            socialEngine.IsInstalled = true;
            _socialProviderRepository.Insert(socialEngine);

            // Update user at last
            _userRepository.Update(user);
        }
    }
}
