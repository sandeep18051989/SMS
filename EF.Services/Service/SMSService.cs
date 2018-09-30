using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
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
		private readonly IRepository<StudentClassDivision> _divisionClassStudentRepository;
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
		private readonly IRepository<StudentAssessment> _studentAssessmentRepository;
		private readonly IRepository<AssessmentQuestion> _assesQuestionRepository;
		private readonly IDataProvider _dataProvider;
		private readonly IDbContext _dbContext;

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
		IRepository<StudentClassDivision> divisionClassStudentRepository,
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
		ICommentService commentService, IReplyService replyService, IFileService fileService, IEventService eventService, IPictureService pictureService,
		IRepository<AcadmicYear> acadmicYearRepository,
		IRepository<Reaction> reactionRepository,
		IRepository<School> schoolRepository,
		IDataProvider dataProvider,
		IRepository<QuestionType> questionTypeRepository,
		IRepository<Question> questionRepository,
		IRepository<Option> optionRepository,
		IRepository<Assessment> assessmentRepository,
		IRepository<StudentAssessment> studentAssessmentRepository,
		IRepository<AssessmentQuestion> assesQuestionRepository,
		IDbContext dbContext)
		{
			this._studentRepository = studentRepository;
			this._teacherRepository = teacherRepository;
			this._studentAttendanceRepository = studentAttendanceRepository;
			this._userRepository = userRepository;
			this._religionRepository = religionRepository;
			this._employeeRepository = employeeRepository;
			this._employeeAttendanceRepository = employeeAttendanceRepository;
			this._feeCategoryRepository = feeCategoryRepository;
			this._casteRepository = casteRepository;
			this._categoryRepository = categoryRepository;
			this._classRepository = classRepository;
			this._subjectRepository = subjectRepository;
			this._divisionRepository = divisionRepository;
			this._divisionClassStudentRepository = divisionClassStudentRepository;
			this._divisionClassSubjectRepository = divisionClassSubjectRepository;
			this._designationRepository = designationRepository;
			this._dailyTimeTableSettingRepository = dailyTimeTableSettingRepository;
			this._dailyTimeTableRepository = dailyTimeTableRepository;
			this._qualificationRepository = qualificationRepository;
			this._allowanceRepository = allowanceRepository;
			this._paymentRepository = paymentRepository;
			this._vendorRepository = vendorRepository;
			this._productRepository = productRepository;
			this._purchaseRepository = purchaseRepository;
			this._feeDetailRepository = feeDetailRepository;
			this._messageGroupRepository = messageGroupRepository;
			this._messageRepository = messageRepository;
			this._studentMessageGroupRepository = studentMessageGroupRepository;
			this._examRepository = examRepository;
			this._commentService = commentService;
			this._replyService = replyService;
			this._fileService = fileService;
			this._eventService = eventService;
			this._pictureService = pictureService;
			this._acadmicYearRepository = acadmicYearRepository;
			this._reactionRepository = reactionRepository;
			this._schoolRepository = schoolRepository;
			this._dataProvider = dataProvider;
			this._dbContext = dbContext;
			this._questionTypeRepository = questionTypeRepository;
			this._questionRepository = questionRepository;
			this._optionRepository = optionRepository;
			this._assessmentRepository = assessmentRepository;
			this._studentAssessmentRepository = studentAssessmentRepository;
			this._assesQuestionRepository = assesQuestionRepository;
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
			var school = _schoolRepository.Table.FirstOrDefault(s => s.Id == id);
			if (school != null)
			{
				school.IsActive = false;
				school.IsDeleted = true;
				_schoolRepository.Update(school);
			}
		}
		public IList<School> GetAllSchools(bool? active)
		{
			var query = _schoolRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.FullName).ToList();
		}
		public School GetSchoolById(int id)
		{
			if (id == 0)
				throw new System.Exception("School Is Missing.");

			return _schoolRepository.Table.FirstOrDefault(x => x.Id == id);
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
			var student = _studentRepository.Table.FirstOrDefault(s => s.Id == id);
			if (student != null)
			{
				student.IsActive = false;
				student.IsDeleted = true;
				_studentRepository.Update(student);
			}
		}
		public IList<Student> GetAllStudents(bool? active)
		{
			var query = _studentRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.FName).ToList();
		}
		public Student GetStudentById(int id)
		{
			if (id == 0)
				throw new System.Exception("Student Is Missing.");

			return _studentRepository.Table.FirstOrDefault(x => x.Id == id);
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
		public IList<Student> SearchStudents(bool? active, string religion = null, string classname = null, int? acedemicyearid = null)
		{
			var query = _studentRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (acedemicyearid.HasValue)
				query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

			if (!String.IsNullOrEmpty(classname))
				query = query.Where(s => s.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.FName).ToList();

		}
		public IList<Student> SearchStudents(bool? active, int religion = 0, int classid = 0, int? acedemicyearid = null)
		{
			var query = _studentRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (acedemicyearid.HasValue)
				query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

			if (classid > 0)
				query = query.Where(s => s.Class.Id == classid).ToList();

			return query.OrderBy(s => s.FName).ToList();

		}
		public int GetTotalStudents()
		{
			return _studentRepository.Table.Count(x => x.IsActive && x.IsDeleted == false);
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
			var employee = _employeeRepository.Table.FirstOrDefault(s => s.Id == id);
			if (employee != null)
			{
				employee.IsActive = false;
				employee.IsDeleted = true;
				_employeeRepository.Update(employee);
			}
		}
		public IList<Employee> GetAllEmployees(bool? active)
		{
			var query = _employeeRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.EmpFName).ToList();
		}
		public Employee GetEmployeeById(int id)
		{
			if (id == 0)
				throw new System.Exception("Employee Id Is Missing.");

			return _employeeRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Employee> GetEmployeesByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Employee Name is Missing.");

			var query = _employeeRepository.Table.Where(a => (a.EmpFName.ToLower().Contains(name.ToLower()) || a.EmpLName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.Username).ToList();
		}
		public IList<Employee> SearchEmployees(bool? active, string religion = null, string designation = null)
		{
			var query = _employeeRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (!String.IsNullOrEmpty(religion))
				query = query.Where(s => s.Religion.Name.Trim().ToLower() == religion.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.EmpFName).ToList();

		}
		public IList<Employee> SearchEmployees(bool? active, int religion = 0, int designationid = 0)
		{
			var query = _employeeRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (designationid > 0)
				query = query.Where(s => s.DesignationId == designationid).ToList();

			if (religion > 0)
				query = query.Where(s => s.Religion.Id == religion).ToList();

			return query.OrderBy(s => s.EmpFName).ToList();

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
			var studentattendance = _studentAttendanceRepository.Table.FirstOrDefault(s => s.Id == id);
			_studentAttendanceRepository.Delete(studentattendance);
		}
		public IList<StudentAttendance> GetAllStudentAttendances(bool? active)
		{
			return _studentAttendanceRepository.Table.OrderByDescending(x => x.Date).ToList();
		}
		public StudentAttendance GetStudentAttendanceById(int id)
		{
			if (id == 0)
				throw new System.Exception("StudentAttendance Id Is Missing.");

			return _studentAttendanceRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<StudentAttendance> GetStudentAttendanceByDate(int DD, int MM, int yyyy)
		{
			return _studentAttendanceRepository.Table.Where(a => (a.DD == DD && a.MM == MM && a.YYYY == yyyy)).OrderByDescending(x => x.Date).ToList();
		}
		public IList<StudentAttendance> SearchStudentAttendances(bool? active, string studentusername = null, string classname = null, int? acedemicyearid = null)
		{
			var query = _studentAttendanceRepository.Table.ToList();

			if (!String.IsNullOrEmpty(studentusername))
				query = query.Where(s => s.Student.UserName.Trim().ToLower() == studentusername.Trim().ToLower()).ToList();

			if (!String.IsNullOrEmpty(classname))
				query = query.Where(s => s.Student.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

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
			var religion = _religionRepository.Table.FirstOrDefault(s => s.Id == id);
			_religionRepository.Delete(religion);
		}
		public IList<Religion> GetAllReligion(bool? active)
		{
			return _religionRepository.Table.OrderBy(x => x.Name).ToList();
		}
		public Religion GetReligionById(int id)
		{
			if (id == 0)
				throw new System.Exception("Religion Id Is Missing.");

			return _religionRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Religion> GetReligionByName(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Religion Name is missing.");

			var query = _religionRepository.Table.Where(a => (a.Name.Trim().ToLower() == name.Trim().ToLower())).ToList();
			return query.OrderBy(x => x.Name).ToList();
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
			var employeeattendance = _employeeAttendanceRepository.Table.FirstOrDefault(s => s.Id == id);
			_employeeAttendanceRepository.Delete(employeeattendance);
		}
		public EmployeeAttendance GetEmployeeAttendanceById(int id)
		{
			if (id == 0)
				throw new System.Exception("EmployeeAttendance Id Is Missing.");

			return _employeeAttendanceRepository.Table.FirstOrDefault(x => x.Id == id);
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
			var feecategory = _feeCategoryRepository.Table.FirstOrDefault(s => s.Id == id);
			if (feecategory != null)
			{
				feecategory.IsActive = false;
				feecategory.IsDeleted = true;
				_feeCategoryRepository.Update(feecategory);
			}
		}
		public IList<FeeCategory> GetAllFeeCategories(bool? active)
		{
			var query = _feeCategoryRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.CategoryName).ToList();
		}
		public FeeCategory GetFeeCategoryById(int id)
		{
			if (id == 0)
				throw new System.Exception("FeeCategory Id Is Missing.");

			return _feeCategoryRepository.Table.FirstOrDefault(x => x.Id == id);
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
				query = query.Where(s => s.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.CategoryName).ToList();

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
			var caste = _casteRepository.Table.FirstOrDefault(s => s.Id == id);
			if (caste != null)
			{
				caste.IsActive = false;
				caste.IsDeleted = true;
				_casteRepository.Update(caste);
			}
		}
		public IList<Caste> GetAllCastes(bool? active)
		{
			var query = _casteRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.CasteName).ToList();
		}
		public Caste GetCasteById(int id)
		{
			if (id == 0)
				throw new System.Exception("Caste Id Is Missing.");

			return _casteRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Caste> GetCasteByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Caste Name is Missing.");

			var query = _casteRepository.Table.Where(a => (a.CasteName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.CasteName).ToList();
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

			return query.OrderBy(s => s.CasteName).ToList();

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
			var category = _categoryRepository.Table.FirstOrDefault(s => s.Id == id);
			if (category != null)
			{
				category.IsActive = false;
				category.IsDeleted = true;
				_categoryRepository.Update(category);
			}
		}
		public IList<Category> GetAllCategories(bool? active)
		{
			var query = _categoryRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.CategoryName).ToList();
		}
		public Category GetCategoryById(int id)
		{
			if (id == 0)
				throw new System.Exception("Category Id Is Missing.");

			return _categoryRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Category> GetCategoryByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Category Name is Missing.");

			var query = _categoryRepository.Table.Where(a => (a.CategoryName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.CategoryName).ToList();
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
			var objClass = _classRepository.Table.FirstOrDefault(s => s.Id == id);
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

			return _classRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Class> GetClassByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Class Name is Missing.");

			var query = _classRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.Name).ToList();
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
			var division = _divisionRepository.Table.FirstOrDefault(s => s.Id == id);
			if (division != null)
			{
				division.IsActive = false;
				division.IsDeleted = true;
				_divisionRepository.Update(division);
			}
		}
		public IList<Division> GetAllDivisions(bool? active)
		{
			var query = _divisionRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.DivisionName).ToList();
		}
		public Division GetDivisionById(int id)
		{
			if (id == 0)
				throw new System.Exception("Division Is Missing.");

			return _divisionRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Division> GetDivisionsByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Division Name is Missing.");

			var query = _divisionRepository.Table.Where(a => a.DivisionName.ToLower().Contains(name.ToLower()) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.DivisionName).ToList();
		}
		public IList<Division> SearchDivisions(bool? active, string classname = null)
		{
			var query = _divisionRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (!String.IsNullOrEmpty(classname))
				query = query.Where(s => s.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.DivisionName).ToList();

		}
		public IList<Division> SearchDivisions(bool? active, int classid = 0)
		{
			var query = _divisionRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (classid > 0)
				query = query.Where(s => s.ClassId == classid).ToList();

			return query.OrderBy(s => s.DivisionName).ToList();

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
			var subject = _subjectRepository.Table.FirstOrDefault(s => s.Id == id);
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

			return _subjectRepository.Table.FirstOrDefault(x => x.Id == id);
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
		#endregion
		#region Student Class Division
		public void InsertStudentClassDivision(StudentClassDivision divisionClassStudent)
		{
			_divisionClassStudentRepository.Insert(divisionClassStudent);
		}
		public void UpdateStudentClassDivision(StudentClassDivision divisionClassStudent)
		{
			_divisionClassStudentRepository.Update(divisionClassStudent);
		}
		public void DeleteStudentClassDivision(int id)
		{
			var divisionClassStudent = _divisionClassStudentRepository.Table.FirstOrDefault(s => s.Id == id);
			//divisionClassStudent.IsActive = false;
			//divisionClassStudent.IsDeleted = true;
			_divisionClassStudentRepository.Update(divisionClassStudent);
		}
		public IList<StudentClassDivision> GetAllDivisionClassStudentMappings(bool? active)
		{
			return _divisionClassStudentRepository.Table.OrderBy(x => x.DivisionName).ToList();
		}
		public StudentClassDivision GetDivisionClassStudentMappingById(int id)
		{
			if (id == 0)
				throw new System.Exception("Division Class Student Mapping Id Is Missing.");

			return _divisionClassStudentRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<StudentClassDivision> SearchDivisionClassStudentMappings(bool? active, string division = null, string classname = null, string student = null, int? acedemicyearid = null)
		{
			var query = _divisionClassStudentRepository.Table.ToList();

			if (!String.IsNullOrEmpty(classname))
				query = query.Where(s => s.Class.Name.Trim().ToLower() == classname.Trim().ToLower()).ToList();

			if (!String.IsNullOrEmpty(division))
				query = query.Where(s => s.DivisionName.Trim().ToLower() == division.Trim().ToLower()).ToList();

			if (!String.IsNullOrEmpty(student))
				query = query.Where(s => s.StudentName.Trim().ToLower() == student.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.DivisionName).ToList();

		}
		public IList<StudentClassDivision> SearchDivisionClassStudents(bool? active, int divisionid = 0, int classid = 0, int studentid = 0, int? acedemicyearid = null)
		{
			var query = _divisionClassStudentRepository.Table.ToList();

			if (classid > 0)
				query = query.Where(s => s.Class.Id == classid).ToList();

			if (divisionid > 0)
				query = query.Where(s => s.DivisionId == divisionid).ToList();

			if (studentid > 0)
				query = query.Where(s => s.StudentId == studentid).ToList();

			return query.OrderBy(s => s.DivisionName).ToList();

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
			var divisionClassSubject = _divisionClassSubjectRepository.Table.FirstOrDefault(s => s.Id == id);
			//divisionClassSubject.IsActive = false;
			//divisionClassSubject.IsDeleted = true;
			_divisionClassSubjectRepository.Update(divisionClassSubject);
		}
		public IList<DivisionSubject> GetAllDivisionSubjectMappings(bool? active)
		{
			return _divisionClassSubjectRepository.Table.OrderBy(x => x.Division.DivisionName).ToList();
		}
		public DivisionSubject GetDivisionSubjectMappingById(int id)
		{
			if (id == 0)
				throw new System.Exception("Division Class Subject Mapping Id Is Missing.");

			return _divisionClassSubjectRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<DivisionSubject> SearchDivisionSubjectMappings(bool? active, string division = null, string subject = null)
		{
			var query = _divisionClassSubjectRepository.Table.ToList();

			if (!String.IsNullOrEmpty(division))
				query = query.Where(s => s.Division.DivisionName.Trim().ToLower() == division.Trim().ToLower()).ToList();

			if (!String.IsNullOrEmpty(subject))
				query = query.Where(s => s.Subject.Name.Trim().ToLower() == subject.Trim().ToLower()).ToList();

			return query.OrderBy(s => s.Division.DivisionName).ToList();

		}
		public IList<DivisionSubject> SearchDivisionSubjects(bool? active, int divisionid = 0, int subjectid = 0)
		{
			var query = _divisionClassSubjectRepository.Table.ToList();

			if (divisionid > 0)
				query = query.Where(s => s.DivisionId == divisionid).ToList();

			if (subjectid > 0)
				query = query.Where(s => s.SubjectId == subjectid).ToList();

			return query.OrderBy(s => s.Division.DivisionName).ToList();

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
			var designation = _designationRepository.Table.FirstOrDefault(s => s.Id == id);
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

			return _designationRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Designation> GetDesignationByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Designation Name is Missing.");

			var query = _designationRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.Name).ToList();
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
			var teacher = _teacherRepository.Table.FirstOrDefault(s => s.Id == id);
			if (teacher != null)
			{
				teacher.IsActive = false;
				teacher.IsDeleted = true;
				_teacherRepository.Update(teacher);
			}
		}
		public IList<Teacher> GetAllTeachers(bool? active)
		{
			var query = _teacherRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.Name).ToList();
		}
		public Teacher GetTeacherById(int id)
		{
			if (id == 0)
				throw new System.Exception("Teacher Id Is Missing.");

			return _teacherRepository.Table.FirstOrDefault(x => x.Id == id);
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

			return query.OrderBy(s => s.Name).ToList();

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

			return query.OrderBy(s => s.Name).ToList();

		}
		public virtual IList<Teacher> GetTeachersByIds(int[] userIds)
		{
			if (userIds == null || userIds.Length == 0)
				return new List<Teacher>();

			var query = from r in _teacherRepository.Table
							where userIds.Contains(r.Id)
							select r;

			var users = query.ToList();

			var sortedTeachers = new List<Teacher>();
			foreach (int id in userIds)
			{
				var teacher = users.Find(x => x.Id == id);
				if (teacher != null)
					sortedTeachers.Add(teacher);
			}
			return sortedTeachers;
		}
		public virtual void DeleteTeachers(IList<Teacher> users)
		{
			if (users == null)
				throw new ArgumentNullException("users");

			foreach (var _teacher in users)
			{
				if (_teacher.Id != 1)
					_teacher.IsDeleted = true;

				_teacherRepository.Update(_teacher);

			}
		}
		public void ToggleUser(int id)
		{
			if (id == 0)
				throw new ArgumentNullException("User");

			var _teacher = _userRepository.Table.Where(x => x.Id == id && x.Id != 1).FirstOrDefault();
			if (_teacher != null)
			{
				_teacher.IsActive = !_teacher.IsActive;
				_userRepository.Update(_teacher);
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
			var dailyTimeTableSetting = _dailyTimeTableSettingRepository.Table.FirstOrDefault(s => s.Id == id);
			//dailyTimeTableSetting.IsActive = false;
			//dailyTimeTableSetting.IsDeleted = true;
			_dailyTimeTableSettingRepository.Update(dailyTimeTableSetting);
		}
		public TimeTableSetting GetTimeTableSettingById(int id)
		{
			if (id == 0)
				throw new System.Exception(" Time Table Setting Id Is Missing.");

			return _dailyTimeTableSettingRepository.Table.FirstOrDefault(x => x.Id == id);
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
			var dailyTimeTable = _dailyTimeTableRepository.Table.FirstOrDefault(s => s.Id == id);
			if (dailyTimeTable != null)
			{
				dailyTimeTable.IsActive = false;
				dailyTimeTable.IsDeleted = true;
				_dailyTimeTableRepository.Update(dailyTimeTable);
			}
		}
		public IList<TimeTable> GetAllTimeTables(bool? active)
		{
			var query = _dailyTimeTableRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.LectureNumber).ToList();
		}
		public TimeTable GetTimeTableById(int id)
		{
			if (id == 0)
				throw new System.Exception("Time Table Id Is Missing.");

			return _dailyTimeTableRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<TimeTable> SearchTimeTables(bool? active, int? acedemicyearid = null)
		{
			var query = _dailyTimeTableRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (acedemicyearid.HasValue)
				query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

			return query.OrderBy(s => s.LectureNumber).ToList();

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
			var qualification = _qualificationRepository.Table.FirstOrDefault(s => s.Id == id);
			//qualification.IsActive = false;
			//qualification.IsDeleted = true;
			_qualificationRepository.Update(qualification);
		}
		public IList<Qualification> GetAllQualifications(bool? active)
		{
			return _qualificationRepository.Table.OrderBy(x => x.Name).ToList();
		}
		public Qualification GetQualificationById(int id)
		{
			if (id == 0)
				throw new System.Exception("Qualification Id Is Missing.");

			return _qualificationRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Qualification> GetQualificationByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Qualification Name is Missing.");

			return _qualificationRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower()))).OrderBy(x => x.Name).ToList();
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
			var allowance = _allowanceRepository.Table.FirstOrDefault(s => s.Id == id);
			//allowance.IsActive = false;
			//allowance.IsDeleted = true;
			_allowanceRepository.Update(allowance);
		}
		public Allowance GetAllowanceById(int id)
		{
			if (id == 0)
				throw new System.Exception("Time Table Setting Id Is Missing.");

			return _allowanceRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Allowance> GetAllAllowances(bool? deleted)
		{
			return _allowanceRepository.Table.Where(a => a.IsDeleted == deleted.HasValue && deleted.Value).OrderByDescending(x => x.CreatedOn).ToList();
		}
		public IList<Allowance> SearchAllowances(bool? active, string designation = null)
		{
			var query = _allowanceRepository.Table.ToList();

			if (!String.IsNullOrEmpty(designation))
				query = query.Where(s => s.Designation.Name.Trim().ToLower() == designation.Trim().ToLower()).ToList();

			return query.OrderByDescending(s => s.CreatedOn).ToList();
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
			var payment = _paymentRepository.Table.FirstOrDefault(s => s.Id == id);
			//payment.IsActive = false;
			//payment.IsDeleted = true;
			_paymentRepository.Update(payment);
		}
		public Payment GetPaymentById(int id)
		{
			if (id == 0)
				throw new System.Exception("Payment Id Is Missing.");

			return _paymentRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Payment> GetAllPayments(bool? active)
		{
			return _paymentRepository.Table.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public IList<Payment> SearchPayments(bool? active, int? allowanceid, string employee = null, int? acedemicyearid = null)
		{
			var query = _paymentRepository.Table.ToList();

			if (allowanceid.HasValue)
				query = query.Where(s => s.AllowanceId == allowanceid).ToList();

			return query.OrderByDescending(s => s.CreatedOn).ToList();
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
			var vendor = _vendorRepository.Table.FirstOrDefault(s => s.Id == id);
			if (vendor != null)
			{
				vendor.IsActive = false;
				vendor.IsDeleted = true;
				_vendorRepository.Update(vendor);
			}
		}
		public IList<Vendor> GetAllVendors(bool? active)
		{
			var query = _vendorRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public Vendor GetVendorById(int id)
		{
			if (id == 0)
				throw new System.Exception("Vendor Id Is Missing.");

			return _vendorRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Vendor> GetVendorsByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Vendor Name is Missing.");

			var query = _vendorRepository.Table.Where(a => (a.VendorName.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.VendorName).ToList();
		}
		public IList<Vendor> SearchVendors(bool? active, string religion = null, string designation = null, int? acedemicyearid = null)
		{
			var query = _vendorRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(s => s.IsActive == active).ToList();

			if (acedemicyearid.HasValue)
				query = query.Where(s => s.AcadmicYearId == acedemicyearid.Value).ToList();

			return query.OrderBy(s => s.VendorName).ToList();

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
			var purchase = _purchaseRepository.Table.FirstOrDefault(s => s.Id == id);
			//purchase.IsActive = false;
			//purchase.IsDeleted = true;
			_purchaseRepository.Update(purchase);
		}
		public IList<Purchase> GetAllPurchases(bool? active)
		{
			return _purchaseRepository.Table.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public Purchase GetPurchaseById(int id)
		{
			if (id == 0)
				throw new System.Exception("Purchase Id Is Missing.");

			return _purchaseRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Purchase> SearchPurchases(bool? active, string product = null, string vendor = null, int? acedemicyearid = null)
		{
			var query = _purchaseRepository.Table.ToList();

			if (!String.IsNullOrEmpty(product))
				query = query.Where(s => s.Product.Name.Trim().ToLower() == product.Trim().ToLower()).ToList();

			return query.OrderByDescending(s => s.IPurchaseDate).ToList();

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
			var feeDetail = _feeDetailRepository.Table.FirstOrDefault(s => s.Id == id);
			//feeDetail.IsActive = false;
			//feeDetail.IsDeleted = true;
			_feeDetailRepository.Update(feeDetail);
		}
		public IList<FeeDetail> GetAllFeeDetails(bool? active)
		{
			return _feeDetailRepository.Table.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public FeeDetail GetFeeDetailById(int id)
		{
			if (id == 0)
				throw new System.Exception("FeeDetail Id Is Missing.");

			return _feeDetailRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<FeeDetail> SearchFeeDetails(bool? active, string student = null, string category = null, int? acedemicyearid = null)
		{
			var query = _feeDetailRepository.Table.ToList();

			if (active.HasValue)
				query = query.ToList();

			if (!String.IsNullOrEmpty(student))
				query = query.Where(s => s.Student.UserName.Trim().ToLower() == student.Trim().ToLower()).ToList();

			if (!String.IsNullOrEmpty(category))
				query = query.Where(s => s.FeeCategoryStructure.CategoryName.Trim().ToLower() == category.Trim().ToLower()).ToList();

			return query.OrderByDescending(s => s.Date).ToList();

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
			var messageGroup = _messageGroupRepository.Table.FirstOrDefault(s => s.Id == id);
			//messageGroup.IsActive = false;
			//messageGroup.IsDeleted = true;
			_messageGroupRepository.Update(messageGroup);
		}
		public IList<MessageGroup> GetAllMessageGroups(bool? active)
		{
			return _messageGroupRepository.Table.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public MessageGroup GetMessageGroupById(int id)
		{
			if (id == 0)
				throw new System.Exception("MessageGroup Id Is Missing.");

			return _messageGroupRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<MessageGroup> GetMessageGroupsByName(string name, bool? active)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Message Group Name is Missing.");

			return _messageGroupRepository.Table.Where(a => (a.GroupName.ToLower().Contains(name.ToLower()))).OrderBy(x => x.GroupName).ToList();
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
			var studentMessageGroup = _studentMessageGroupRepository.Table.FirstOrDefault(s => s.Id == id);
			//studentMessageGroup.IsActive = false;
			//studentMessageGroup.IsDeleted = true;
			_studentMessageGroupRepository.Update(studentMessageGroup);
		}
		public IList<Student_MessageGroup> GetAllStudent_MessageGroups(bool? active)
		{
			var query = _studentMessageGroupRepository.Table.ToList();
			return query.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public Student_MessageGroup GetStudent_MessageGroupById(int id)
		{
			if (id == 0)
				throw new System.Exception("Student_Message Group Id Is Missing.");

			return _studentMessageGroupRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Student_MessageGroup> GetStudent_MessageGroupsByStudentUsername(string username, bool? active)
		{
			if (string.IsNullOrEmpty(username))
				throw new Exception("Message Group Student Username is Missing.");

			var query = _studentMessageGroupRepository.Table.Where(a => (a.Student.UserName.ToLower().Contains(username.ToLower()))).ToList();

			if (active.HasValue)
				query = query.ToList();

			return query.OrderByDescending(x => x.CreatedOn).ToList();
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
			var message = _messageRepository.Table.FirstOrDefault(s => s.Id == id);
			if (message != null)
			{
				message.IsActive = false;
				message.IsDeleted = true;
				_messageRepository.Update(message);
			}
		}
		public IList<Message> GetAllMessages(bool? active)
		{
			var query = _messageRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderByDescending(x => x.CreatedOn).ToList();
		}
		public Message GetMessageById(int id)
		{
			if (id == 0)
				throw new System.Exception("Student_Message Group Id Is Missing.");

			return _messageRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<Message> GetMessagesByMessageGroup(string messagegroup, bool? active)
		{
			if (string.IsNullOrEmpty(messagegroup))
				throw new Exception("Message Group Name is Missing.");

			var query = _messageRepository.Table.Where(a => (a.MessageGroup.GroupName.ToLower().Contains(messagegroup.ToLower())) && a.IsDeleted == false).ToList();

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
			var exam = _examRepository.Table.FirstOrDefault(s => s.Id == id);
			exam.IsActive = false;
			exam.IsDeleted = true;
			_examRepository.Update(exam);
		}
		public IList<Exam> GetAllExams(bool? active)
		{
			var query = _examRepository.Table.ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive).ToList();

			return query.OrderBy(x => x.ExamName).ToList();
		}
		public Exam GetExamById(int id)
		{
			if (id == 0)
				throw new System.Exception("Exam Id Is Missing.");

			return _examRepository.Table.FirstOrDefault(x => x.Id == id);
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
			var objAcadmicYear = _acadmicYearRepository.Table.FirstOrDefault(s => s.Id == id);
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

			return _acadmicYearRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public AcadmicYear GetActiveAcadmicYear()
		{
			return _acadmicYearRepository.Table.FirstOrDefault(x => x.IsActive);
		}
		public AcadmicYear GetAcadmicYearByName(string name, bool? active=null)
		{
			if (string.IsNullOrEmpty(name))
				throw new Exception("Acadmic Year is Missing.");

			var query = _acadmicYearRepository.Table.Where(a => (a.Name.ToLower().Contains(name.ToLower())) && a.IsDeleted == false).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.FirstOrDefault();
		}

		public IList<AcadmicYear> GetAllAcadmicYears(bool? active)
		{
			var query = _acadmicYearRepository.Table.OrderBy(x => x.Name).ToList();

			if (active.HasValue)
				query = query.Where(x => x.IsActive == active).ToList();

			return query.OrderBy(x => x.Name).ToList();
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

			return _reactionRepository.Table.FirstOrDefault(x => x.Id == id);
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
						Username = _userRepository.Table.FirstOrDefault(x => x.Id == userid) != null ? _userRepository.Table.FirstOrDefault(x => x.Id == userid).UserName : "",
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
						Username = _userRepository.Table.FirstOrDefault(x => x.Id == userid) != null ? _userRepository.Table.FirstOrDefault(x => x.Id == userid).UserName : "",
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
						Username = _userRepository.Table.FirstOrDefault(x => x.Id == userid) != null ? _userRepository.Table.FirstOrDefault(x => x.Id == userid).UserName : "",
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
		public QuestionType GetQuestionTypeById(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			return _questionTypeRepository.Table.FirstOrDefault(x => x.Id == id);
		}
		public IList<QuestionType> GetAllQuestionTypes()
		{
			return _questionTypeRepository.Table.OrderBy(x => x.Name).ToList();
		}
		#endregion
		#region Question & Assessments

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
			var question = _questionRepository.Table.FirstOrDefault(s => s.Id == id);
			if (question != null)
			{
				question.IsActive = false;
				question.IsDeleted = true;
				_questionRepository.Update(question);
			}
		}

		public Question GetQuestionById(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			return _questionRepository.Table.FirstOrDefault(x => x.Id == id);
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
			var option = _optionRepository.Table.FirstOrDefault(s => s.Id == id);
			if (option != null)
			{
				_optionRepository.Update(option);
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
			var assessment = _assessmentRepository.Table.FirstOrDefault(s => s.Id == id);
			if (assessment != null)
			{
				_assessmentRepository.Update(assessment);
			}
		}

		public IList<StudentAssessment> GetAssessmentByStudentId(int studentid, bool? active = null, bool? completed = null, DateTime? starttime = null, DateTime? endtime = null)
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

			return _assessmentRepository.Table.FirstOrDefault(x => x.Id == id);
		}

		public StudentAssessment GetStudentAssessmentById(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			return _studentAssessmentRepository.Table.FirstOrDefault(x => x.Id == id);
		}

		public void InsertstudentAssessment(StudentAssessment studentAssessment)
		{
			_studentAssessmentRepository.Insert(studentAssessment);
		}
		public void UpdatestudentAssessment(StudentAssessment studentAssessment)
		{
			_studentAssessmentRepository.Update(studentAssessment);
		}
		public void DeletestudentAssessment(int id)
		{
			var studentAssessment = _studentAssessmentRepository.Table.FirstOrDefault(s => s.Id == id);
			if (studentAssessment != null)
			{
				_studentAssessmentRepository.Update(studentAssessment);
			}
		}

		#endregion
		#endregion

	}
}
