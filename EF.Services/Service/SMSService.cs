using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Data;

namespace EF.Services.Service
{
    public class SMSService : ISMSService
    {
        #region Constructor
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IRepository<StudentAttendance> _studentAttendanceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Religion> _religionRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<EmployeeAttendance> _employeeAttendanceRepository;
        private readonly IRepository<FeeCategory> _feeCategoryRepository;
        private readonly IRepository<Caste> _casteRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Class> _classRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<Division> _divisionRepository;
        private readonly IRepository<DivisionSubject> _divisionClassSubjectRepository;
        private readonly IRepository<Designation> _designationRepository;
        private readonly IRepository<TimeTableSetting> _dailyTimeTableSettingRepository;
        private readonly IRepository<TimeTable> _dailyTimeTableRepository;
        private readonly IRepository<Qualification> _qualificationRepository;
        private readonly IRepository<Allowance> _allowanceRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IRepository<FeeDetail> _feeDetailRepository;
        private readonly IRepository<MessageGroup> _messageGroupRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Student_MessageGroup> _studentMessageGroupRepository;
        private readonly IRepository<Exam> _examRepository;
        private readonly IRepository<Reaction> _reactionRepository;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IFileService _fileService;
        private readonly IPictureService _pictureService;
        private readonly IEventService _eventService;
        private readonly IRepository<AcadmicYear> _acadmicYearRepository;
        private readonly IRepository<School> _schoolRepository;
        private readonly IRepository<QuestionType> _questionTypeRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Option> _optionRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<AssessmentStudent> _studentAssessmentRepository;
        private readonly IRepository<AssessmentQuestion> _assesQuestionRepository;
        private readonly IRepository<Homework> _homeworkRepository;
        private readonly IRepository<ClassRoomDivision> _classDivisionRepository;
        private readonly IRepository<ClassRoom> _classRoomRepository;
        private readonly IRepository<Settings> _settingRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IRepository<DivisionHomework> _divisionHomeworkRepository;
        private readonly IRepository<DivisionExam> _divisionExamRepository;
        private readonly IRepository<House> _houseRepository;
        private readonly IRepository<StudentExam> _studentExamRepository;
        private readonly IRepository<TeacherExam> _teacherExamRepository;
        private readonly IRepository<BookIssue> _bookIssueRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<AssessmentStudent> _assessmentStudentRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ProductCategoryMapping> _productCategoryMappingRepository;

        public SMSService(IRepository<Student> studentRepository,
        IRepository<Teacher> teacherRepository,
        IRepository<StudentAttendance> studentAttendanceRepository,
        IRepository<User> userRepository,
        IRepository<Religion> religionRepository,
        IRepository<Employee> employeeRepository,
        IRepository<EmployeeAttendance> employeeAttendanceRepository,
        IRepository<FeeCategory> feeCategoryRepository,
        IRepository<Caste> casteRepository,
        IRepository<Category> categoryRepository,
        IRepository<Class> classRepository,
        IRepository<Subject> subjectRepository,
        IRepository<Division> divisionRepository,
        IRepository<DivisionSubject> divisionClassSubjectRepository,
        IRepository<Designation> designationRepository,
        IRepository<TimeTableSetting> dailyTimeTableSettingRepository,
        IRepository<TimeTable> dailyTimeTableRepository,
        IRepository<Qualification> qualificationRepository,
        IRepository<Allowance> allowanceRepository,
        IRepository<Payment> paymentRepository,
        IRepository<Vendor> vendorRepository,
        IRepository<Product> productRepository,
        IRepository<Purchase> purchaseRepository,
        IRepository<FeeDetail> feeDetailRepository,
        IRepository<MessageGroup> messageGroupRepository,
        IRepository<Message> messageRepository,
        IRepository<Student_MessageGroup> studentMessageGroupRepository,
        IRepository<Exam> examRepository,
        ICommentService commentService, IReplyService replyService,
        IFileService fileService,
        IEventService eventService,
        IPictureService pictureService,
        IRepository<AcadmicYear> acadmicYearRepository,
        IRepository<Reaction> reactionRepository,
        IRepository<School> schoolRepository,
        IDataProvider dataProvider,
        IRepository<QuestionType> questionTypeRepository,
        IRepository<Question> questionRepository,
        IRepository<Option> optionRepository,
        IRepository<Assessment> assessmentRepository,
        IRepository<AssessmentStudent> studentAssessmentRepository,
        IRepository<AssessmentQuestion> assesQuestionRepository,
        IRepository<Homework> homeworkRepository,
        IRepository<ClassRoomDivision> classDivisionRepository,
        IRepository<ClassRoom> classRoomRepository,
        IRepository<Settings> settingRepository,
        IRepository<DivisionHomework> divisionHomeworkRepository,
        IRepository<DivisionExam> divisionExamRepository,
        IRepository<House> houseRepository,
        IRepository<StudentExam> studentExamRepository,
        IRepository<TeacherExam> teacherExamRepository,
        IRepository<Book> bookRepository,
        IRepository<BookIssue> bookIssueRepository,
        IRepository<Holiday> holidayRepository,
        IRepository<AssessmentStudent> assessmentStudentRepository,
        IRepository<ProductCategory> productCategoryRepository,
        IRepository<ProductCategoryMapping> productCategoryMappingRepository,
        IDbContext dbContext)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _studentAttendanceRepository = studentAttendanceRepository;
            _userRepository = userRepository;
            _religionRepository = religionRepository;
            _employeeRepository = employeeRepository;
            _employeeAttendanceRepository = employeeAttendanceRepository;
            _feeCategoryRepository = feeCategoryRepository;
            _casteRepository = casteRepository;
            _categoryRepository = categoryRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _divisionRepository = divisionRepository;
            _divisionClassSubjectRepository = divisionClassSubjectRepository;
            _designationRepository = designationRepository;
            _dailyTimeTableSettingRepository = dailyTimeTableSettingRepository;
            _dailyTimeTableRepository = dailyTimeTableRepository;
            _qualificationRepository = qualificationRepository;
            _allowanceRepository = allowanceRepository;
            _paymentRepository = paymentRepository;
            _vendorRepository = vendorRepository;
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
            _feeDetailRepository = feeDetailRepository;
            _messageGroupRepository = messageGroupRepository;
            _messageRepository = messageRepository;
            _studentMessageGroupRepository = studentMessageGroupRepository;
            _examRepository = examRepository;
            _commentService = commentService;
            _replyService = replyService;
            _fileService = fileService;
            _eventService = eventService;
            _pictureService = pictureService;
            _acadmicYearRepository = acadmicYearRepository;
            _reactionRepository = reactionRepository;
            _schoolRepository = schoolRepository;
            _dataProvider = dataProvider;
            _dbContext = dbContext;
            _questionTypeRepository = questionTypeRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _assessmentRepository = assessmentRepository;
            _studentAssessmentRepository = studentAssessmentRepository;
            _assesQuestionRepository = assesQuestionRepository;
            _homeworkRepository = homeworkRepository;
            _classDivisionRepository = classDivisionRepository;
            _classRoomRepository = classRoomRepository;
            _settingRepository = settingRepository;
            _divisionHomeworkRepository = divisionHomeworkRepository;
            _divisionExamRepository = divisionExamRepository;
            this._houseRepository = houseRepository;
            this._studentExamRepository = studentExamRepository;
            this._teacherExamRepository = teacherExamRepository;
            this._bookRepository = bookRepository;
            this._bookIssueRepository = bookIssueRepository;
            this._holidayRepository = holidayRepository;
            this._assessmentStudentRepository = assessmentStudentRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._productCategoryMappingRepository = productCategoryMappingRepository;
        }
        #endregion

        #region ISMSService Members

        #region School
        public void InsertSchool(School school)
        {
            _schoolRepository.Insert(school);
        }
        public void UpdateSchool(School school)
        {
            _schoolRepository.Update(school);
        }
        public void DeleteSchool(int id)
        {
            var school = _schoolRepository.GetByID(id);
            if (school != null)
            {
                school.IsActive = false;
                school.IsDeleted = true;
                _schoolRepository.Update(school);
            }
        }
        public IList<School> GetAllSchools(bool? onlyActive = null)
        {
            return _schoolRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public School GetSchoolById(int id)
        {
            if (id == 0)
                throw new System.Exception("School Is Missing.");

            return _schoolRepository.GetByID(id);
        }
        #endregion

        #region Student
        public void InsertStudent(Student student)
        {
            _studentRepository.Insert(student);
        }
        public void UpdateStudent(Student student)
        {
            _studentRepository.Update(student);
        }
        public void DeleteStudent(int id)
        {
            var student = _studentRepository.GetByID(id);
            if (student != null)
            {
                student.IsActive = false;
                student.IsDeleted = true;
                _studentRepository.Update(student);
            }
        }
        public IList<Student> GetAllStudents(bool? onlyActive = null)
        {
            return _studentRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).OrderBy(x => x.FName).ThenBy(x => x.MName).ThenBy(x => x.LName).ToList();
        }
        public Student GetStudentById(int id)
        {
            if (id == 0)
                throw new System.Exception("Student Is Missing.");

            return _studentRepository.GetByID(id);
        }
        public Student GetStudentByImpersonateId(int id)
        {
            if (id == 0)
                throw new System.Exception("Student impersonate id is missing.");

            return _studentRepository.Table.FirstOrDefault(x => x.ImpersonateId == id);
        }
        public IList<Student> GetStudentsByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Student Name is Missing.");

