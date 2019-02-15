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
        IList<School> GetAllSchools(bool? onlyActive = null);
        School GetSchoolById(int id);
        #endregion

        #region Student
        void InsertStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        IList<Student> GetAllStudents(bool? onlyActive = null);
        Student GetStudentById(int id);
        IList<Student> GetStudentsByName(string name, bool? active);
        IList<Student> SearchStudents(bool? onlyActive = null, int? classid = null, int? acedemicyearid = null);
        void ToggleActiveStatusStudent(int id);
        bool CheckStudentExists(string name, int? id = null);
        Student GetStudentByImpersonateId(int id);
        bool CheckUsernameExistsForStudent(string username);
        IList<Student> GetStudentsByDivision(int id);
        Student GetStudentByImpersonatedUser(int userid);
        IList<StudentExam> GetAllStudentExamMappings();
        #endregion

        #region Student Exam

        void InsertStudentExam(StudentExam StudentExam);
        void UpdateStudentExam(StudentExam StudentExam);
        void DeleteStudentExam(int id);
        StudentExam GetStudentExamMappingById(int id);
        IList<StudentExam> GetAllExamsByStudent(int id);

        #endregion

        #region Employee
        void InsertEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        IList<Employee> GetAllEmployees(bool? onlyActive = null);
        Employee GetEmployeeById(int id);
        void ToggleActiveStatusEmployee(int id);
        bool CheckEmployeeExists(string name, int? id = null);
        bool CheckUsernameExistsForEmployee(string username);

        bool IsEmployeeAlreadyAssignedToTeacher(int id);
        #endregion

        #region Student Attendance
        void InsertStudentAttendance(StudentAttendance studentattendance);
        void UpdateStudentAttendance(StudentAttendance studentattendance);
        void DeleteStudentAttendance(int id);
        IList<StudentAttendance> GetStudentAttendanceByDate(int DD, int MM, int yyyy);
        StudentAttendance GetStudentAttendanceById(int id);
        IList<StudentAttendance> SearchStudentAttendances(bool? active, string studentusername = null, string classname = null, int? acedemicyearid = null);
        #endregion

        #region Religion
        void InsertReligion(Religion religion);
        void UpdateReligion(Religion religion);
        void DeleteReligion(int id);
        IList<Religion> GetAllReligions();
        Religion GetReligionById(int id);
        IList<Religion> GetReligionByName(string name);
        bool CheckReligionExists(string name, int? id = null);
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
        IList<FeeCategory> GetAllFeeCategories(bool? onlyActive = null);
        FeeCategory GetFeeCategoryById(int id);
        IList<FeeCategory> GetFeeCategoryByName(string name, bool? active);
        IList<FeeCategory> SearchFeeCategories(bool? active, string category = null, string classname = null, int? acadmicyearid = null);
        FeeCategory GetFeeCategoryByClassAndCategory(int classid, int? categoryid = null);
        void ToggleActiveStatusFeeCategory(int id);
        bool CheckFeeCategoryExists(int catid, int classdivid, int acadmicyearid, int? id = null);
        #endregion

        #region Caste
        void InsertCaste(Caste caste);
        void UpdateCaste(Caste caste);
        void DeleteCaste(int id);
        IList<Caste> GetAllCastes(bool? onlyActive = null);
        IList<Caste> GetAllCastesByCategory(int categoryid);
        void RemoveCasteFromCategory(int categoryid, int casteid);
        Caste GetCasteById(int id);
        IList<Caste> GetCasteByName(string name, bool? active);
        IList<Caste> SearchFeeCastes(bool? active, string religion = null, int? acedemicyearid = null);
        bool CheckCasteExists(string name, int? id = null);
        void ToggleActiveStatusCaste(int id);
        #endregion

        #region Category
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        IList<Category> GetAllCategories(bool? onlyActive = null);
        Category GetCategoryById(int id);
        IList<Category> GetCategoryByName(string name, bool? active);
        void ToggleActiveStatusCategory(int id);

        bool CheckCategoryExists(string name, int? id = null);
        #endregion

        #region Product Category
        void InsertProductCategory(ProductCategory category);
        void UpdateProductCategory(ProductCategory category);
        void DeleteProductCategory(int id);
        IList<ProductCategory> GetAllProductCategories(bool? onlyActive = null);
        ProductCategory GetProductCategoryById(int id);
        IList<ProductCategory> GetProductCategoryByName(string name, bool? active);
        void ToggleActiveStatusProductCategory(int id);
        IList<Product> GetAllProductsByProductCategory(int productCategoryId, bool? onlyActive = null);
        bool CheckProductCategoryExists(string name, int? id = null);
        void ToggleMenuStatusProductCategory(int id);
        IList<ProductCategoryMapping> GetCategoryProductMappings(int? categoryid = null, int? productid = null);
        void InsertProductCategoryMapping(ProductCategoryMapping productCategoryMapping);
        void UpdateProductCategoryMapping(ProductCategoryMapping productCategoryMapping);
        void DeleteProductCategoryMapping(int id);
        void RemoveProductFromCategory(int productcategoryid, int productid);
        #endregion

        #region Class
        void InsertClass(Class objClass);
        void UpdateClass(Class objClass);
        void DeleteClass(int id);
        Class GetClassById(int id);
        IList<Class> GetClassByName(string name);
        bool CheckClassExists(string name, int? id = null);
        void ToggleActiveStatusClass(int id);
        IList<Class> GetAllClasses(bool? onlyActive = null);
        IList<ClassRoomDivision> GetAllDivisionsByClass(int? id, bool? onlyActive = null);
        #endregion

        #region Class Division
        void InsertClassDivision(ClassRoomDivision division);
        void UpdateClassDivision(ClassRoomDivision division);
        void DeleteClassDivision(int id);
        IList<ClassRoomDivision> GetDivisionsByClass(int id);
        IList<ClassRoomDivision> GetClassDivisions(int? classid = null, int? divisionid = null, int? classroomid = null);
        bool CheckClassRoomAlreadyAssociatedToOtherDivisionAndClass(int classroomid, int? classid = null, int? divisionid = null);
        void RemoveDivisionFromClass(int classid, int divisionid);
        IList<ClassRoomDivision> GetAllClassRoomDivisions(bool? onlyActive = null);
        IList<ClassRoomDivision> GetAllClassDivisionsByTeacher(int id);
        ClassRoomDivision GetClassRoomDivisionById(int id);

        #endregion

        #region Class Room
        void InsertClassRoom(ClassRoom classroom);
        void UpdateClassRoom(ClassRoom classroom);
        void DeleteClassRoom(int id);

        ClassRoom GetClassRoomById(int id);

        bool CheckClassRoomExists(string roomnumber, int? id = null);
        ClassRoomDivision GetClassDivisionByClassRoom(int id);
        bool CheckClassRoomExistsForAnotherClassDivision(int? id, string roomnumber);
        void RemoveDivisionFromClassRoom(int classroomid, int divisionid);
        void RemoveClassFromClassRoom(int classroomid, int classid);
        IList<ClassRoom> GetAllClassRooms(bool? onlyActive = null);

        void ToggleActiveStatusClassRoom(int id);
        #endregion

        #region Homework

        void InsertHomework(Homework objHomework);
        void UpdateHomework(Homework objHomework);
        void DeleteHomework(int id);
        Homework GetHomeworkById(int id);
        IList<DivisionHomework> GetAllHomeworksByDivision(int id);
        IList<Homework> GetAllHomeworks(bool? onlyActive = null);
        bool CheckHomeworkExists(string name, int? id = null);
        void ToggleActiveStatusHomework(int id);

        #endregion

        #region Division
        void InsertDivision(Division division);
        void UpdateDivision(Division division);
        void DeleteDivision(int id);
        IList<Division> GetAllDivisions(bool? onlyActive = null);
        Division GetDivisionById(int id);
        IList<Division> GetDivisionsByName(string name, bool? active);
        bool CheckDivisionExists(string name, int? id = null);
        void ToggleActiveStatusDivision(int id);
        #endregion

        #region Subject
        void InsertSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteSubject(int id);
        Subject GetSubjectById(int id);
        IList<Subject> GetSubjectByName(string name, bool? active);
        IList<Subject> GetAllSubjects(bool? onlyActive = null);
        bool CheckSubjectExists(string name, int? id = null);
        void ToggleActiveStatusSubject(int id);
        bool CheckCodeExistsForSubject(string code);
        IList<Subject> GetAllSubjectsByTeacher(int id);
        IList<DivisionSubject> GetAllSubjectsByDivision(int id);
        #endregion

        #region Division Subject
        void InsertDivisionSubject(DivisionSubject divisionClassSubject);
        void UpdateDivisionSubject(DivisionSubject divisionClassSubject);
        void DeleteDivisionSubject(int id);
        IList<DivisionSubject> GetAllDivisionSubjectMappings();
        DivisionSubject GetDivisionSubjectMappingById(int id);
        IList<DivisionSubject> GetDivisionSubjects(int? divisionid = null, int? subjectid = null);

        void RemoveSubjectFromDivision(int divisionid, int subjectid);
        void RemoveHomeworkFromDivision(int divisionid, int homeworkid);
        IList<DivisionHomework> GetDivisionHomeworks(int? divisionid = null, int? homeworkid = null);
        void RemoveExamFromDivision(int divisionid, int examid);
        IList<DivisionExam> GetDivisionExams(int? divisionid = null, int? examid = null);
        #endregion

        #region Division Homework
        void InsertDivisionHomework(DivisionHomework divisionHomework);
        void UpdateDivisionHomework(DivisionHomework divisionHomework);
        void DeleteDivisionHomework(int id);
        IList<DivisionHomework> GetAllDivisionHomeworkMappings();
        DivisionHomework GetDivisionHomeworkMappingById(int id);
        #endregion

        #region Division Exam
        void InsertDivisionExam(DivisionExam divisionExam);
        void UpdateDivisionExam(DivisionExam divisionExam);
        void DeleteDivisionExam(int id);
        IList<DivisionExam> GetAllDivisionExamMappings();
        DivisionExam GetDivisionExamMappingById(int id);
        IList<DivisionExam> GetAllExamsByDivision(int id);
        #endregion

        #region Designation
        void InsertDesignation(Designation designation);
        void UpdateDesignation(Designation designation);
        void DeleteDesignation(int id);
        Designation GetDesignationById(int id);
        bool CheckDesignationExists(string name, int? id = null);
        IList<Designation> GetAllDesignations(bool? onlyActive = null);

        IList<Designation> GetDesignationByName(string name, bool? active);

        void ToggleActiveStatusDesignation(int id);
        #endregion

        #region Teacher
        void InsertTeacher(Teacher teacher);
        void UpdateTeacher(Teacher teacher);
        void DeleteTeacher(int id);
        IList<Teacher> GetAllTeachers(bool? onlyActive = null);
        Teacher GetTeacherById(int id);
        IList<Teacher> GetTeachersByName(string name, bool? active);
        IList<Teacher> SearchTeachers(bool? active, string subject = null, string qualification = null, string employee = null, int? acedemicyearid = null);
        IList<Teacher> SearchTeachers(bool? active, int subjectid = 0, int qualificationid = 0, int employeeid = 0, int? acedemicyearid = null);
        IList<Teacher> GetTeachersByIds(int[] teacherids);
        void DeleteTeachers(IList<Teacher> teachers);
        void ToggleTeacher(int id);
        int GetTeacherCountByLoginDate(DateTime logindate);
        bool CheckTeacherExists(string name, int? id = null);
        void ToggleActiveStatusTeacher(int id);
        int GetTotalTeachers();
        Teacher GetTeacherByImpersonateId(int id);
        IList<TeacherExam> GetAllExamsByTeacher(int id);
        IList<TeacherExam> GetAllTeacherExamMappings();
        Teacher GetTeacherByImpersonatedUser(int userid);
        #endregion

        #region Teacher Exam

        void InsertTeacherExam(TeacherExam teacherExam);
        void UpdateTeacherExam(TeacherExam teacherExam);
        void DeleteTeacherExam(int id);
        TeacherExam GetTeacherExamMappingById(int id);

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
        void ToggleActiveStatusTimetable(int id);
        #endregion

        #region Qualification
        void InsertQualification(Qualification qualification);
        void UpdateQualification(Qualification qualification);
        void DeleteQualification(int id);
        IList<Qualification> GetAllQualifications();
        Qualification GetQualificationById(int id);
        IList<Qualification> GetQualificationByName(string name);

        bool CheckQualificationExists(string name, int? id = null);
        #endregion

        #region Allowance
        void InsertAllowance(Allowance allowance);
        void UpdateAllowance(Allowance allowance);
        void DeleteAllowance(int id);
        Allowance GetAllowanceById(int id);
        IList<Allowance> GetAllAllowances();
        Allowance GetAllowanceByDesignation(int designationid);
        bool CheckAllowanceExistsForDesignation(int designationid, int? id = null);
        #endregion

        #region Payment
        void InsertPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int id);
        Payment GetPaymentById(int id);
        IList<Payment> GetAllPayments();
        IList<Payment> SearchPayments(int? employeeid = null, int? acedemicyearid = null);
        #endregion

        #region Vendor
        void InsertVendor(Vendor vendor);
        void UpdateVendor(Vendor vendor);
        void DeleteVendor(int id);
        IList<Vendor> GetAllVendors(bool? active = null);
        Vendor GetVendorById(int id);
        Vendor GetVendorsByName(string name);
        IList<Vendor> SearchVendors(bool? active, int? acedemicyearid = null);
        void ToggleActiveStatusVendor(int id);
        #endregion

        #region Purchase
        void InsertPurchase(Purchase purchase);
        void UpdatePurchase(Purchase purchase);
        void DeletePurchase(int id);
        IList<Purchase> GetAllPurchases();
        Purchase GetPurchaseById(int id);
        IList<Purchase> SearchPurchases(int? productid = null, int? vendorid = null, int? acedemicyearid = null);
        #endregion

        #region Fee Detail
        void InsertFeeDetail(FeeDetail feeDetail);
        void UpdateFeeDetail(FeeDetail feeDetail);
        void DeleteFeeDetail(int id);
        IList<FeeDetail> GetAllFeeDetails();
        FeeDetail GetFeeDetailById(int id);
        IList<FeeDetail> SearchFeeDetails(int? studentid = null, int? categoryid = null, int? cashierid = null, int? acedemicyearid = null);
        #endregion

        #region Message Group
        void InsertMessageGroup(MessageGroup messageGroup);
        void UpdateMessageGroup(MessageGroup messageGroup);
        void DeleteMessageGroup(int id);
        IList<MessageGroup> GetAllMessageGroups();
        MessageGroup GetMessageGroupById(int id);
        MessageGroup GetMessageGroupByName(string name);
        #endregion

        #region Student Message Group
        void InsertStudent_MessageGroup(Student_MessageGroup studentMessageGroup);
        void UpdateStudent_MessageGroup(Student_MessageGroup studentMessageGroup);
        void DeleteStudent_MessageGroup(int id);
        IList<Student_MessageGroup> GetAllStudent_MessageGroups();
        Student_MessageGroup GetStudent_MessageGroupById(int id);
        IList<Student_MessageGroup> GetStudent_MessageGroupsByStudent(int studentid);
        #endregion

        #region Message
        void InsertMessage(Message message);
        void UpdateMessage(Message message);
        void DeleteMessage(int id);
        IList<Message> GetAllMessages(bool? active);
        Message GetMessageById(int id);
        IList<Message> GetMessagesByMessageGroup(int messagegroupid, bool? active);
        #endregion

        #region Exam
        void InsertExam(Exam exam);
        void UpdateExam(Exam exam);
        void DeleteExam(int id);
        IList<Exam> GetAllExams(bool? onlyActive = null);
        Exam GetExamById(int id);
        IList<Exam> GetExamByName(string name, bool? active);
        bool CheckExamExists(string name, int? id = null);
        void ToggleActiveStatusExam(int id);
        IList<DivisionExam> GetAllExamsByClassDivision(int id);
        IList<ClassRoom> GetVacantClassRoomsForExams();
        #endregion

        #region Acadmic Year
        void InsertAcadmicYear(AcadmicYear objAcadmicYear);
        void UpdateAcadmicYear(AcadmicYear objAcadmicYear);
        void DeleteAcadmicYear(int id);
        AcadmicYear GetActiveAcadmicYear();
        AcadmicYear GetAcadmicYearById(int id);
        AcadmicYear GetAcadmicYearByName(string name, bool? active = null);
        IList<AcadmicYear> GetAllAcadmicYears(bool? onlyActive = null);
        bool CheckAcadmicYearExists(string name, int? id = null);
        void ToggleActiveStatusAcadmicYear(int id);
        IList<Holiday> GetAllHolidaysByAcadmicYear(int id, bool? onlyActive = null);
        void DeactivateAllAcadmicYears();
        #endregion

        #region Book
        void InsertBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
        IList<Book> GetAllBooks(bool? onlyActive = null);
        Book GetBookById(int id);
        IList<Book> GetBookByName(string name, bool? active);
        bool CheckBookExists(string name, int? id = null);
        void ToggleActiveStatusBook(int id);
        #endregion

        #region Book Issue
        void InsertBookIssue(BookIssue bookIssue);
        void UpdateBookIssue(BookIssue bookIssue);
        void DeleteBookIssue(int id);
        IList<BookIssue> GetAllBookIssueIssues();
        BookIssue GetBookIssueById(int id);
        IList<BookIssue> GetBookIssueByStudent(int studentid);
        bool CheckBookIssueExists(int studentid, int bookid, int? id = null);
        #endregion

        #region Reaction
        void InsertReaction(Reaction objReaction);
        void UpdateReaction(Reaction objReaction);
        Reaction GetReactionById(int id);
        IList<Reaction> SearchReactions(int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null, int? userid = null);
        IList<Reaction> GetReactionsByUser(int userid);
        void SaveLikeReaction(int userid, bool IsLike, bool IsDislike, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);
        void SaveMoodReaction(int userid, bool IsAngry, bool IsHappy, bool IsLOL, bool IsSad, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);

        void SaveRating(int userid, int rating, int? blogid = null, int? productid = null, int? eventid = null, int? pictureid = null, int? videoid = null, int? newsid = null, int? commentid = null, int? replyid = null);
        #endregion

        #region Question Type
        void InsertQuestionType(QuestionType questionType);
        void UpdateQuestionType(QuestionType questionType);
        void DeleteQuestionType(int id);

        QuestionType GetQuestionTypeById(int id);
        IList<QuestionType> GetAllQuestionTypes(bool? onlyActive = null);
        void ToggleActiveStatusQuestionType(int id);
        bool CheckQuestionTypeExists(string name, int? id = null);
        #endregion

        #region Assessments
        void InsertQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(int id);
        IList<Question> GetAllQuestions(bool? onlyActive = null);
        Question GetQuestionById(int id);
        IList<Question> SearchQuestions(int[] questionTypeIds = null, int[] subjectids = null,
            int? difficultylevel = null, bool? onlytimebound = null, bool? active = null);

        IList<Question> GetRandomQuestions(int count, int[] questionTypeIds = null, int[] subjectids = null,
            int? difficultylevel = null, bool? onlytimebound = null, bool? active = null);
        IList<AssessmentQuestion> GetQuestionsByAssessmentId(int assesmentid);
        IList<AssessmentStudent> GetStudentsByAssessmentId(int assesmentid);

        void InsertOption(Option option);
        void UpdateOption(Option option);
        void DeleteOption(int id);
        IList<Option> GetOptionsByQuestionId(int questionid);
        void InsertAssessment(Assessment assessment);
        void UpdateAssessment(Assessment assessment);
        void DeleteAssessment(int id);

        IList<AssessmentStudent> GetAssessmentByStudentId(int studentid, bool? active = null, bool? completed = null,
            DateTime? starttime = null, DateTime? endtime = null);

        IList<Assessment> SearchAssessments(int? difficultylevel = null, bool? onlytimebound = null,
            bool? active = null, DateTime? starttime = null, DateTime? endtime = null);

        Assessment GetAssessmentById(int id);
        AssessmentStudent GetStudentAssessmentById(int id);
        void InsertStudentAssessment(AssessmentStudent studentAssessment);
        void UpdateStudentAssessment(AssessmentStudent studentAssessment);
        void DeleteStudentAssessment(int id);
        IList<Assessment> GetAllAssessments(bool? onlyActive = null);
        bool CheckAssessmentExists(string name, int? id = null);
        void ToggleActiveStatusAssessment(int id);
        IList<AssessmentStudent> GetAllAssessmentsByStudent(int id);
        AssessmentQuestion GetAssessmentQuestionById(int id);
        AssessmentQuestion GetAssessmentQuestion(int assessmentid, int questionid);
        void InsertAssessmentQuestion(AssessmentQuestion assessmentQuestion);
        void UpdateAssessmentQuestion(AssessmentQuestion assessmentQuestion);
        void DeleteAssessmentQuestion(int id);
        #endregion

        #region Global Settings

        Settings GetSettingByKey(string key, int userid = 0);

        #endregion

        #region House
        void InsertHouse(House house);
        void UpdateHouse(House house);
        void DeleteHouse(int id);
        IList<House> GetAllHouses(bool? onlyActive = null);
        House GetHouseById(int id);
        IList<House> GetHouseByName(string name, bool? active);
        bool CheckHouseExists(string name, int? id = null);
        void ToggleActiveStatusHouse(int id);
        #endregion

        #region Holiday
        void InsertHoliday(Holiday objHoliday);
        void UpdateHoliday(Holiday objHoliday);
        void DeleteHoliday(int id);
        Holiday GetHolidayById(int id);
        IList<Holiday> GetAllHolidays(bool? onlyActive = null);
        bool CheckHolidayExists(string name, int? id = null);
        void ToggleActiveStatusHoliday(int id);
        #endregion

        #region Question

        bool CheckQuestionExists(string name, int? id = null);
        void ToggleActiveStatusQuestion(int id);
        Option GetOptionById(int id);

        #endregion
    }
}
