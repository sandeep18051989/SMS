using System;
using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public interface ISMSService
	{
		#region School
		void InsertSchool(School school);
		void UpdateSchool(School school);
		void DeleteSchool(int id);
		IList<School> GetAllSchools(bool? active);
		School GetSchoolById(int id);
		#endregion
		#region Student
		void InsertStudent(Student student);
		void UpdateStudent(Student student);
		void DeleteStudent(int id);
		IList<Student> GetAllStudents(bool? active);
		Student GetStudentById(int id);
		IList<Student> GetStudentsByName(string name, bool? active);
		IList<Student> SearchStudents(bool? active, string religion = null, string classname = null, int? acedemicyearid = null);
		IList<Student> SearchStudents(bool? active, int religion = 0, int classid = 0, int? acedemicyearid = null);
		int GetTotalStudents();
		#endregion
		#region Employee
		void InsertEmployee(Employee employee);
		void UpdateEmployee(Employee employee);
		void DeleteEmployee(int id);
		IList<Employee> GetAllEmployees(bool? active);
		Employee GetEmployeeById(int id);
		IList<Employee> GetEmployeesByName(string name, bool? active);
		IList<Employee> SearchEmployees(bool? active, string religion = null, string designation = null);
		IList<Employee> SearchEmployees(bool? active, int religion = 0, int designationid = 0);
		#endregion
		#region Student Attendance
		void InsertStudentAttendance(StudentAttendance studentattendance);
		void UpdateStudentAttendance(StudentAttendance studentattendance);
		void DeleteStudentAttendance(int id);
		IList<StudentAttendance> GetStudentAttendanceByDate(int DD, int MM, int yyyy);
		StudentAttendance GetStudentAttendanceById(int id);
		IList<StudentAttendance> GetAllStudentAttendances(bool? active);
		IList<StudentAttendance> SearchStudentAttendances(bool? active, string studentusername = null, string classname = null, int? acedemicyearid = null);
		#endregion
		#region Religion
		void InsertReligion(Religion religion);
		void UpdateReligion(Religion religion);
		void DeleteReligion(int id);
		IList<Religion> GetAllReligion(bool? active);
		Religion GetReligionById(int id);
		IList<Religion> GetReligionByName(string name);
		#endregion
		#region EmployeeAttendance
		void InsertEmployeeAttendance(EmployeeAttendance employeeattendance);
		void UpdateEmployeeAttendance(EmployeeAttendance employeeattendance);
		void DeleteEmployeeAttendance(int id);
		EmployeeAttendance GetEmployeeAttendanceById(int id);
		IPagedList<EmployeeAttendance> GetEmployeeAttendanceByDateAndEmployee(PagerParams param, DateTime? Date, int EmployeeId = 0);
		#endregion
		#region Fee Category
		void InsertFeeCategory(FeeCategory feecategory);
		void UpdateFeeCategory(FeeCategory feecategory);
		void DeleteFeeCategory(int id);
		IList<FeeCategory> GetAllFeeCategories(bool? active);
		FeeCategory GetFeeCategoryById(int id);
		IList<FeeCategory> GetFeeCategoryByName(string name, bool? active);
		IList<FeeCategory> SearchFeeCategories(bool? active, string category = null, string classname = null, int? acadmicyearid = null);
		#endregion
		#region Caste
		void InsertCaste(Caste caste);
		void UpdateCaste(Caste caste);
		void DeleteCaste(int id);
		IList<Caste> GetAllCastes(bool? active);
		Caste GetCasteById(int id);
		IList<Caste> GetCasteByName(string name, bool? active);
		IList<Caste> SearchFeeCastes(bool? active, string religion = null, int? acedemicyearid = null);
		#endregion
		#region Category
		void InsertCategory(Category category);
		void UpdateCategory(Category category);
		void DeleteCategory(int id);
		IList<Category> GetAllCategories(bool? active);
		Category GetCategoryById(int id);
		IList<Category> GetCategoryByName(string name, bool? active);
		#endregion
		#region Class
		void InsertClass(Class objClass);
		void UpdateClass(Class objClass);
		void DeleteClass(int id);
		Class GetClassById(int id);
		IList<Class> GetClassByName(string name, bool? active);
		#endregion
		#region Division
		void InsertDivision(Division division);
		void UpdateDivision(Division division);
		void DeleteDivision(int id);
		IList<Division> GetAllDivisions(bool? active);
		Division GetDivisionById(int id);
		IList<Division> GetDivisionsByName(string name, bool? active);
		IList<Division> SearchDivisions(bool? active, string classname = null);
		IList<Division> SearchDivisions(bool? active, int classid = 0);
		#endregion
		#region Subject
		void InsertSubject(Subject subject);
		void UpdateSubject(Subject subject);
		void DeleteSubject(int id);
		Subject GetSubjectById(int id);
		IList<Subject> GetSubjectByName(string name, bool? active);
		#endregion
		#region Division Class Student
		void InsertStudentClassDivision(StudentClassDivision divisionClassStudent);
		void UpdateStudentClassDivision(StudentClassDivision divisionClassStudent);
		void DeleteStudentClassDivision(int id);
		IList<StudentClassDivision> GetAllDivisionClassStudentMappings(bool? active);
		StudentClassDivision GetDivisionClassStudentMappingById(int id);
		IList<StudentClassDivision> SearchDivisionClassStudentMappings(bool? active, string division = null, string classname = null, string studentusername = null, int? acedemicyearid = null);
		IList<StudentClassDivision> SearchDivisionClassStudents(bool? active, int divisionid = 0, int classid = 0, int studentid = 0, int? acedemicyearid = null);

		#endregion
		#region Division Subject
		void InsertDivisionSubject(DivisionSubject divisionClassSubject);
		void UpdateDivisionSubject(DivisionSubject divisionClassSubject);
		void DeleteDivisionSubject(int id);
		IList<DivisionSubject> GetAllDivisionSubjectMappings(bool? active);
		DivisionSubject GetDivisionSubjectMappingById(int id);
		IList<DivisionSubject> SearchDivisionSubjectMappings(bool? active, string division = null, string subject = null);
		IList<DivisionSubject> SearchDivisionSubjects(bool? active, int divisionid = 0, int subjectid = 0);
		#endregion
		#region Designation
		void InsertDesignation(Designation designation);
		void UpdateDesignation(Designation designation);
		void DeleteDesignation(int id);
		Designation GetDesignationById(int id);
		IList<Designation> GetDesignationByName(string name, bool? active);
		#endregion
		#region Teacher
		void InsertTeacher(Teacher teacher);
		void UpdateTeacher(Teacher teacher);
		void DeleteTeacher(int id);
		IList<Teacher> GetAllTeachers(bool? active);
		Teacher GetTeacherById(int id);
		IList<Teacher> GetTeachersByName(string name, bool? active);
		IList<Teacher> SearchTeachers(bool? active, string subject = null, string qualification = null, string employee = null, int? acedemicyearid = null);
		IList<Teacher> SearchTeachers(bool? active, int subjectid = 0, int qualificationid = 0, int employeeid = 0, int? acedemicyearid = null);
		int GetTotalTeachers();
		#endregion
		#region Time Table Setting
		void InsertTimeTableSetting(TimeTableSetting TimeTableSetting);
		void UpdateTimeTableSetting(TimeTableSetting TimeTableSetting);
		void DeleteTimeTableSetting(int id);
		TimeTableSetting GetTimeTableSettingById(int id);
		IList<TimeTableSetting> GetAllTimeTableSettings();
		IList<TimeTableSetting> SearchTimeTableSettings(bool? active, int? acedemicyearid = null);
		#endregion
		#region Time Table
		void InsertTimeTable(TimeTable TimeTable);
		void UpdateTimeTable(TimeTable TimeTable);
		void DeleteTimeTable(int id);
		IList<TimeTable> GetAllTimeTables(bool? active);
		TimeTable GetTimeTableById(int id);
		IList<TimeTable> SearchTimeTables(bool? active, int? acedemicyearid = null);
		#endregion
		#region Qualification
		void InsertQualification(Qualification qualification);
		void UpdateQualification(Qualification qualification);
		void DeleteQualification(int id);
		IList<Qualification> GetAllQualifications(bool? active);
		Qualification GetQualificationById(int id);
		IList<Qualification> GetQualificationByName(string name, bool? active);
		#endregion
		#region Allowance
		void InsertAllowance(Allowance allowance);
		void UpdateAllowance(Allowance allowance);
		void DeleteAllowance(int id);
		Allowance GetAllowanceById(int id);
		IList<Allowance> GetAllAllowances(bool? active);
		IList<Allowance> SearchAllowances(bool? active, string designation = null);
		#endregion
		#region Payment
		void InsertPayment(Payment payment);
		void UpdatePayment(Payment payment);
		void DeletePayment(int id);
		Payment GetPaymentById(int id);
		IList<Payment> GetAllPayments(bool? active);
		IList<Payment> SearchPayments(bool? active, int? allowanceid, string employee = null, int? acedemicyearid = null);
		#endregion
		#region Vendor
		void InsertVendor(Vendor vendor);
		void UpdateVendor(Vendor vendor);
		void DeleteVendor(int id);
		IList<Vendor> GetAllVendors(bool? active);
		Vendor GetVendorById(int id);
		IList<Vendor> GetVendorsByName(string name, bool? active);
		IList<Vendor> SearchVendors(bool? active, string religion = null, string designation = null, int? acedemicyearid = null);
		#endregion
		#region Purchase
		void InsertPurchase(Purchase purchase);
		void UpdatePurchase(Purchase purchase);
		void DeletePurchase(int id);
		IList<Purchase> GetAllPurchases(bool? active);
		Purchase GetPurchaseById(int id);
		IList<Purchase> SearchPurchases(bool? active, string product = null, string vendor = null, int? acedemicyearid = null);
		#endregion
		#region Fee Detail
		void InsertFeeDetail(FeeDetail feeDetail);
		void UpdateFeeDetail(FeeDetail feeDetail);
		void DeleteFeeDetail(int id);
		IList<FeeDetail> GetAllFeeDetails(bool? active);
		FeeDetail GetFeeDetailById(int id);
		IList<FeeDetail> SearchFeeDetails(bool? active, string student = null, string category = null, int? acedemicyearid = null);
		#endregion
		#region Message Group
		void InsertMessageGroup(MessageGroup messageGroup);
		void UpdateMessageGroup(MessageGroup messageGroup);
		void DeleteMessageGroup(int id);
		IList<MessageGroup> GetAllMessageGroups(bool? active);
		MessageGroup GetMessageGroupById(int id);
		IList<MessageGroup> GetMessageGroupsByName(string name, bool? active);
		#endregion
		#region Student Message Group
		void InsertStudent_MessageGroup(Student_MessageGroup studentMessageGroup);
		void UpdateStudent_MessageGroup(Student_MessageGroup studentMessageGroup);
		void DeleteStudent_MessageGroup(int id);
		IList<Student_MessageGroup> GetAllStudent_MessageGroups(bool? active);
		Student_MessageGroup GetStudent_MessageGroupById(int id);
		IList<Student_MessageGroup> GetStudent_MessageGroupsByStudentUsername(string username, bool? active);
		#endregion
		#region Message
		void InsertMessage(Message message);
		void UpdateMessage(Message message);
		void DeleteMessage(int id);
		IList<Message> GetAllMessages(bool? active);
		Message GetMessageById(int id);
		IList<Message> GetMessagesByMessageGroup(string messagegroup, bool? active);
		#endregion
		#region Exam
		void InsertExam(Exam exam);
		void UpdateExam(Exam exam);
		void DeleteExam(int id);
		IList<Exam> GetAllExams(bool? active);
		Exam GetExamById(int id);
		IList<Exam> GetExamByName(string name, bool? active);
		#endregion
		#region Acadmic Year
		void InsertAcadmicYear(AcadmicYear objAcadmicYear);
		void UpdateAcadmicYear(AcadmicYear objAcadmicYear);
		void DeleteAcadmicYear(int id);
		AcadmicYear GetActiveAcadmicYear();
		AcadmicYear GetAcadmicYearById(int id);
		AcadmicYear GetAcadmicYearByName(string name, bool? active = null);
		IList<AcadmicYear> GetAllAcadmicYears(bool? active);
		IList<Teacher> GetTeachersByIds(int[] userIds);
		void DeleteTeachers(IList<Teacher> users);
		void ToggleUser(int id);
		int GetTeacherCountByLoginDate(DateTime logindate);
        #endregion
      #region Reaction
        void InsertReaction(Reaction objReaction);
        void UpdateReaction(Reaction objReaction);
        Reaction GetReactionById(int id);
		IList<Reaction> SearchReactions(int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);
        IList<Reaction> GetReactionsByUser(int userid);
        void SaveLikeReaction(int userid, bool IsLike, bool IsDislike, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);
        void SaveMoodReaction(int userid, bool IsAngry, bool IsHappy, bool IsLOL, bool IsSad, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);

        void SaveRating(int userid, int rating, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);
		#endregion
		#region Question Type

		void InsertQuestionType(QuestionType questionType);
		void UpdateQuestionType(QuestionType questionType);
		QuestionType GetQuestionTypeById(int id);
		IList<QuestionType> GetAllQuestionTypes();
		#endregion
		#region Question & Assessments

		void InsertQuestion(Question question);
		void UpdateQuestion(Question question);
		void DeleteQuestion(int id);
		Question GetQuestionById(int id);

		IList<Question> SearchQuestions(int[] questionTypeIds = null, int[] subjectids = null,
			int? difficultylevel = null, bool? onlytimebound = null, bool? active = null);

		IList<Question> GetRandomQuestions(int count, int[] questionTypeIds = null, int[] subjectids = null,
			int? difficultylevel = null, bool? onlytimebound = null, bool? active = null);
		IList<AssessmentQuestion> GetQuestionsByAssessmentId(int assesmentid);
		void InsertOption(Option option);
		void UpdateOption(Option option);
		void DeleteOption(int id);
		IList<Option> GetOptionsByQuestionId(int questionid);
		void InsertAssessment(Assessment assessment);
		void UpdateAssessment(Assessment assessment);
		void DeleteAssessment(int id);

		IList<StudentAssessment> GetAssessmentByStudentId(int studentid, bool? active = null, bool? completed = null,
			DateTime? starttime = null, DateTime? endtime = null);

		IList<Assessment> SearchAssessments(int? difficultylevel = null, bool? onlytimebound = null,
			bool? active = null, DateTime? starttime = null, DateTime? endtime = null);

		Assessment GetAssessmentById(int id);
		StudentAssessment GetStudentAssessmentById(int id);
		void InsertstudentAssessment(StudentAssessment studentAssessment);
		void UpdatestudentAssessment(StudentAssessment studentAssessment);
		void DeletestudentAssessment(int id);

		#endregion
	}
}