            var query = _studentRepository.Table.Where(a => (a.FName.ToLower().Contains(name.ToLower()) || a.LName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.FName).ToList();
        }
        public IList<Student> SearchStudents(bool? onlyActive = null, int? classid = null, int? acedemicyearid = null)
        {
            var query = _studentRepository.Table.ToList();

            if (onlyActive.HasValue)
                query = query.Where(s => s.IsActive == onlyActive).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            if (classid > 0)
                query = query.Where(s => s.ClassRoomDivision?.Class.Id == classid).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(s => s.FName).ToList();

        }
        public bool CheckStudentExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _studentRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.FName.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusStudent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objStudent = _studentRepository.GetByID(id);
            if (objStudent != null)
            {
                objStudent.IsActive = !objStudent.IsActive;
                objStudent.ModifiedOn = DateTime.Now;
                _studentRepository.Update(objStudent);
            }

        }
        public bool CheckUsernameExistsForStudent(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("Student Username is Missing.");

            return _studentRepository.Table.Any(a => a.UserName.Trim().ToLower() == username.Trim().ToLower() && a.IsDeleted == false);
        }
        public IList<Student> GetStudentsByDivision(int id)
        {
            return _studentRepository.Table.Where(x => x.ClassRoomDivisionId == id).ToList();
        }
        public IList<StudentExam> GetAllStudentExamMappings()
        {
            return _studentExamRepository.Table.ToList();
        }
        public Student GetStudentByImpersonatedUser(int userid)
        {
            return _studentRepository.Table.FirstOrDefault(x => x.ImpersonateId == userid);
        }
        #endregion

        #region Student Exam

        public void InsertStudentExam(StudentExam StudentExam)
        {
            _studentExamRepository.Insert(StudentExam);
        }
        public void UpdateStudentExam(StudentExam StudentExam)
        {
            _studentExamRepository.Update(StudentExam);
        }
        public void DeleteStudentExam(int id)
        {
            var StudentExam = _studentExamRepository.GetByID(id);
            if (StudentExam != null)
                _studentExamRepository.Update(StudentExam);
        }

        public StudentExam GetStudentExamMappingById(int id)
        {
            if (id == 0)
                throw new System.Exception("Student Exam Mapping Id Is Missing.");

            return _studentExamRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        public IList<StudentExam> GetAllExamsByStudent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _studentExamRepository.Table.Where(x => x.StudentId == id).ToList();
        }

        #endregion

        #region Employee
        public void InsertEmployee(Employee employee)
        {
            _employeeRepository.Insert(employee);
        }
        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.Update(employee);
        }
        public void DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetByID(id);
            if (employee != null)
            {
                employee.IsActive = false;
                employee.IsDeleted = true;
                _employeeRepository.Update(employee);
            }
        }
        public IList<Employee> GetAllEmployees(bool? onlyActive = null)
        {
            return _employeeRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Employee GetEmployeeById(int id)
        {
            if (id == 0)
                throw new System.Exception("Employee Id Is Missing.");

            return _employeeRepository.GetByID(id);
        }
        public bool CheckEmployeeExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _employeeRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.EmpFName.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusEmployee(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objEmployee = _employeeRepository.GetByID(id);
            if (objEmployee != null)
            {
                objEmployee.IsActive = !objEmployee.IsActive;
                objEmployee.ModifiedOn = DateTime.Now;
                _employeeRepository.Update(objEmployee);
            }

        }
        public bool CheckUsernameExistsForEmployee(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("Employee Username is Missing.");

            return _employeeRepository.Table.Any(a => a.Username.Trim().ToLower() == username.Trim().ToLower() && a.IsDeleted == false);
        }
        #endregion

        #region Student Attendance
        public void InsertStudentAttendance(StudentAttendance studentattendance)
        {
            _studentAttendanceRepository.Insert(studentattendance);
        }
        public void UpdateStudentAttendance(StudentAttendance studentattendance)
        {
            _studentAttendanceRepository.Update(studentattendance);
        }
        public void DeleteStudentAttendance(int id)
        {
            var studentattendance = _studentAttendanceRepository.GetByID(id);
            if (studentattendance != null)
                _studentAttendanceRepository.Delete(studentattendance);
        }
        public StudentAttendance GetStudentAttendanceById(int id)
        {
            if (id == 0)
                throw new System.Exception("Student Attendance Id Is Missing.");

            return _studentAttendanceRepository.GetByID(id);
        }
        public IList<StudentAttendance> GetStudentAttendanceByDate(int DD, int MM, int yyyy)
        {
            return _studentAttendanceRepository.Table.Where(a => (a.DD == DD && a.MM == MM && a.YYYY == yyyy)).ToList();
        }
        public IList<StudentAttendance> SearchStudentAttendances(bool? active, string studentusername = null, string classname = null, int? acedemicyearid = null)
        {
            var query = _studentAttendanceRepository.Table.ToList();

            if (!String.IsNullOrEmpty(studentusername))
                query = query.Where(s => s.Student.UserName.Trim().ToLower() == studentusername.Trim().ToLower()).ToList();

            if (!String.IsNullOrEmpty(classname))
                query = query.Where(s => s.Student.ClassRoomDivision?.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

            return query.OrderByDescending(s => s.Date).ToList();

        }

        #endregion

        #region Religion
        public void InsertReligion(Religion religion)
        {
            _religionRepository.Insert(religion);
        }
        public void UpdateReligion(Religion religion)
        {
            _religionRepository.Update(religion);
        }
        public void DeleteReligion(int id)
        {
            var religion = _religionRepository.GetByID(id);
            if (religion != null)
                _religionRepository.Delete(religion);
        }
        public IList<Religion> GetAllReligions()
        {
            return _religionRepository.GetAll().ToList();
        }
        public Religion GetReligionById(int id)
        {
            if (id == 0)
                throw new System.Exception("Religion Id Is Missing.");

            return _religionRepository.GetByID(id);
        }
        public IList<Religion> GetReligionByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Religion Name is missing.");

            var query = _religionRepository.Table.Where(a => (a.Name.Trim().ToLower() == name.Trim().ToLower())).ToList();
            return query.OrderBy(x => x.Name).ToList();
        }
        public bool CheckReligionExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _religionRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }

        #endregion

        #region EmployeeAttendance
        public void InsertEmployeeAttendance(EmployeeAttendance employeeattendance)
        {
            _employeeAttendanceRepository.Insert(employeeattendance);
        }
        public void UpdateEmployeeAttendance(EmployeeAttendance employeeattendance)
        {
            _employeeAttendanceRepository.Update(employeeattendance);
        }
        public void DeleteEmployeeAttendance(int id)
        {
            var employeeattendance = _employeeAttendanceRepository.GetByID(id);
            if (employeeattendance != null)
                _employeeAttendanceRepository.Delete(employeeattendance);
        }
        public EmployeeAttendance GetEmployeeAttendanceById(int id)
        {
            if (id == 0)
                throw new System.Exception("Employee Attendance Id Is Missing.");

            return _employeeAttendanceRepository.GetByID(id);
        }

        #region Procedure Support

        public IPagedList<EmployeeAttendance> GetEmployeeAttendanceByDateAndEmployee(PagerParams param, DateTime? Date, int EmployeeId = 0)
        {
            //prepare parameters
            var pEmployeeId = _dataProvider.GetParameter();
            pEmployeeId.ParameterName = "employeeid";
            pEmployeeId.Value = EmployeeId;
            pEmployeeId.DbType = DbType.Int32;

            var pDate = _dataProvider.GetParameter();
            pDate.ParameterName = "date";
            pDate.Value = Date.HasValue ? (object)Date.Value : DBNull.Value;
            pDate.DbType = DbType.DateTime;

            var pageIndexParameter = _dataProvider.GetParameter();
            pageIndexParameter.ParameterName = "i_Page_Index";
            pageIndexParameter.Value = param.PageIndex;
            pageIndexParameter.DbType = DbType.Int32;

            var pageSizeParameter = _dataProvider.GetParameter();
            pageSizeParameter.ParameterName = "i_Page_Count";
            pageSizeParameter.Value = param.PageSize;
            pageSizeParameter.DbType = DbType.Int32;

            var totalRecordsParameter = _dataProvider.GetParameter();
            totalRecordsParameter.ParameterName = "o_total_rows";
            totalRecordsParameter.Direction = ParameterDirection.Output;
            totalRecordsParameter.DbType = DbType.Int32;

            //invoke stored procedure
            var employeeAttendance = _dbContext.ExecuteStoredProcedureList<EmployeeAttendance>("GetEmployeeAttendanceWithPaging", pageIndexParameter, pageSizeParameter, pDate, pEmployeeId);

            int totalRecords = (totalRecordsParameter.Value != DBNull.Value) ? Convert.ToInt32(totalRecordsParameter.Value) : 0;
            return new PagedList<EmployeeAttendance>(employeeAttendance, param.PageIndex, param.PageSize, totalRecords);
        }

        #endregion


        #endregion

        #region Fee Category
        public void InsertFeeCategory(FeeCategory feecategory)
        {
            _feeCategoryRepository.Insert(feecategory);
        }
        public void UpdateFeeCategory(FeeCategory feecategory)
        {
            _feeCategoryRepository.Update(feecategory);
        }
        public void DeleteFeeCategory(int id)
        {
            var feecategory = _feeCategoryRepository.GetByID(id);
            if (feecategory != null)
            {
                feecategory.IsActive = false;
                feecategory.IsDeleted = true;
                _feeCategoryRepository.Update(feecategory);
            }
        }
        public IList<FeeCategory> GetAllFeeCategories(bool? onlyActive = null)
        {
            return _feeCategoryRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).OrderBy(x => x.CategoryName).ToList();
        }
        public FeeCategory GetFeeCategoryById(int id)
        {
            if (id == 0)
                throw new System.Exception("FeeCategory Id Is Missing.");

            return _feeCategoryRepository.GetByID(id);
        }
        public IList<FeeCategory> GetFeeCategoryByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("FeeCategory Name is Missing.");

            var query = _feeCategoryRepository.Table.Where(a => (a.CategoryName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.CategoryName).ToList();
        }
        public IList<FeeCategory> SearchFeeCategories(bool? active, string category = null, string classname = null, int? acadmicyearid = null)
        {
            var query = _feeCategoryRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acadmicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acadmicyearid.Value).ToList();

            if (!String.IsNullOrEmpty(category))
                query = query.Where(s => s.CategoryName.Trim().ToLower() == category.Trim().ToLower()).ToList();

            if (!String.IsNullOrEmpty(classname))
                query = query.Where(s => s.ClassDivision?.Class?.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

            return query.OrderBy(s => s.CategoryName).ToList();

        }
        public FeeCategory GetFeeCategoryByClassAndCategory(int classid, int? categoryid = null)
        {
            return _feeCategoryRepository.Table.FirstOrDefault(x => x.ClassDivisionId == classid && (!categoryid.HasValue || x.CategoryId == categoryid.Value));
        }
        public void ToggleActiveStatusFeeCategory(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objFeeCategory = _feeCategoryRepository.GetByID(id);
            if (objFeeCategory != null)
            {
                objFeeCategory.IsActive = !objFeeCategory.IsActive;
                objFeeCategory.ModifiedOn = DateTime.Now;
                _feeCategoryRepository.Update(objFeeCategory);
            }

        }
        public bool CheckFeeCategoryExists(int catid, int classdivid, int acadmicyearid, int? id = null)
        {
            if (catid == 0 || classdivid == 0 || acadmicyearid == 0)
                throw new ArgumentNullException("catid");

            return _feeCategoryRepository.Table.Any(a => ((a.CategoryId == catid) && (a.ClassDivisionId == classdivid) && a.AcadmicYearId == acadmicyearid) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        #endregion

        #region Caste
        public void InsertCaste(Caste caste)
        {
            _casteRepository.Insert(caste);
        }
        public void UpdateCaste(Caste caste)
        {
            _casteRepository.Update(caste);
        }
        public void DeleteCaste(int id)
        {
            var caste = _casteRepository.GetByID(id);
            if (caste != null)
            {
                caste.IsActive = false;
                caste.IsDeleted = true;
                _casteRepository.Update(caste);
            }
        }
        public IList<Caste> GetAllCastes(bool? onlyActive = null)
        {
            return _casteRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public void RemoveCasteFromCategory(int categoryid, int casteid)
        {
            var categoryCaste = _categoryRepository.Table.FirstOrDefault(s => s.Id == categoryid);
            var selectedCaste = _casteRepository.Table.FirstOrDefault(c => c.Id == casteid);

            if (categoryCaste != null && selectedCaste != null)
            {
                categoryCaste.Castes.Remove(selectedCaste);
            }
        }
        public IList<Caste> GetAllCastesByCategory(int categoryid)
        {
            return _casteRepository.Table.Where(x => x.IsDeleted == false && x.Categories.Any(y => y.Id == categoryid)).ToList();
        }
        public Caste GetCasteById(int id)
        {
            if (id == 0)
                throw new System.Exception("Caste Id Is Missing.");

            return _casteRepository.GetByID(id);
        }
        public IList<Caste> GetCasteByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Caste Name is Missing.");

            var query = _casteRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public IList<Caste> SearchFeeCastes(bool? active, string religion = null, int? acedemicyearid = null)
        {
            var query = _casteRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid).ToList();

            if (!String.IsNullOrEmpty(religion))
                query = query.Where(s => s.Religion.Name.Trim().ToLower() == religion.Trim().ToLower()).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(s => s.Name).ToList();

        }
        public bool CheckCasteExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _casteRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusCaste(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objCaste = _casteRepository.GetByID(id);
            if (objCaste != null)
            {
                objCaste.IsActive = !objCaste.IsActive;
                objCaste.ModifiedOn = DateTime.Now;
                _casteRepository.Update(objCaste);
            }

        }
        #endregion

        #region Category
        public void InsertCategory(Category category)
        {
            _categoryRepository.Insert(category);
        }
        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
        }
        public void DeleteCategory(int id)
        {
            var category = _categoryRepository.GetByID(id);
            if (category != null)
            {
                category.IsActive = false;
                category.IsDeleted = true;
                _categoryRepository.Update(category);
            }
        }
        public IList<Category> GetAllCategories(bool? onlyActive = null)
        {
            return _categoryRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Category GetCategoryById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _categoryRepository.GetByID(id);
        }
        public IList<Category> GetCategoryByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Category Name is Missing.");

            var query = _categoryRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public void ToggleActiveStatusCategory(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objCategory = _categoryRepository.GetByID(id);
            if (objCategory != null)
            {
                objCategory.IsActive = !objCategory.IsActive;
                objCategory.ModifiedOn = DateTime.Now;
                _categoryRepository.Update(objCategory);
            }

        }
        public bool CheckCategoryExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _casteRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()) && x.IsDeleted == false);
        }
        #endregion

        #region Product Category
        public void InsertProductCategory(ProductCategory productCategory)
        {
            _productCategoryRepository.Insert(productCategory);
        }
        public void UpdateProductCategory(ProductCategory productCategory)
        {
            _productCategoryRepository.Update(productCategory);
        }
        public void DeleteProductCategory(int id)
        {
            var productCategory = _productCategoryRepository.GetByID(id);
            if (productCategory != null)
            {
                productCategory.IsActive = false;
                productCategory.IsDeleted = true;
                _productCategoryRepository.Update(productCategory);
            }
        }
        public IList<ProductCategory> GetAllProductCategories(bool? onlyActive = null)
        {
            return _productCategoryRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }

        public IList<Product> GetAllProductsByProductCategory(int productCategoryId, bool? onlyActive = null)
        {
            return _productRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.ProductCategories.Any(y => y.ProductCategoryId == productCategoryId) && x.IsDeleted == false).ToList();
        }
        public ProductCategory GetProductCategoryById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _productCategoryRepository.GetByID(id);
        }
        public IList<ProductCategory> GetProductCategoryByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("ProductCategory Name is Missing.");

            var query = _productCategoryRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public void ToggleActiveStatusProductCategory(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objProductCategory = _productCategoryRepository.GetByID(id);
            if (objProductCategory != null)
            {
                objProductCategory.IsActive = !objProductCategory.IsActive;
                objProductCategory.ModifiedOn = DateTime.Now;
                _productCategoryRepository.Update(objProductCategory);
            }

        }
        public void ToggleMenuStatusProductCategory(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objProductCategory = _productCategoryRepository.GetByID(id);
            if (objProductCategory != null)
            {
                objProductCategory.IncludeInTopMenu = !objProductCategory.IncludeInTopMenu;
                objProductCategory.ModifiedOn = DateTime.Now;
                _productCategoryRepository.Update(objProductCategory);
            }

        }
        public bool CheckProductCategoryExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _casteRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()) && x.IsDeleted == false);
        }
        public IList<ProductCategoryMapping> GetCategoryProductMappings(int? categoryid = null, int? productid = null)
        {
            return _productCategoryMappingRepository.Table.Where(x => (!categoryid.HasValue || x.ProductCategoryId == categoryid.Value) && (!productid.HasValue || x.ProductId == productid.Value)).ToList();
        }

        public void InsertProductCategoryMapping(ProductCategoryMapping productCategoryMapping)
        {
            _productCategoryMappingRepository.Insert(productCategoryMapping);
        }
        public void UpdateProductCategoryMapping(ProductCategoryMapping productCategoryMapping)
        {
            _productCategoryMappingRepository.Update(productCategoryMapping);
        }
        public void DeleteProductCategoryMapping(int id)
        {
            var productCategoryMapping = _productCategoryMappingRepository.GetByID(id);
            if (productCategoryMapping != null)
            {
                _productCategoryMappingRepository.Delete(productCategoryMapping);
            }
        }

        public void RemoveProductFromCategory(int productcategoryid, int productid)
        {
            var productCategoryMapping = _productCategoryMappingRepository.Table.FirstOrDefault(s => s.ProductCategoryId == productcategoryid && s.ProductId == productid);
            if (productCategoryMapping != null)
            {
                _productCategoryMappingRepository.Delete(productCategoryMapping);
            }
        }
        #endregion

        #region Class
        public void InsertClass(Class objClass)
        {
            _classRepository.Insert(objClass);
        }
        public void UpdateClass(Class objClass)
        {
            _classRepository.Update(objClass);
        }
        public void DeleteClass(int id)
        {
            var objClass = _classRepository.GetByID(id);
            if (objClass != null)
            {
                objClass.IsActive = false;
                objClass.IsDeleted = true;
                _classRepository.Update(objClass);
            }
        }
        public Class GetClassById(int id)
        {
            if (id == 0)
                throw new System.Exception("Class Id Is Missing.");

            return _classRepository.GetByID(id);
        }
        public IList<Class> GetClassByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _classRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower()))).ToList();
        }
        public bool CheckClassExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _classRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public IList<Class> GetAllClasses(bool? onlyActive = null)
        {
            return _classRepository.Table.Where(a => (!onlyActive.HasValue || onlyActive.Value == a.IsActive) && a.IsDeleted == false).ToList();
        }
        public void ToggleActiveStatusClass(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objClass = _classRepository.GetByID(id);
            if (objClass != null)
            {
                objClass.IsActive = !objClass.IsActive;
                objClass.ModifiedOn = DateTime.Now;
                _classRepository.Update(objClass);
            }

        }
        public IList<ClassRoomDivision> GetAllDivisionsByClass(int? id, bool? onlyActive = null)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _classDivisionRepository.Table.Where(a => (!onlyActive.HasValue || (onlyActive.Value == a.Division.IsActive && onlyActive.Value == a.Class.IsActive && onlyActive.Value == a.ClassRoom.IsActive)) && (!id.HasValue || a.ClassId == id.Value)).ToList();
        }
        #endregion

        #region Class Division
        public void InsertClassDivision(ClassRoomDivision division)
        {
            _classDivisionRepository.Insert(division);
        }
        public void UpdateClassDivision(ClassRoomDivision division)
        {
            _classDivisionRepository.Update(division);
        }
        public void DeleteClassDivision(int id)
        {
            var division = _classDivisionRepository.GetByID(id);
            if (division != null)
            {
                _classDivisionRepository.Delete(division);
            }
        }
        public IList<ClassRoomDivision> GetDivisionsByClass(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _classDivisionRepository.Table.Where(x => x.ClassId == id).ToList();
        }
        public IList<ClassRoomDivision> GetClassDivisions(int? classid = null, int? divisionid = null, int? classroomid = null)
        {
            return _classDivisionRepository.Table.Where(x => (!classid.HasValue || x.ClassId == classid.Value) && (!divisionid.HasValue || x.DivisionId == divisionid.Value) && (!classroomid.HasValue || x.ClassRoomId == classroomid.Value)).ToList();
        }
        public void RemoveDivisionFromClass(int classid, int divisionid)
        {
            var classDivision = _classDivisionRepository.Table.FirstOrDefault(s => s.ClassId == classid && s.DivisionId == divisionid);
            if (classDivision != null)
            {
                _classDivisionRepository.Delete(classDivision);
            }
        }
        #endregion

        #region Class Room
        public void InsertClassRoom(ClassRoom classroom)
        {
            _classRoomRepository.Insert(classroom);
        }
        public void UpdateClassRoom(ClassRoom classroom)
        {
            _classRoomRepository.Update(classroom);
        }
        public void DeleteClassRoom(int id)
        {
            var classroom = _classRoomRepository.GetByID(id);
            if (classroom != null)
            {
                classroom.IsActive = false;
                classroom.IsDeleted = true;
                _classRoomRepository.Update(classroom);
            }
        }
        public bool CheckClassRoomExists(string roomnumber, int? id = null)
        {
            if (string.IsNullOrEmpty(roomnumber))
                throw new ArgumentNullException("roomnumber");

            return _classRoomRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Number.Trim().ToLower() == roomnumber.Trim().ToLower()));
        }
        public ClassRoom GetClassRoomById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _classRoomRepository.GetByID(id);
        }
        public ClassRoomDivision GetClassDivisionByClassRoom(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _classDivisionRepository.Table.FirstOrDefault(x => x.ClassRoomId == id);
        }
        public bool CheckClassRoomExistsForAnotherClassDivision(int? id, string roomnumber)
        {
            return _classDivisionRepository.Table.Any(x => (!id.HasValue || x.ClassRoomId == id.Value) && (string.IsNullOrEmpty(roomnumber) || (x.ClassRoom != null && x.ClassRoom.Number.Trim().ToLower() == roomnumber.Trim().ToLower())));
        }
        public void RemoveDivisionFromClassRoom(int classroomid, int divisionid)
        {
            var classDivision = _classDivisionRepository.Table.FirstOrDefault(s => s.ClassRoomId == classroomid && s.DivisionId == divisionid);
            if (classDivision != null)
            {
                _classDivisionRepository.Delete(classDivision);
            }
        }
        public void RemoveClassFromClassRoom(int classroomid, int classid)
        {
            var classDivision = _classDivisionRepository.Table.FirstOrDefault(s => s.ClassRoomId == classroomid && s.ClassId == classid);
            if (classDivision != null)
            {
                _classDivisionRepository.Delete(classDivision);
            }
        }
        public IList<ClassRoom> GetAllClassRooms(bool? onlyActive = null)
        {
            return _classRoomRepository.Table.Where(a => (!onlyActive.HasValue || onlyActive.Value == a.IsActive) && a.IsDeleted == false).ToList();
        }
        public void ToggleActiveStatusClassRoom(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objClassRoom = _classRoomRepository.GetByID(id);
            if (objClassRoom != null)
            {
                objClassRoom.IsActive = !objClassRoom.IsActive;
                objClassRoom.ModifiedOn = DateTime.Now;
                _classRoomRepository.Update(objClassRoom);
            }

        }
        #endregion

        #region Class Room Division

        public IList<ClassRoomDivision> GetAllClassRoomDivisions(bool? onlyActive = null)
        {
            return _classDivisionRepository.GetAll().ToList();
        }

        public ClassRoomDivision GetClassRoomDivisionById(int id)
        {
            return _classDivisionRepository.GetByID(id);
        }

        public bool CheckClassRoomAlreadyAssociatedToOtherDivisionAndClass(int classroomid, int? classid = null, int? divisionid = null)
        {
            return _classDivisionRepository.Table.Any(cl => (!classid.HasValue || cl.ClassId == classid.Value) && (!divisionid.HasValue || cl.DivisionId == divisionid.Value) && cl.ClassRoomId == classroomid);
        }

        public IList<ClassRoomDivision> GetAllClassDivisionsByTeacher(int id)
        {
            return _teacherRepository.GetByID(id).ClassRoomDivisions.ToList();
        }

        #endregion

        #region Homework
        public void InsertHomework(Homework objHomework)
        {
            _homeworkRepository.Insert(objHomework);
        }
        public void UpdateHomework(Homework objHomework)
        {
            _homeworkRepository.Update(objHomework);
        }
        public void DeleteHomework(int id)
        {
            var objHomework = _homeworkRepository.GetByID(id);
            if (objHomework != null)
            {
                objHomework.IsActive = false;
                objHomework.IsDeleted = true;
                _homeworkRepository.Update(objHomework);
            }
        }
        public Homework GetHomeworkById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _homeworkRepository.GetByID(id);
        }
        public IList<Homework> GetAllHomeworks(bool? onlyActive = null)
        {
            return _homeworkRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }

        public IList<DivisionHomework> GetAllHomeworksByDivision(int id)
        {
            return _divisionHomeworkRepository.Table.Where(x => x.DivisionId == id).OrderBy(x => x.Homework.Name).ToList();
        }
        public bool CheckHomeworkExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _homeworkRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusHomework(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objHomework = _homeworkRepository.GetByID(id);
            if (objHomework != null)
            {
                objHomework.IsActive = !objHomework.IsActive;
                objHomework.ModifiedOn = DateTime.Now;
                _homeworkRepository.Update(objHomework);
            }

        }

        #endregion

        #region Division
        public void InsertDivision(Division division)
        {
            _divisionRepository.Insert(division);
        }
        public void UpdateDivision(Division division)
        {
            _divisionRepository.Update(division);
        }
        public void DeleteDivision(int id)
        {
            var division = _divisionRepository.GetByID(id);
            if (division != null)
            {
                division.IsActive = false;
                division.IsDeleted = true;
                _divisionRepository.Update(division);
            }
        }
        public IList<Division> GetAllDivisions(bool? onlyActive = null)
        {
            return _divisionRepository.Table.Where(x => (!onlyActive.HasValue || onlyActive.Value == x.IsActive) && x.IsDeleted == false).ToList();
        }
        public Division GetDivisionById(int id)
        {
            if (id == 0)
                throw new System.Exception("Division Is Missing.");

            return _divisionRepository.GetByID(id);
        }
        public IList<Division> GetDivisionsByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Division Name is Missing.");

            var query = _divisionRepository.Table.Where(a => a.Name.ToLower().Contains(name.ToLower()) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public bool CheckDivisionExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _divisionRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusDivision(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objDivision = _divisionRepository.Table.FirstOrDefault(x => x.Id == id);
            if (objDivision != null)
            {
                objDivision.IsActive = !objDivision.IsActive;
                objDivision.ModifiedOn = DateTime.Now;
                _divisionRepository.Update(objDivision);
            }

        }
        #endregion

        #region Subject
        public void InsertSubject(Subject subject)
        {
            _subjectRepository.Insert(subject);
        }
        public void UpdateSubject(Subject subject)
        {
            _subjectRepository.Update(subject);
        }
        public void DeleteSubject(int id)
        {
            var subject = _subjectRepository.GetByID(id);
            if (subject != null)
            {
                subject.IsActive = false;
                subject.IsDeleted = true;
                _subjectRepository.Update(subject);
            }
        }
        public Subject GetSubjectById(int id)
        {
            if (id == 0)
                throw new System.Exception("Subject Id Is Missing.");

            return _subjectRepository.GetByID(id);
        }
        public IList<Subject> GetSubjectByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Subject Name is Missing.");

            var query = _subjectRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public bool CheckCodeExistsForSubject(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new Exception("Subject code is Missing.");

            return _subjectRepository.Table.Any(a => a.Code.Trim().ToLower() == code.Trim().ToLower() && a.IsDeleted == false);
        }
        public IList<Subject> GetAllSubjects(bool? onlyActive = null)
        {
            return _subjectRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public bool CheckSubjectExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _subjectRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()) && x.IsDeleted == false);
        }
        public void ToggleActiveStatusSubject(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objSubject = _subjectRepository.GetByID(id);
            if (objSubject != null)
            {
                objSubject.IsActive = !objSubject.IsActive;
                objSubject.ModifiedOn = DateTime.Now;
                _subjectRepository.Update(objSubject);
            }

        }
        public IList<Subject> GetAllSubjectsByTeacher(int id)
        {
            return _teacherRepository.GetByID(id).Subjects.ToList();
        }
        public IList<DivisionSubject> GetAllSubjectsByDivision(int id)
        {
            return _divisionClassSubjectRepository.Table.Where(x => x.DivisionId == id).OrderBy(x => x.Subject.Name).ToList();
        }
        #endregion

        #region Division Subject
        public void InsertDivisionSubject(DivisionSubject divisionClassSubject)
        {
            _divisionClassSubjectRepository.Insert(divisionClassSubject);
        }
        public void UpdateDivisionSubject(DivisionSubject divisionClassSubject)
        {
            _divisionClassSubjectRepository.Update(divisionClassSubject);
        }
        public void DeleteDivisionSubject(int id)
        {
            var divisionClassSubject = _divisionClassSubjectRepository.GetByID(id);
            if (divisionClassSubject != null)
                _divisionClassSubjectRepository.Update(divisionClassSubject);
        }
        public IList<DivisionSubject> GetAllDivisionSubjectMappings()
        {
            return _divisionClassSubjectRepository.Table.ToList();
        }
        public DivisionSubject GetDivisionSubjectMappingById(int id)
        {
            if (id == 0)
                throw new System.Exception("Division Class Subject Mapping Id Is Missing.");

            return _divisionClassSubjectRepository.GetByID(id);
        }
        public IList<DivisionSubject> GetDivisionSubjects(int? divisionid = null, int? subjectid = null)
        {
            return _divisionClassSubjectRepository.Table.Where(x => (!divisionid.HasValue || x.DivisionId == divisionid.Value) && (!subjectid.HasValue || x.SubjectId == subjectid.Value)).ToList();
        }
        public void RemoveSubjectFromDivision(int divisionid, int subjectid)
        {
            var divisionSubject = _divisionClassSubjectRepository.Table.FirstOrDefault(s => s.DivisionId == divisionid && s.SubjectId == subjectid);
            if (divisionSubject != null)
            {
                _divisionClassSubjectRepository.Delete(divisionSubject);
            }
        }

        public IList<DivisionHomework> GetDivisionHomeworks(int? divisionid = null, int? homeworkid = null)
        {
            return _divisionHomeworkRepository.Table.Where(x => (!divisionid.HasValue || x.DivisionId == divisionid.Value) && (!homeworkid.HasValue || x.HomeworkId == homeworkid.Value)).ToList();
        }
        public void RemoveHomeworkFromDivision(int divisionid, int homeworkid)
        {
            var divisionHomework = _divisionHomeworkRepository.Table.FirstOrDefault(s => s.DivisionId == divisionid && s.HomeworkId == homeworkid);
            if (divisionHomework != null)
            {
                _divisionHomeworkRepository.Delete(divisionHomework);
            }
        }

        public IList<DivisionExam> GetDivisionExams(int? divisionid = null, int? examid = null)
        {
            return _divisionExamRepository.Table.Where(x => (!divisionid.HasValue || x.DivisionId == divisionid.Value) && (!examid.HasValue || x.ExamId == examid.Value)).ToList();
        }
        public void RemoveExamFromDivision(int divisionid, int examid)
        {
            var divisionExam = _divisionExamRepository.Table.FirstOrDefault(s => s.DivisionId == divisionid && s.ExamId == examid);
            if (divisionExam != null)
            {
                _divisionExamRepository.Delete(divisionExam);
            }
        }
        #endregion

        #region Division Homework
        public void InsertDivisionHomework(DivisionHomework divisionHomework)
        {
            _divisionHomeworkRepository.Insert(divisionHomework);
        }
        public void UpdateDivisionHomework(DivisionHomework divisionHomework)
        {
            _divisionHomeworkRepository.Update(divisionHomework);
        }
        public void DeleteDivisionHomework(int id)
        {
            var divisionHomework = _divisionHomeworkRepository.GetByID(id);
            if (divisionHomework != null)
                _divisionHomeworkRepository.Update(divisionHomework);
        }
        public IList<DivisionHomework> GetAllDivisionHomeworkMappings()
        {
            return _divisionHomeworkRepository.Table.ToList();
        }
        public DivisionHomework GetDivisionHomeworkMappingById(int id)
        {
            if (id == 0)
                throw new System.Exception("Division  Homework Mapping Id Is Missing.");

            return _divisionHomeworkRepository.GetByID(id);
        }
        #endregion

        #region Division Exam
        public void InsertDivisionExam(DivisionExam divisionExam)
        {
            _divisionExamRepository.Insert(divisionExam);
        }
        public void UpdateDivisionExam(DivisionExam divisionExam)
        {
            _divisionExamRepository.Update(divisionExam);
        }
        public void DeleteDivisionExam(int id)
        {
            var divisionExam = _divisionExamRepository.GetByID(id);
            if (divisionExam != null)
                _divisionExamRepository.Update(divisionExam);
        }
        public IList<DivisionExam> GetAllDivisionExamMappings()
        {
            return _divisionExamRepository.Table.ToList();
        }
        public DivisionExam GetDivisionExamMappingById(int id)
        {
            if (id == 0)
                throw new System.Exception("Division  Exam Mapping Id Is Missing.");

            return _divisionExamRepository.Table.FirstOrDefault(x => x.Id == id);
        }
        public IList<DivisionExam> GetAllExamsByDivision(int id)
        {
            return _divisionExamRepository.Table.Where(x => x.DivisionId == id).OrderBy(x => x.Exam.ExamName).ToList();
        }
        #endregion

        #region Designation
        public void InsertDesignation(Designation designation)
        {
            _designationRepository.Insert(designation);
        }
        public void UpdateDesignation(Designation designation)
        {
            _designationRepository.Update(designation);
        }
        public void DeleteDesignation(int id)
        {
            var designation = _designationRepository.GetByID(id);
            if (designation != null)
            {
                designation.IsActive = false;
                designation.IsDeleted = true;
                _designationRepository.Update(designation);
            }
        }
        public Designation GetDesignationById(int id)
        {
            if (id == 0)
                throw new System.Exception("Designation Id Is Missing.");

            return _designationRepository.GetByID(id);
        }
        public IList<Designation> GetAllDesignations(bool? onlyActive = null)
        {
            return _designationRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public IList<Designation> GetDesignationByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Designation Name is Missing.");

            var query = _designationRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(x => x.Name).ToList();
        }
        public bool CheckDesignationExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _designationRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()) && x.IsDeleted == false);
        }
        public void ToggleActiveStatusDesignation(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objDesignation = _designationRepository.GetByID(id);
            if (objDesignation != null)
            {
                objDesignation.IsActive = !objDesignation.IsActive;
                objDesignation.ModifiedOn = DateTime.Now;
                _designationRepository.Update(objDesignation);
            }

        }
        #endregion

        #region Teacher
        public void InsertTeacher(Teacher teacher)
        {
            _teacherRepository.Insert(teacher);
        }
        public void UpdateTeacher(Teacher teacher)
        {
            _teacherRepository.Update(teacher);
        }
        public void DeleteTeacher(int id)
        {
            var teacher = _teacherRepository.GetByID(id);
            if (teacher != null)
            {
                teacher.IsActive = false;
                teacher.IsDeleted = true;
                _teacherRepository.Update(teacher);
            }
        }
        public Teacher GetTeacherByImpersonateId(int id)
        {
            if (id == 0)
                throw new System.Exception("Student impersonate id is missing.");

            return _teacherRepository.Table.FirstOrDefault(x => x.ImpersonateId == id);
        }
        public IList<Teacher> GetAllTeachers(bool? onlyActive = null)
        {
            return _teacherRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Teacher GetTeacherById(int id)
        {
            if (id == 0)
                throw new System.Exception("Teacher Id Is Missing.");

            return _teacherRepository.GetByID(id);
        }
        public IList<Teacher> GetTeachersByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Teacher Name is Missing.");

            var query = _teacherRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public IList<Teacher> SearchTeachers(bool? active, string subject = null, string qualification = null, string employee = null, int? acedemicyearid = null)
        {
            var query = _teacherRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            if (!String.IsNullOrEmpty(qualification))
                query = query.Where(s => s.Qualification.Name.Trim().ToLower() == qualification.Trim().ToLower()).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(s => s.Name).ToList();

        }
        public IList<Teacher> SearchTeachers(bool? active, int subjectid = 0, int qualificationid = 0, int employeeid = 0, int? acedemicyearid = null)
        {
            var query = _teacherRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            if (qualificationid > 0)
                query = query.Where(s => s.QualificationId == qualificationid).ToList();

            if (employeeid > 0)
                query = query.Where(s => s.EmployeeId == employeeid).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(s => s.Name).ToList();

        }
        public virtual IList<Teacher> GetTeachersByIds(int[] teacherids)
        {
            if (teacherids == null || teacherids.Length == 0)
                return new List<Teacher>();

            var query = from r in _teacherRepository.Table
                        where teacherids.Contains(r.Id)
                        select r;

            var users = query.ToList();

            var sortedTeachers = new List<Teacher>();
            foreach (int id in teacherids)
            {
                var teacher = users.Find(x => x.Id == id);
                if (teacher != null)
                    sortedTeachers.Add(teacher);
            }
            return sortedTeachers;
        }
        public virtual void DeleteTeachers(IList<Teacher> teachers)
        {
            if (teachers == null)
                throw new ArgumentNullException("teachers");

            foreach (var _teacher in teachers)
            {
                _teacher.IsActive = false;
                _teacher.IsDeleted = true;
                _teacherRepository.Update(_teacher);
            }
        }
        public void ToggleTeacher(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Teacher");

            var _teacher = _teacherRepository.GetByID(id);
            if (_teacher != null)
            {
                _teacher.IsActive = !_teacher.IsActive;
                _teacherRepository.Update(_teacher);
            }

        }
        public int GetTeacherCountByLoginDate(DateTime logindate)
        {
            if (logindate == null)
                throw new ArgumentNullException("logindate");

            var query = _teacherRepository.Table.ToList();
            var lstTeachers = new List<Teacher>();
            foreach (var q in query)
            {
                if (q.ModifiedOn.Date == logindate.Date)
                    lstTeachers.Add(q);
            }
            return lstTeachers.ToList().Count;
        }
        public int GetTotalTeachers()
        {
            return _teacherRepository.Table.Count(x => x.IsActive && x.IsDeleted == false);
        }
        public bool CheckTeacherExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _teacherRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusTeacher(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objTeacher = _teacherRepository.GetByID(id);
            if (objTeacher != null)
            {
                objTeacher.IsActive = !objTeacher.IsActive;
                objTeacher.ModifiedOn = DateTime.Now;
                _teacherRepository.Update(objTeacher);
            }

        }
        public IList<TeacherExam> GetAllTeacherExamMappings()
        {
            return _teacherExamRepository.Table.ToList();
        }
        public IList<TeacherExam> GetAllExamsByTeacher(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _teacherExamRepository.Table.Where(x => x.TeacherId == id).ToList();
        }
        public Teacher GetTeacherByImpersonatedUser(int userid)
        {
            return _teacherRepository.Table.FirstOrDefault(x => x.ImpersonateId == userid);
        }
        #endregion

        #region Teacher Exam

        public void InsertTeacherExam(TeacherExam teacherExam)
        {
            _teacherExamRepository.Insert(teacherExam);
        }
        public void UpdateTeacherExam(TeacherExam teacherExam)
        {
            _teacherExamRepository.Update(teacherExam);
        }
        public void DeleteTeacherExam(int id)
        {
            var teacherExam = _teacherExamRepository.GetByID(id);
            if (teacherExam != null)
                _teacherExamRepository.Update(teacherExam);
        }

        public TeacherExam GetTeacherExamMappingById(int id)
        {
            if (id == 0)
                throw new System.Exception("Teacher  Exam Mapping Id Is Missing.");

            return _teacherExamRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        #endregion

        #region Time Table Setting
        public void InsertTimeTableSetting(TimeTableSetting dailyTimeTableSetting)
        {
            _dailyTimeTableSettingRepository.Insert(dailyTimeTableSetting);
        }
        public void UpdateTimeTableSetting(TimeTableSetting dailyTimeTableSetting)
        {
            _dailyTimeTableSettingRepository.Update(dailyTimeTableSetting);
        }
        public void DeleteTimeTableSetting(int id)
        {
            var dailyTimeTableSetting = _dailyTimeTableSettingRepository.GetByID(id);
            if (dailyTimeTableSetting != null)
                _dailyTimeTableSettingRepository.Update(dailyTimeTableSetting);
        }
        public TimeTableSetting GetTimeTableSettingById(int id)
        {
            if (id == 0)
                throw new System.Exception(" Time Table Setting Id Is Missing.");

            return _dailyTimeTableSettingRepository.GetByID(id);
        }
        public IList<TimeTableSetting> GetAllTimeTableSettings()
        {
            return _dailyTimeTableSettingRepository.Table.OrderBy(x => x.SchoolStartTime).ToList();
        }
        public IList<TimeTableSetting> SearchTimeTableSettings(bool? active, int? acedemicyearid = null)
        {
            var query = _dailyTimeTableSettingRepository.Table.ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            return query.OrderBy(s => s.SchoolStartTime).ToList();
        }
        #endregion

        #region Time Table
        public void InsertTimeTable(TimeTable dailyTimeTable)
        {
            _dailyTimeTableRepository.Insert(dailyTimeTable);
        }
        public void UpdateTimeTable(TimeTable dailyTimeTable)
        {
            _dailyTimeTableRepository.Update(dailyTimeTable);
        }
        public void DeleteTimeTable(int id)
        {
            var dailyTimeTable = _dailyTimeTableRepository.GetByID(id);
            if (dailyTimeTable != null)
            {
                dailyTimeTable.IsActive = false;
                dailyTimeTable.IsDeleted = true;
                _dailyTimeTableRepository.Update(dailyTimeTable);
            }
        }
        public IList<TimeTable> GetAllTimeTables(bool? onlyActive = null)
        {
            return _dailyTimeTableRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public TimeTable GetTimeTableById(int id)
        {
            if (id == 0)
                throw new System.Exception("Time Table Id Is Missing.");

            return _dailyTimeTableRepository.GetByID(id);
        }
        public IList<TimeTable> SearchTimeTables(bool? active, int? acedemicyearid = null)
        {
            var query = _dailyTimeTableRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            return query.Where(x => x.IsDeleted == false).OrderBy(s => s.LectureNumber).ToList();

        }
        public void ToggleActiveStatusTimetable(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objTimetable = _dailyTimeTableRepository.GetByID(id);
            if (objTimetable != null)
            {
                objTimetable.IsActive = !objTimetable.IsActive;
                objTimetable.ModifiedOn = DateTime.Now;
                _dailyTimeTableRepository.Update(objTimetable);
            }

        }

        #endregion

        #region Qualification
        public void InsertQualification(Qualification qualification)
        {
            _qualificationRepository.Insert(qualification);
        }
        public void UpdateQualification(Qualification qualification)
        {
            _qualificationRepository.Update(qualification);
        }
        public void DeleteQualification(int id)
        {
            var qualification = _qualificationRepository.GetByID(id);

            if (qualification != null)
                _qualificationRepository.Delete(qualification);
        }
        public IList<Qualification> GetAllQualifications()
        {
            return _qualificationRepository.Table.OrderBy(x => x.Name).ToList();
        }
        public Qualification GetQualificationById(int id)
        {
            if (id == 0)
                throw new System.Exception("Qualification Id Is Missing.");

            return _qualificationRepository.GetByID(id);
        }
        public IList<Qualification> GetQualificationByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Qualification Name is Missing.");

            return _qualificationRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower()))).OrderBy(x => x.Name).ToList();
        }
        public bool CheckQualificationExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _qualificationRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        #endregion

        #region Allowance
        public void InsertAllowance(Allowance allowance)
        {
            _allowanceRepository.Insert(allowance);
        }
        public void UpdateAllowance(Allowance allowance)
        {
            _allowanceRepository.Update(allowance);
        }
        public void DeleteAllowance(int id)
        {
            var allowance = _allowanceRepository.GetByID(id);
            if (allowance != null)
                _allowanceRepository.Delete(allowance);
        }
        public Allowance GetAllowanceById(int id)
        {
            if (id == 0)
                throw new System.Exception("Time Table Setting Id Is Missing.");

            return _allowanceRepository.GetByID(id);
        }
        public IList<Allowance> GetAllAllowances()
        {
            return _allowanceRepository.Table.Where(a => a.IsDeleted == false).ToList();
        }
        public Allowance GetAllowanceByDesignation(int designationid)
        {
            return _allowanceRepository.Table.FirstOrDefault(x => x.IsDeleted == false && x.DesignationId == designationid);
        }
        public bool CheckAllowanceExistsForDesignation(int designationid, int? id = null)
        {
            if (designationid == 0)
                throw new ArgumentNullException("designationid");

            return _allowanceRepository.Table.Any(a => (a.DesignationId == designationid) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        #endregion

        #region Payment
        public void InsertPayment(Payment payment)
        {
            _paymentRepository.Insert(payment);
        }
        public void UpdatePayment(Payment payment)
        {
            _paymentRepository.Update(payment);
        }
        public void DeletePayment(int id)
        {
            var payment = _paymentRepository.GetByID(id);
            if (payment != null)
                _paymentRepository.Delete(payment);
        }
        public Payment GetPaymentById(int id)
        {
            if (id == 0)
                throw new System.Exception("Payment Id Is Missing.");

            return _paymentRepository.GetByID(id);
        }
        public IList<Payment> GetAllPayments()
        {
            return _paymentRepository.GetAll().ToList();
        }
        public IList<Payment> SearchPayments(int? employeeid = null, int? acedemicyearid = null)
        {
            var query = _paymentRepository.Table.ToList();

            if (employeeid.HasValue)
                query = query.Where(s => s.EmployeeId == employeeid).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid).ToList();

            return query.ToList();
        }
        #endregion

        #region Vendor
        public void InsertVendor(Vendor vendor)
        {
            _vendorRepository.Insert(vendor);
        }
        public void UpdateVendor(Vendor vendor)
        {
            _vendorRepository.Update(vendor);
        }
        public void DeleteVendor(int id)
        {
            var vendor = _vendorRepository.GetByID(id);
            if (vendor != null)
            {
                vendor.IsActive = false;
                vendor.IsDeleted = true;
                _vendorRepository.Update(vendor);
            }
        }
        public IList<Vendor> GetAllVendors(bool? active = null)
        {
            return _vendorRepository.Table.Where(x => (!active.HasValue || x.IsActive == active.Value) && x.IsDeleted == false).OrderBy(x => x.Name).ToList();
        }
        public Vendor GetVendorById(int id)
        {
            if (id == 0)
                throw new System.Exception("Vendor Id Is Missing.");

            return _vendorRepository.GetByID(id);
        }
        public Vendor GetVendorsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Vendor Name is Missing.");

            return _vendorRepository.Table.FirstOrDefault(a => (string.Equals(a.Name.Trim().ToLower(), name.Trim().ToLower(), StringComparison.Ordinal)) && a.IsDeleted == false);
        }
        public IList<Vendor> SearchVendors(bool? active, int? acedemicyearid = null)
        {
            var query = _vendorRepository.Table.ToList();

            if (active.HasValue)
                query = query.Where(s => s.IsActive == active).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            return query.Where(s => s.IsDeleted == false).OrderBy(s => s.Name).ToList();

        }
        public void ToggleActiveStatusVendor(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objVendor = _vendorRepository.GetByID(id);
            if (objVendor != null)
            {
                objVendor.IsActive = !objVendor.IsActive;
                objVendor.ModifiedOn = DateTime.Now;
                _vendorRepository.Update(objVendor);
            }

        }
        #endregion

        #region Purchase
        public void InsertPurchase(Purchase purchase)
        {
            _purchaseRepository.Insert(purchase);
        }
        public void UpdatePurchase(Purchase purchase)
        {
            _purchaseRepository.Update(purchase);
        }
        public void DeletePurchase(int id)
        {
            var purchase = _purchaseRepository.GetByID(id);

            if (purchase != null)
                _purchaseRepository.Delete(purchase);
        }
        public IList<Purchase> GetAllPurchases()
        {
            return _purchaseRepository.GetAll().ToList();
        }
        public Purchase GetPurchaseById(int id)
        {
            if (id == 0)
                throw new System.Exception("Purchase Id Is Missing.");

            return _purchaseRepository.GetByID(id);
        }
        public IList<Purchase> SearchPurchases(int? productid = null, int? vendorid = null, int? acedemicyearid = null)
        {
            var query = _purchaseRepository.Table.ToList();

            if (productid.HasValue)
                query = query.Where(s => s.ProductId == productid.Value).ToList();

            if (vendorid.HasValue)
                query = query.Where(s => s.VendorId == vendorid.Value).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

            return query.ToList();

        }
        #endregion

        #region Fee Detail
        public void InsertFeeDetail(FeeDetail feeDetail)
        {
            _feeDetailRepository.Insert(feeDetail);
        }
        public void UpdateFeeDetail(FeeDetail feeDetail)
        {
            _feeDetailRepository.Update(feeDetail);
        }
        public void DeleteFeeDetail(int id)
        {
            var feeDetail = _feeDetailRepository.GetByID(id);
            if (feeDetail != null)
                _feeDetailRepository.Update(feeDetail);
        }
        public IList<FeeDetail> GetAllFeeDetails()
        {
            return _feeDetailRepository.GetAll().ToList();
        }
        public FeeDetail GetFeeDetailById(int id)
        {
            if (id == 0)
                throw new System.Exception("FeeDetail Id Is Missing.");

            return _feeDetailRepository.GetByID(id);
        }
        public IList<FeeDetail> SearchFeeDetails(int? studentid = null, int? categoryid = null, int? cashierid = null, int? acedemicyearid = null)
        {
            var query = _feeDetailRepository.GetAll().ToList();

            if (studentid.HasValue)
                query = query.Where(x => x.StudentId == studentid.Value).ToList();

            if (categoryid.HasValue)
                query = query.Where(x => x.FeeCategoryStructureId == categoryid.Value).ToList();

            if (cashierid.HasValue)
                query = query.Where(x => x.CashierId == cashierid.Value).ToList();

            if (acedemicyearid.HasValue)
                query = query.Where(x => x.AcadmicYearId == acedemicyearid.Value).ToList();

            return query.ToList();

        }
        #endregion

        #region Message Group
        public void InsertMessageGroup(MessageGroup messageGroup)
        {
            _messageGroupRepository.Insert(messageGroup);
        }
        public void UpdateMessageGroup(MessageGroup messageGroup)
        {
            _messageGroupRepository.Update(messageGroup);
        }
        public void DeleteMessageGroup(int id)
        {
            var messageGroup = _messageGroupRepository.GetByID(id);
            if (messageGroup != null)
                _messageGroupRepository.Update(messageGroup);
        }
        public IList<MessageGroup> GetAllMessageGroups()
        {
            return _messageGroupRepository.GetAll().ToList();
        }
        public MessageGroup GetMessageGroupById(int id)
        {
            if (id == 0)
                throw new System.Exception("MessageGroup Id Is Missing.");

            return _messageGroupRepository.GetByID(id);
        }
        public MessageGroup GetMessageGroupByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Message Group Name is Missing.");

            return _messageGroupRepository.Table.FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
        }
        #endregion

        #region Student Message Group
        public void InsertStudent_MessageGroup(Student_MessageGroup studentMessageGroup)
        {
            _studentMessageGroupRepository.Insert(studentMessageGroup);
        }
        public void UpdateStudent_MessageGroup(Student_MessageGroup studentMessageGroup)
        {
            _studentMessageGroupRepository.Update(studentMessageGroup);
        }
        public void DeleteStudent_MessageGroup(int id)
        {
            var studentMessageGroup = _studentMessageGroupRepository.GetByID(id);
            if (studentMessageGroup != null)
                _studentMessageGroupRepository.Update(studentMessageGroup);
        }
        public IList<Student_MessageGroup> GetAllStudent_MessageGroups()
        {
            return _studentMessageGroupRepository.GetAll().ToList();
        }
        public Student_MessageGroup GetStudent_MessageGroupById(int id)
        {
            if (id == 0)
                throw new System.Exception("Student_Message Group Id Is Missing.");

            return _studentMessageGroupRepository.GetByID(id);
        }
        public IList<Student_MessageGroup> GetStudent_MessageGroupsByStudent(int studentid)
        {
            if (studentid == 0)
                throw new Exception("Message Group Student Id is Missing.");

            return _studentMessageGroupRepository.Table.Where(a => (a.StudentId == studentid)).ToList();
        }
        #endregion

        #region Message
        public void InsertMessage(Message message)
        {
            _messageRepository.Insert(message);
        }
        public void UpdateMessage(Message message)
        {
            _messageRepository.Update(message);
        }
        public void DeleteMessage(int id)
        {
            var message = _messageRepository.GetByID(id);
            if (message != null)
            {
                message.IsActive = false;
                message.IsDeleted = true;
                _messageRepository.Update(message);
            }
        }
        public IList<Message> GetAllMessages(bool? onlyActive = null)
        {
            return _messageRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Message GetMessageById(int id)
        {
            if (id == 0)
                throw new System.Exception("Student_Message Group Id Is Missing.");

            return _messageRepository.GetByID(id);
        }
        public IList<Message> GetMessagesByMessageGroup(int messagegroupid, bool? active)
        {
            if (messagegroupid == 0)
                throw new Exception("Message Group Id is Missing.");

            var query = _messageRepository.Table.Where(a => (a.MessageGroupId == messagegroupid) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderByDescending(x => x.CreatedOn).ToList();
        }
        #endregion

        #region Exam
        public void InsertExam(Exam exam)
        {
            _examRepository.Insert(exam);
        }
        public void UpdateExam(Exam exam)
        {
            _examRepository.Update(exam);
        }
        public void DeleteExam(int id)
        {
            var exam = _examRepository.GetByID(id);
            exam.IsActive = false;
            exam.IsDeleted = true;
            _examRepository.Update(exam);
        }
        public IList<Exam> GetAllExams(bool? onlyActive = null)
        {
            return _examRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Exam GetExamById(int id)
        {
            if (id == 0)
                throw new System.Exception("Exam Id Is Missing.");

            return _examRepository.GetByID(id);
        }
        public IList<Exam> GetExamByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Exam Name is Missing.");

            var query = _examRepository.Table.Where(a => (a.ExamName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.ExamName).ToList();
        }
        public bool CheckExamExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _examRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.ExamName.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusExam(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objExam = _examRepository.GetByID(id);
            if (objExam != null)
            {
                objExam.IsActive = !objExam.IsActive;
                objExam.ModifiedOn = DateTime.Now;
                _examRepository.Update(objExam);
            }

        }
        public IList<DivisionExam> GetAllExamsByClassDivision(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _divisionExamRepository.Table.Where(x => x.DivisionId == id).ToList();
        }

        public IList<ClassRoom> GetVacantClassRoomsForExams()
        {
            var divisionExams = (from s in GetAllDivisionExamMappings()
                                 where (s.EndDate.Value >= DateTime.Now || s.Exam.IsActive)
                                 select s).ToList();

            var studentExams = (from s in GetAllStudentExamMappings()
                                where (s.EndDate.Value >= DateTime.Now || s.Exam.IsActive)
                                select s).ToList();

            var teacherExams = (from s in GetAllTeacherExamMappings()
                                where (s.EndDate >= DateTime.Now || s.Exam.IsActive)
                                select s).ToList();

            return (from c in GetAllClassRooms()
                    where !divisionExams.Any(d => d.ClassRoomId == c.Id) && !studentExams.Any(d => d.ClassRoomId == c.Id) && !teacherExams.Any(d => d.ClassRoomId == c.Id)
                    select c).ToList();
        }
        #endregion

        #region Book
        public void InsertBook(Book book)
        {
            _bookRepository.Insert(book);
        }
        public void UpdateBook(Book book)
        {
            _bookRepository.Update(book);
        }
        public void DeleteBook(int id)
        {
            var book = _bookRepository.GetByID(id);
            book.IsActive = false;
            book.IsDeleted = true;
            _bookRepository.Update(book);
        }
        public IList<Book> GetAllBooks(bool? onlyActive = null)
        {
            return _bookRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Book GetBookById(int id)
        {
            if (id == 0)
                throw new System.Exception("Book Id Is Missing.");

            return _bookRepository.GetByID(id);
        }
        public IList<Book> GetBookByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Book Name is Missing.");

            var query = _bookRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public bool CheckBookExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _bookRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusBook(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objBook = _bookRepository.GetByID(id);
            if (objBook != null)
            {
                objBook.IsActive = !objBook.IsActive;
                objBook.ModifiedOn = DateTime.Now;
                _bookRepository.Update(objBook);
            }

        }
        #endregion

        #region Book Issue
        public void InsertBookIssue(BookIssue bookIssue)
        {
            _bookIssueRepository.Insert(bookIssue);
        }
        public void UpdateBookIssue(BookIssue bookIssue)
        {
            _bookIssueRepository.Update(bookIssue);
        }
        public void DeleteBookIssue(int id)
        {
            var bookIssue = _bookIssueRepository.GetByID(id);
            if (bookIssue != null)
                _bookIssueRepository.Delete(bookIssue);
        }
        public IList<BookIssue> GetAllBookIssueIssues()
        {
            return _bookIssueRepository.Table.ToList();
        }
        public BookIssue GetBookIssueById(int id)
        {
            if (id == 0)
                throw new System.Exception("BookIssue Id Is Missing.");

            return _bookIssueRepository.GetByID(id);
        }
        public IList<BookIssue> GetBookIssueByStudent(int studentid)
        {
            if (studentid == 0)
                throw new Exception("BookIssue student is Missing.");

            return _bookIssueRepository.Table.Where(a => a.StudentId == studentid).ToList();
        }
        public bool CheckBookIssueExists(int studentid, int bookid, int? id = null)
        {
            if (studentid == 0 || bookid == 0)
                throw new ArgumentNullException("student and book");

            return _bookIssueRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.StudentId == studentid && x.BookId == bookid) && (x.Book.BookStatusId != 1));
        }
        #endregion

        #region AcadmicYear
        public void InsertAcadmicYear(AcadmicYear objAcadmicYear)
        {
            _acadmicYearRepository.Insert(objAcadmicYear);
        }
        public void UpdateAcadmicYear(AcadmicYear objAcadmicYear)
        {
            _acadmicYearRepository.Update(objAcadmicYear);
        }
        public void DeleteAcadmicYear(int id)
        {
            var objAcadmicYear = _acadmicYearRepository.GetByID(id);
            if (objAcadmicYear != null)
            {
                objAcadmicYear.IsActive = false;
                objAcadmicYear.IsDeleted = true;
                _acadmicYearRepository.Update(objAcadmicYear);
            }
        }
        public AcadmicYear GetAcadmicYearById(int id)
        {
            if (id == 0)
                throw new System.Exception("Acadmic Year Id Is Missing.");

            return _acadmicYearRepository.GetByID(id);
        }
        public AcadmicYear GetActiveAcadmicYear()
        {
            return _acadmicYearRepository.Table.FirstOrDefault(x => x.IsActive);
        }
        public AcadmicYear GetAcadmicYearByName(string name, bool? active = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Acadmic Year is Missing.");

            var query = _acadmicYearRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.FirstOrDefault();
        }
        public IList<AcadmicYear> GetAllAcadmicYears(bool? onlyActive = null)
        {
            return _acadmicYearRepository.Table.Where(ac => (!onlyActive.HasValue || onlyActive.Value == ac.IsActive) && ac.IsDeleted == false).ToList();
        }
        public bool CheckAcadmicYearExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("acadmicyear");

            return _acadmicYearRepository.Table.Any(a => (a.Name.Trim().ToLower() == name) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusAcadmicYear(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objAcadmicYear = _acadmicYearRepository.GetByID(id);
            if (objAcadmicYear != null)
            {
                objAcadmicYear.IsActive = !objAcadmicYear.IsActive;
                objAcadmicYear.ModifiedOn = DateTime.Now;
                _acadmicYearRepository.Update(objAcadmicYear);
            }

        }
        public void DeactivateAllAcadmicYears()
        {
            var objAcadmicYears = _acadmicYearRepository.GetAll().ToList();
            if (objAcadmicYears.Count > 0)
            {
                foreach (var year in objAcadmicYears)
                {
                    year.IsActive = false;
                    _acadmicYearRepository.Update(year);
                }
            }

        }
        #endregion

        #region Reaction
        public void InsertReaction(Reaction objReaction)
        {
            _reactionRepository.Insert(objReaction);
        }
        public void UpdateReaction(Reaction objReaction)
        {
            _reactionRepository.Update(objReaction);
        }
        public Reaction GetReactionById(int id)
        {
            if (id == 0)
                throw new Exception("Id is missing");

            return _reactionRepository.GetByID(id);
        }
        public IList<Reaction> SearchReactions(int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null)
        {
            var query = _reactionRepository.Table.OrderByDescending(x => x.CreatedOn).ToList();

            if (blogid.HasValue)
                query = query.Where(x => x.BlogId == blogid.Value).ToList();

            if (productid.HasValue)
                query = query.Where(x => x.ProductId == productid.Value).ToList();

            if (eventid.HasValue)
                query = query.Where(x => x.EventId == eventid.Value).ToList();

            if (pictureid.HasValue)
                query = query.Where(x => x.PictureId == pictureid.Value).ToList();

            if (videoid.HasValue)
                query = query.Where(x => x.VideoId == videoid.Value).ToList();

            if (newsid.HasValue)
                query = query.Where(x => x.NewsId == newsid.Value).ToList();

            if (commentid.HasValue)
                query = query.Where(x => x.CommentId == commentid.Value).ToList();

            if (replyid.HasValue)
                query = query.Where(x => x.ReplyId == replyid.Value).ToList();

            return query;
        }
        public IList<Reaction> GetReactionsByUser(int userid)
        {
            if (userid == 0)
                throw new Exception("User Id is missing");

            var query = _reactionRepository.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();
            return query;
        }
        public void SaveLikeReaction(int userid, bool IsLike, bool IsDislike, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null)
        {
            var reaction = SearchReactions(blogid, productid, eventid, pictureid, videoid, newsid, commentid, replyid);

            if (userid > 0)
            {
                var userReaction = reaction.FirstOrDefault(x => x.UserId == userid);
                if (userReaction != null)
                {
                    if (IsLike)
                    {
                        userReaction.IsLike = true;
                        userReaction.IsDislike = false;
                    }

                    if (IsDislike)
                    {
                        userReaction.IsDislike = true;
                        userReaction.IsLike = false;
                    }
                    userReaction.ModifiedOn = DateTime.Now;
                    _reactionRepository.Update(userReaction);

                }
                else
                {
                    var newReaction = new Reaction()
                    {
                        BlogId = blogid,
                        ProductId = productid,
                        EventId = eventid,
                        PictureId = pictureid,
                        VideoId = videoid,
                        NewsId = newsid,
                        CommentId = commentid,
                        ReplyId = replyid,
                        UserId = userid,
                        Username = _userRepository.GetByID(userid) != null ? _userRepository.GetByID(userid).UserName : "",
                        IsAngry = null,
                        IsHappy = null,
                        IsLOL = null,
                        IsSad = null,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Rating = null
                    };

                    if (IsLike)
                    {
                        newReaction.IsLike = true;
                        newReaction.IsDislike = false;
                    }

                    if (IsDislike)
                    {
                        newReaction.IsDislike = true;
                        newReaction.IsLike = false;
                    }

                    _reactionRepository.Insert(newReaction);
                }
            }
        }
        public void SaveMoodReaction(int userid, bool IsAngry, bool IsHappy, bool IsLOL, bool IsSad, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null)
        {
            var reaction = SearchReactions(blogid, productid, eventid, pictureid, videoid, newsid, commentid, replyid);

            if (userid > 0)
            {
                var userReaction = reaction.FirstOrDefault(x => x.UserId == userid);
                if (userReaction != null)
                {
                    if (IsAngry)
                    {
                        userReaction.IsAngry = true;
                        userReaction.IsLOL = null;
                        userReaction.IsSad = null;
                        userReaction.IsHappy = null;
                    }

                    if (IsLOL)
                    {
                        userReaction.IsLOL = true;
                        userReaction.IsAngry = null;
                        userReaction.IsSad = null;
                        userReaction.IsHappy = null;
                    }

                    if (IsSad)
                    {
                        userReaction.IsSad = true;
                        userReaction.IsAngry = null;
                        userReaction.IsLOL = null;
                        userReaction.IsHappy = null;
                    }

                    if (IsHappy)
                    {
                        userReaction.IsHappy = true;
                        userReaction.IsAngry = null;
                        userReaction.IsLOL = null;
                        userReaction.IsSad = null;
                    }
                    userReaction.ModifiedOn = DateTime.Now;
                    _reactionRepository.Update(userReaction);

                }
                else
                {
                    var newReaction = new Reaction()
                    {
                        BlogId = blogid,
                        ProductId = productid,
                        EventId = eventid,
                        PictureId = pictureid,
                        VideoId = videoid,
                        NewsId = newsid,
                        CommentId = commentid,
                        ReplyId = replyid,
                        UserId = userid,
                        Username = _userRepository.GetByID(userid) != null ? _userRepository.GetByID(userid).UserName : "",
                        IsAngry = null,
                        IsHappy = null,
                        IsLOL = null,
                        IsSad = null,
                        IsLike = null,
                        IsDislike = null,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Rating = null
                    };

                    if (IsAngry)
                    {
                        newReaction.IsAngry = true;
                        newReaction.IsLOL = null;
                        newReaction.IsSad = null;
                        newReaction.IsHappy = null;
                    }

                    if (IsLOL)
                    {
                        newReaction.IsLOL = true;
                        newReaction.IsAngry = null;
                        newReaction.IsSad = null;
                        newReaction.IsHappy = null;
                    }

                    if (IsSad)
                    {
                        newReaction.IsSad = true;
                        newReaction.IsAngry = null;
                        newReaction.IsLOL = null;
                        newReaction.IsHappy = null;
                    }

                    if (IsHappy)
                    {
                        newReaction.IsHappy = true;
                        newReaction.IsAngry = null;
                        newReaction.IsLOL = null;
                        newReaction.IsSad = null;
                    }

                    _reactionRepository.Insert(newReaction);
                }
            }
        }
        public void SaveRating(int userid, int rating, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null)
        {
            var reaction = SearchReactions(blogid, productid, eventid, pictureid, videoid, newsid, commentid, replyid);

            if (userid > 0)
            {
                var userReaction = reaction.FirstOrDefault(x => x.UserId == userid);
                if (userReaction != null)
                {
                    userReaction.Rating = rating;
                    userReaction.ModifiedOn = DateTime.Now;
                    _reactionRepository.Update(userReaction);
                }
                else
                {
                    var newReaction = new Reaction()
                    {
                        BlogId = blogid,
                        ProductId = productid,
                        EventId = eventid,
                        PictureId = pictureid,
                        VideoId = videoid,
                        NewsId = newsid,
                        CommentId = commentid,
                        ReplyId = replyid,
                        UserId = userid,
                        Username = _userRepository.GetByID(userid) != null ? _userRepository.GetByID(userid).UserName : "",
                        IsAngry = null,
                        IsHappy = null,
                        IsLOL = null,
                        IsSad = null,
                        IsLike = null,
                        IsDislike = null,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Rating = rating
                    };
                    _reactionRepository.Insert(newReaction);
                }
            }
        }

        #endregion

        #region Question Type
        public void InsertQuestionType(QuestionType questionType)
        {
            _questionTypeRepository.Insert(questionType);
        }
        public void UpdateQuestionType(QuestionType questionType)
        {
            _questionTypeRepository.Update(questionType);
        }
        public void DeleteQuestionType(int id)
        {
            var questionType = _questionTypeRepository.GetByID(id);
            if (questionType != null && !questionType.IsSystemDefined)
                _questionTypeRepository.Delete(questionType);
        }
        public QuestionType GetQuestionTypeById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException();

            return _questionTypeRepository.GetByID(id);
        }
        public IList<QuestionType> GetAllQuestionTypes(bool? onlyActive = null)
        {
            return _questionTypeRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value)).ToList();
        }
        public bool CheckQuestionTypeExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _questionTypeRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusQuestionType(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objQuestionType = _questionTypeRepository.GetByID(id);
            if (objQuestionType != null)
            {
                objQuestionType.IsActive = !objQuestionType.IsActive;
                objQuestionType.ModifiedOn = DateTime.Now;
                _questionTypeRepository.Update(objQuestionType);
            }

        }
        #endregion

        #region Assessments

        public void InsertQuestion(Question question)
        {
            _questionRepository.Insert(question);
        }
        public void UpdateQuestion(Question question)
        {
            _questionRepository.Update(question);
        }
        public void DeleteQuestion(int id)
        {
            var question = _questionRepository.GetByID(id);
            if (question != null)
            {
                question.IsActive = false;
                question.IsDeleted = true;
                _questionRepository.Update(question);
            }
        }

        public IList<Question> GetAllQuestions(bool? onlyActive = null)
        {
            return _questionRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public Question GetQuestionById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException();

            return _questionRepository.GetByID(id);
        }
        public IList<Question> SearchQuestions(int[] questionTypeIds = null, int[] subjectids = null, int? difficultylevel = null, bool? onlytimebound = null, bool? active = null)
        {
            var query = _questionRepository.Table.OrderBy(x => x.Name).ToList();

            if (subjectids != null && subjectids.Length > 0)
                query = query.Where(x => !x.SubjectId.HasValue || subjectids.Contains(x.SubjectId.Value)).ToList();

            if (questionTypeIds != null && questionTypeIds.Length > 0)
                query = query.Where(x => questionTypeIds.Contains(x.QuestionTypeId)).ToList();

            if (difficultylevel.HasValue)
                query = query.Where(x => x.DifficultyLevelId == difficultylevel.Value).ToList();

            if (onlytimebound.HasValue)
                query = query.Where(x => x.IsTimeBound == onlytimebound.Value).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active.Value).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public IList<Question> GetRandomQuestions(int count, int[] questionTypeIds = null, int[] subjectids = null, int? difficultylevel = null, bool? onlytimebound = null, bool? active = null)
        {
            var query = SearchQuestions(questionTypeIds, subjectids, difficultylevel, onlytimebound, active);
            query = query.Shuffle().Take(count).ToList();
            return query.OrderBy(x => x.Name).ToList();
        }
        public IList<AssessmentQuestion> GetQuestionsByAssessmentId(int assesmentid)
        {
            if (assesmentid == 0)
                throw new ArgumentException();

            var query = _assesQuestionRepository.Table.Where(x => x.AssessmentId == assesmentid).ToList();

            return query.OrderBy(x => x.DisplayOrder).ToList();
        }
        public IList<AssessmentStudent> GetStudentsByAssessmentId(int assesmentid)
        {
            if (assesmentid == 0)
                throw new ArgumentException();

            var query = _assessmentStudentRepository.Table.Where(x => x.AssessmentId == assesmentid).ToList();

            return query.ToList();
        }
        public void InsertOption(Option option)
        {
            _optionRepository.Insert(option);
        }
        public void UpdateOption(Option option)
        {
            _optionRepository.Update(option);
        }
        public void DeleteOption(int id)
        {
            var option = _optionRepository.GetByID(id);
            if (option != null)
            {
                _optionRepository.Delete(option);
            }
        }
        public IList<Option> GetOptionsByQuestionId(int questionid)
        {
            if (questionid == 0)
                throw new ArgumentException();

            var query = _optionRepository.Table.Where(x => x.QuestionId == questionid).ToList();
            return query.OrderBy(x => x.DisplayOrder).ToList();
        }
        public void InsertAssessment(Assessment assessment)
        {
            _assessmentRepository.Insert(assessment);
        }
        public void UpdateAssessment(Assessment assessment)
        {
            _assessmentRepository.Update(assessment);
        }
        public void DeleteAssessment(int id)
        {
            var assessment = _assessmentRepository.GetByID(id);
            if (assessment != null)
            {
                _assessmentRepository.Update(assessment);
            }
        }
        public IList<AssessmentStudent> GetAssessmentByStudentId(int studentid, bool? active = null, bool? completed = null, DateTime? starttime = null, DateTime? endtime = null)
        {
            if (studentid == 0)
                throw new ArgumentException();

            var query = _studentAssessmentRepository.Table.Where(x => x.StudentId == studentid).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active.Value).ToList();

            if (completed.HasValue)
                query = query.Where(x => x.IsCompleted == completed.Value).ToList();

            if (starttime.HasValue && endtime.HasValue)
                query = query.Where(x => x.StartOn.HasValue && x.EndOn.HasValue && (x.StartOn.Value >= starttime.Value && x.StartOn.Value <= endtime.Value || x.EndOn.Value >= starttime.Value && x.EndOn.Value <= endtime.Value)).ToList();

            return query.OrderByDescending(x => x.StartOn).ToList();
        }
        public IList<Assessment> SearchAssessments(int? difficultylevel = null, bool? onlytimebound = null, bool? active = null, DateTime? starttime = null, DateTime? endtime = null)
        {
            var query = _assessmentRepository.Table.OrderBy(x => x.StartTime).ToList();

            if (difficultylevel.HasValue)
                query = query.Where(x => x.DifficultyLevelId == difficultylevel.Value).ToList();

            if (onlytimebound.HasValue)
                query = query.Where(x => x.IsTimeBound == onlytimebound.Value).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active.Value).ToList();

            if (starttime.HasValue && endtime.HasValue)
                query = query.Where(x => x.StartTime.HasValue && x.EndTime.HasValue && (x.StartTime.Value >= starttime.Value && x.StartTime.Value <= endtime.Value || x.EndTime.Value >= starttime.Value && x.EndTime.Value <= endtime.Value)).ToList();

            return query.OrderBy(x => x.StartTime).ToList();
        }
        public Assessment GetAssessmentById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException();

            return _assessmentRepository.GetByID(id);
        }
        public AssessmentStudent GetStudentAssessmentById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException();

            return _studentAssessmentRepository.GetByID(id);
        }
        public void InsertStudentAssessment(AssessmentStudent studentAssessment)
        {
            _studentAssessmentRepository.Insert(studentAssessment);
        }
        public void UpdateStudentAssessment(AssessmentStudent studentAssessment)
        {
            _studentAssessmentRepository.Update(studentAssessment);
        }
        public void DeleteStudentAssessment(int id)
        {
            var studentAssessment = _studentAssessmentRepository.GetByID(id);
            if (studentAssessment != null)
            {
                _studentAssessmentRepository.Update(studentAssessment);
            }
        }
        public IList<Assessment> GetAllAssessments(bool? onlyActive = null)
        {
            return _assessmentRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public bool CheckAssessmentExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _assessmentRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusAssessment(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objAssessment = _assessmentRepository.GetByID(id);
            if (objAssessment != null)
            {
                objAssessment.IsActive = !objAssessment.IsActive;
                objAssessment.ModifiedOn = DateTime.Now;
                _assessmentRepository.Update(objAssessment);
            }

        }
        public IList<AssessmentStudent> GetAllAssessmentsByStudent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _assessmentStudentRepository.Table.Where(x => x.StudentId == id).ToList();
        }
        public AssessmentQuestion GetAssessmentQuestionById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException();

            return _assesQuestionRepository.GetByID(id);
        }
        public AssessmentQuestion GetAssessmentQuestion(int assessmentid, int questionid)
        {
            if (assessmentid == 0 && questionid == 0)
                throw new ArgumentNullException();

            return _assesQuestionRepository.Table.FirstOrDefault(x => x.AssessmentId == assessmentid && x.QuestionId == questionid);
        }
        public void InsertAssessmentQuestion(AssessmentQuestion assessmentQuestion)
        {
            _assesQuestionRepository.Insert(assessmentQuestion);
        }
        public void UpdateAssessmentQuestion(AssessmentQuestion assessmentQuestion)
        {
            _assesQuestionRepository.Update(assessmentQuestion);
        }
        public void DeleteAssessmentQuestion(int id)
        {
            var assessmentQuestion = _assesQuestionRepository.GetByID(id);
            if (assessmentQuestion != null)
            {
                _assesQuestionRepository.Update(assessmentQuestion);
            }
        }

        #endregion

        #region Global Settings

        public Settings GetSettingByKey(string key, int userid = 0)
        {
            if (string.IsNullOrEmpty(key))
                throw new System.Exception("Setting key missing.");

            var query = (from setting in _settingRepository.TableNoTracking
                         where setting.Name.Trim().ToLower() == key.Trim().ToLower() && (userid > 0 ? setting.UserId == userid : true)
                         select setting).FirstOrDefault();

            return query;
        }

        #endregion

        #region House
        public void InsertHouse(House house)
        {
            _houseRepository.Insert(house);
        }
        public void UpdateHouse(House house)
        {
            _houseRepository.Update(house);
        }
        public void DeleteHouse(int id)
        {
            var house = _houseRepository.GetByID(id);
            if (house != null)
            {
                house.IsActive = false;
                house.IsDeleted = true;
                _houseRepository.Update(house);
            }
        }
        public IList<House> GetAllHouses(bool? onlyActive = null)
        {
            return _houseRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }
        public House GetHouseById(int id)
        {
            if (id == 0)
                throw new System.Exception("House Id Is Missing.");

            return _houseRepository.GetByID(id);
        }
        public IList<House> GetHouseByName(string name, bool? active)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("House Name is Missing.");

            var query = _houseRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }
        public bool CheckHouseExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _houseRepository.Table.Any(a => (a.Name.ToLower() == name.ToLower()) && (!id.HasValue || id.Value != a.Id) && a.IsDeleted == false);
        }
        public void ToggleActiveStatusHouse(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objHouse = _houseRepository.GetByID(id);
            if (objHouse != null)
            {
                objHouse.IsActive = !objHouse.IsActive;
                objHouse.ModifiedOn = DateTime.Now;
                _houseRepository.Update(objHouse);
            }

        }
        #endregion

        #region Holiday
        public void InsertHoliday(Holiday objHoliday)
        {
            _holidayRepository.Insert(objHoliday);
        }
        public void UpdateHoliday(Holiday objHoliday)
        {
            _holidayRepository.Update(objHoliday);
        }
        public void DeleteHoliday(int id)
        {
            var objHoliday = _holidayRepository.GetByID(id);
            if (objHoliday != null)
            {
                objHoliday.IsActive = false;
                objHoliday.IsDeleted = true;
                _holidayRepository.Update(objHoliday);
            }
        }
        public Holiday GetHolidayById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _holidayRepository.GetByID(id);
        }
        public IList<Holiday> GetAllHolidays(bool? onlyActive = null)
        {
            return _holidayRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
        }

        public IList<Holiday> GetAllHolidaysByAcadmicYear(int id, bool? onlyActive = null)
        {
            return _holidayRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.AcadmicYearId == id && x.IsDeleted == false).ToList();
        }
        public bool CheckHolidayExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _holidayRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusHoliday(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objHoliday = _holidayRepository.GetByID(id);
            if (objHoliday != null)
            {
                objHoliday.IsActive = !objHoliday.IsActive;
                objHoliday.ModifiedOn = DateTime.Now;
                _holidayRepository.Update(objHoliday);
            }
        }
        #endregion

        #region Question

        public bool CheckQuestionExists(string name, int? id = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _questionRepository.Table.Any(x => (!id.HasValue || id.Value != x.Id) && (x.Name.Trim().ToLower() == name.Trim().ToLower()));
        }
        public void ToggleActiveStatusQuestion(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objQuestion = _questionRepository.GetByID(id);
            if (objQuestion != null)
            {
                objQuestion.IsActive = !objQuestion.IsActive;
                objQuestion.ModifiedOn = DateTime.Now;
                _questionRepository.Update(objQuestion);
            }

        }

        public Option GetOptionById(int id)
        {
            return _optionRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        #endregion

        #endregion

    }
}
