using System;
using System.Linq;
using EF.Core.Mapper;
using SMS.Models;
using EF.Core.Data;
using SMS.Areas.Admin.Models;

namespace SMS.Mappers
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
        
        #region Blog

        public static BlogModel ToModel(this Blog entity)
        {
            return entity.MapTo<Blog, BlogModel>();
        }

        public static Blog ToEntity(this BlogModel model)
        {
            return model.MapTo<BlogModel, Blog>();
        }

        public static Blog ToEntity(this BlogModel model, Blog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Acadmic Year

        public static AcadmicYearModel ToModel(this AcadmicYear entity)
        {
            return entity.MapTo<AcadmicYear, AcadmicYearModel>();
        }

        public static AcadmicYear ToEntity(this AcadmicYearModel model)
        {
            return model.MapTo<AcadmicYearModel, AcadmicYear>();
        }

        public static AcadmicYear ToEntity(this AcadmicYearModel model, AcadmicYear destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Allowance

        public static AllowanceModel ToModel(this Allowance entity)
        {
            return entity.MapTo<Allowance, AllowanceModel>();
        }

        public static Allowance ToEntity(this AllowanceModel model)
        {
            return model.MapTo<AllowanceModel, Allowance>();
        }

        public static Allowance ToEntity(this AllowanceModel model, Allowance destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Assessment

        public static AssessmentModel ToModel(this Assessment entity)
        {
            return entity.MapTo<Assessment, AssessmentModel>();
        }

        public static Assessment ToEntity(this AssessmentModel model)
        {
            return model.MapTo<AssessmentModel, Assessment>();
        }

        public static Assessment ToEntity(this AssessmentModel model, Assessment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Assessment Question

        public static AssessmentQuestionModel ToModel(this AssessmentQuestion entity)
        {
            return entity.MapTo<AssessmentQuestion, AssessmentQuestionModel>();
        }

        public static AssessmentQuestion ToEntity(this AssessmentQuestionModel model)
        {
            return model.MapTo<AssessmentQuestionModel, AssessmentQuestion>();
        }

        public static AssessmentQuestion ToEntity(this AssessmentQuestionModel model, AssessmentQuestion destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student Assessment

        public static StudentAssessmentModel ToModel(this StudentAssessment entity)
        {
            return entity.MapTo<StudentAssessment, StudentAssessmentModel>();
        }

        public static StudentAssessment ToEntity(this StudentAssessmentModel model)
        {
            return model.MapTo<StudentAssessmentModel, StudentAssessment>();
        }

        public static StudentAssessment ToEntity(this StudentAssessmentModel model, StudentAssessment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Blog Picture

        public static BlogPictureModel ToModel(this BlogPicture entity)
        {
            return entity.MapTo<BlogPicture, BlogPictureModel>();
        }

        public static BlogPicture ToEntity(this BlogPictureModel model)
        {
            return model.MapTo<BlogPictureModel, BlogPicture>();
        }

        public static BlogPicture ToEntity(this BlogPictureModel model, BlogPicture destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Book

        public static BookModel ToModel(this Book entity)
        {
            return entity.MapTo<Book, BookModel>();
        }

        public static Book ToEntity(this BookModel model)
        {
            return model.MapTo<BookModel, Book>();
        }

        public static Book ToEntity(this BookModel model, Book destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Book Issue

        public static BookIssueModel ToModel(this BookIssue entity)
        {
            return entity.MapTo<BookIssue, BookIssueModel>();
        }

        public static BookIssue ToEntity(this BookIssueModel model)
        {
            return model.MapTo<BookIssueModel, BookIssue>();
        }

        public static BookIssue ToEntity(this BookIssueModel model, BookIssue destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Caste

        public static CasteModel ToModel(this Caste entity)
        {
            return entity.MapTo<Caste, CasteModel>();
        }

        public static Caste ToEntity(this CasteModel model)
        {
            return model.MapTo<CasteModel, Caste>();
        }

        public static Caste ToEntity(this CasteModel model, Caste destination)
        {
            return model.MapTo(destination);
        }
        
        #endregion

        #region Category

        public static CategoryModel ToModel(this Category entity)
        {
            return entity.MapTo<Category, CategoryModel>();
        }

        public static Category ToEntity(this CategoryModel model)
        {
            return model.MapTo<CategoryModel, Category>();
        }

        public static Category ToEntity(this CategoryModel model, Category destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Class

        public static ClassModel ToModel(this Class entity)
        {
            return entity.MapTo<Class, ClassModel>();
        }

        public static Class ToEntity(this ClassModel model)
        {
            return model.MapTo<ClassModel, Class>();
        }

        public static Class ToEntity(this ClassModel model, Class destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Class Exam

        public static ClassExamModel ToModel(this ClassExam entity)
        {
            return entity.MapTo<ClassExam, ClassExamModel>();
        }

        public static ClassExam ToEntity(this ClassExamModel model)
        {
            return model.MapTo<ClassExamModel, ClassExam>();
        }

        public static ClassExam ToEntity(this ClassExamModel model, ClassExam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Class Homework

        public static ClassHomeworkModel ToModel(this ClassHomework entity)
        {
            return entity.MapTo<ClassHomework, ClassHomeworkModel>();
        }

        public static ClassHomework ToEntity(this ClassHomeworkModel model)
        {
            return model.MapTo<ClassHomeworkModel, ClassHomework>();
        }

        public static ClassHomework ToEntity(this ClassHomeworkModel model, ClassHomework destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Custom Page

        public static CustomPageModel ToModel(this CustomPage entity)
        {
            return entity.MapTo<CustomPage, CustomPageModel>();
        }

        public static CustomPage ToEntity(this CustomPageModel model)
        {
            return model.MapTo<CustomPageModel, CustomPage>();
        }

        public static CustomPage ToEntity(this CustomPageModel model, CustomPage destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region System Log

        public static SystemLogModel ToModel(this SystemLog entity)
        {
            return entity.MapTo<SystemLog, SystemLogModel>();
        }

        public static SystemLog ToEntity(this SystemLogModel model)
        {
            return model.MapTo<SystemLogModel, SystemLog>();
        }

        public static SystemLog ToEntity(this SystemLogModel model, SystemLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        
        #region Class Room

        public static ClassRoomModel ToModel(this ClassRoom entity)
        {
            return entity.MapTo<ClassRoom, ClassRoomModel>();
        }

        public static ClassRoom ToEntity(this ClassRoomModel model)
        {
            return model.MapTo<ClassRoomModel, ClassRoom>();
        }

        public static ClassRoom ToEntity(this ClassRoomModel model, ClassRoom destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Comment

        public static CommentModel ToModel(this Comment entity)
        {
            return entity.MapTo<Comment, CommentModel>();
        }

        public static Comment ToEntity(this CommentModel model)
        {
            return model.MapTo<CommentModel, Comment>();
        }

        public static Comment ToEntity(this CommentModel model, Comment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Data Token

        public static DataTokenModel ToModel(this DataToken entity)
        {
            return entity.MapTo<DataToken, DataTokenModel>();
        }

        public static DataToken ToEntity(this DataTokenModel model)
        {
            return model.MapTo<DataTokenModel, DataToken>();
        }

        public static DataToken ToEntity(this DataTokenModel model, DataToken destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Designation

        public static DesignationModel ToModel(this Designation entity)
        {
            return entity.MapTo<Designation, DesignationModel>();
        }

        public static Designation ToEntity(this DesignationModel model)
        {
            return model.MapTo<DesignationModel, Designation>();
        }

        public static Designation ToEntity(this DesignationModel model, Designation destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Division

        public static DivisionModel ToModel(this Division entity)
        {
            return entity.MapTo<Division, DivisionModel>();
        }

        public static Division ToEntity(this DivisionModel model)
        {
            return model.MapTo<DivisionModel, Division>();
        }

        public static Division ToEntity(this DivisionModel model, Division destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Division Exam

        public static DivisionExamModel ToModel(this DivisionExam entity)
        {
            return entity.MapTo<DivisionExam, DivisionExamModel>();
        }

        public static DivisionExam ToEntity(this DivisionExamModel model)
        {
            return model.MapTo<DivisionExamModel, DivisionExam>();
        }

        public static DivisionExam ToEntity(this DivisionExamModel model, DivisionExam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Division Homework

        public static DivisionHomeworkModel ToModel(this DivisionHomework entity)
        {
            return entity.MapTo<DivisionHomework, DivisionHomeworkModel>();
        }

        public static DivisionHomework ToEntity(this DivisionHomeworkModel model)
        {
            return model.MapTo<DivisionHomeworkModel, DivisionHomework>();
        }

        public static DivisionHomework ToEntity(this DivisionHomeworkModel model, DivisionHomework destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Division Subject

        public static DivisionSubjectModel ToModel(this DivisionSubject entity)
        {
            return entity.MapTo<DivisionSubject, DivisionSubjectModel>();
        }

        public static DivisionSubject ToEntity(this DivisionSubjectModel model)
        {
            return model.MapTo<DivisionSubjectModel, DivisionSubject>();
        }

        public static DivisionSubject ToEntity(this DivisionSubjectModel model, DivisionSubject destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Employee

        public static EmployeeModel ToModel(this Employee entity)
        {
            return entity.MapTo<Employee, EmployeeModel>();
        }

        public static Employee ToEntity(this EmployeeModel model)
        {
            return model.MapTo<EmployeeModel, Employee>();
        }

        public static Employee ToEntity(this EmployeeModel model, Employee destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Employee Attendance

        public static EmployeeAttendanceModel ToModel(this EmployeeAttendance entity)
        {
            return entity.MapTo<EmployeeAttendance, EmployeeAttendanceModel>();
        }

        public static EmployeeAttendance ToEntity(this EmployeeAttendanceModel model)
        {
            return model.MapTo<EmployeeAttendanceModel, EmployeeAttendance>();
        }

        public static EmployeeAttendance ToEntity(this EmployeeAttendanceModel model, EmployeeAttendance destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Event

        public static EventModel ToModel(this Event entity)
        {
            return entity.MapTo<Event, EventModel>();
        }

        public static Event ToEntity(this EventModel model)
        {
            return model.MapTo<EventModel, Event>();
        }

        public static Event ToEntity(this EventModel model, Event destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Event Picture

        public static EventPictureModel ToModel(this EventPicture entity)
        {
            return entity.MapTo<EventPicture, EventPictureModel>();
        }

        public static EventPicture ToEntity(this EventPictureModel model)
        {
            return model.MapTo<EventPictureModel, EventPicture>();
        }

        public static EventPicture ToEntity(this EventPictureModel model, EventPicture destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region News

        public static NewsModel ToModel(this News entity)
        {
            return entity.MapTo<News, NewsModel>();
        }

        public static News ToEntity(this NewsModel model)
        {
            return model.MapTo<NewsModel, News>();
        }

        public static News ToEntity(this NewsModel model, News destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region News Picture

        public static NewsPictureModel ToModel(this NewsPicture entity)
        {
            return entity.MapTo<NewsPicture, NewsPictureModel>();
        }

        public static NewsPicture ToEntity(this NewsPictureModel model)
        {
            return model.MapTo<NewsPictureModel, NewsPicture>();
        }

        public static NewsPicture ToEntity(this NewsPictureModel model, NewsPicture destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Exam
        public static ExamModel ToModel(this Exam entity)
        {
            return entity.MapTo<Exam, ExamModel>();
        }

        public static Exam ToEntity(this ExamModel model)
        {
            return model.MapTo<ExamModel, Exam>();
        }

        public static Exam ToEntity(this ExamModel model, Exam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Fee Category

        public static FeeCategoryModel ToModel(this FeeCategory entity)
        {
            return entity.MapTo<FeeCategory, FeeCategoryModel>();
        }

        public static FeeCategory ToEntity(this FeeCategoryModel model)
        {
            return model.MapTo<FeeCategoryModel, FeeCategory>();
        }

        public static FeeCategory ToEntity(this FeeCategoryModel model, FeeCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Feedback

        public static FeedbackModel ToModel(this Feedback entity)
        {
            return entity.MapTo<Feedback, FeedbackModel>();
        }

        public static Feedback ToEntity(this FeedbackModel model)
        {
            return model.MapTo<FeedbackModel, Feedback>();
        }

        public static Feedback ToEntity(this FeedbackModel model, Feedback destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Fee Detail

        public static FeeDetailModel ToModel(this FeeDetail entity)
        {
            return entity.MapTo<FeeDetail, FeeDetailModel>();
        }

        public static FeeDetail ToEntity(this FeeDetailModel model)
        {
            return model.MapTo<FeeDetailModel, FeeDetail>();
        }

        public static FeeDetail ToEntity(this FeeDetailModel model, FeeDetail destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region File

        public static FilesModel ToModel(this File entity)
        {
            return entity.MapTo<File, FilesModel>();
        }

        public static File ToEntity(this FilesModel model)
        {
            return model.MapTo<FilesModel, File>();
        }

        public static File ToEntity(this FilesModel model, File destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Holiday

        public static HolidayModel ToModel(this Holiday entity)
        {
            return entity.MapTo<Holiday, HolidayModel>();
        }

        public static Holiday ToEntity(this HolidayModel model)
        {
            return model.MapTo<HolidayModel, Holiday>();
        }

        public static Holiday ToEntity(this HolidayModel model, Holiday destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Homework

        public static HomeworkModel ToModel(this Homework entity)
        {
            return entity.MapTo<Homework, HomeworkModel>();
        }

        public static Homework ToEntity(this HomeworkModel model)
        {
            return model.MapTo<HomeworkModel, Homework>();
        }

        public static Homework ToEntity(this HomeworkModel model, Homework destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Message

        public static MessageModel ToModel(this Message entity)
        {
            return entity.MapTo<Message, MessageModel>();
        }

        public static Message ToEntity(this MessageModel model)
        {
            return model.MapTo<MessageModel, Message>();
        }

        public static Message ToEntity(this MessageModel model, Message destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Message Group

        public static MessageGroupModel ToModel(this MessageGroup entity)
        {
            return entity.MapTo<MessageGroup, MessageGroupModel>();
        }

        public static MessageGroup ToEntity(this MessageGroupModel model)
        {
            return model.MapTo<MessageGroupModel, MessageGroup>();
        }

        public static MessageGroup ToEntity(this MessageGroupModel model, MessageGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Option

        public static OptionModel ToModel(this Option entity)
        {
            return entity.MapTo<Option, OptionModel>();
        }

        public static Option ToEntity(this OptionModel model)
        {
            return model.MapTo<OptionModel, Option>();
        }

        public static Option ToEntity(this OptionModel model, Option destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Payment

        public static PaymentModel ToModel(this Payment entity)
        {
            return entity.MapTo<Payment, PaymentModel>();
        }

        public static Payment ToEntity(this PaymentModel model)
        {
            return model.MapTo<PaymentModel, Payment>();
        }

        public static Payment ToEntity(this PaymentModel model, Payment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Permission Record

        public static PermissionRecordModel ToModel(this PermissionRecord entity)
        {
            return entity.MapTo<PermissionRecord, PermissionRecordModel>();
        }

        public static PermissionRecord ToEntity(this PermissionRecordModel model)
        {
            return model.MapTo<PermissionRecordModel, PermissionRecord>();
        }

        public static PermissionRecord ToEntity(this PermissionRecordModel model, PermissionRecord destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Picture

        public static PictureModel ToModel(this Picture entity)
        {
            return entity.MapTo<Picture, PictureModel>();
        }

        public static Picture ToEntity(this PictureModel model)
        {
            return model.MapTo<PictureModel, Picture>();
        }

        public static Picture ToEntity(this PictureModel model, Picture destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Product Category

        public static ProductCategoryModel ToModel(this ProductCategory entity)
        {
            return entity.MapTo<ProductCategory, ProductCategoryModel>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model)
        {
            return model.MapTo<ProductCategoryModel, ProductCategory>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model, ProductCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Product

        public static ProductModel ToModel(this Product entity)
        {
            return entity.MapTo<Product, ProductModel>();
        }

        public static Product ToEntity(this ProductModel model)
        {
            return model.MapTo<ProductModel, Product>();
        }

        public static Product ToEntity(this ProductModel model, Product destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Product Picture

        public static ProductPictureModel ToModel(this ProductPicture entity)
        {
            return entity.MapTo<ProductPicture, ProductPictureModel>();
        }

        public static ProductPicture ToEntity(this ProductPictureModel model)
        {
            return model.MapTo<ProductPictureModel, ProductPicture>();
        }

        public static ProductPicture ToEntity(this ProductPictureModel model, ProductPicture destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Purchase

        public static PurchaseModel ToModel(this Purchase entity)
        {
            return entity.MapTo<Purchase, PurchaseModel>();
        }

        public static Purchase ToEntity(this PurchaseModel model)
        {
            return model.MapTo<PurchaseModel, Purchase>();
        }

        public static Purchase ToEntity(this PurchaseModel model, Purchase destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Qualification

        public static QualificationModel ToModel(this Qualification entity)
        {
            return entity.MapTo<Qualification, QualificationModel>();
        }

        public static Qualification ToEntity(this QualificationModel model)
        {
            return model.MapTo<QualificationModel, Qualification>();
        }

        public static Qualification ToEntity(this QualificationModel model, Qualification destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Question

        public static QuestionModel ToModel(this Question entity)
        {
            return entity.MapTo<Question, QuestionModel>();
        }

        public static Question ToEntity(this QuestionModel model)
        {
            return model.MapTo<QuestionModel, Question>();
        }

        public static Question ToEntity(this QuestionModel model, Question destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Question type

        public static QuestionTypeModel ToModel(this QuestionType entity)
        {
            return entity.MapTo<QuestionType, QuestionTypeModel>();
        }

        public static QuestionType ToEntity(this QuestionTypeModel model)
        {
            return model.MapTo<QuestionTypeModel, QuestionType>();
        }

        public static QuestionType ToEntity(this QuestionTypeModel model, QuestionType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Reaction

        public static ReactionModel ToModel(this Reaction entity)
        {
            return entity.MapTo<Reaction, ReactionModel>();
        }

        public static Reaction ToEntity(this ReactionModel model)
        {
            return model.MapTo<ReactionModel, Reaction>();
        }

        public static Reaction ToEntity(this ReactionModel model, Reaction destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Religion

        public static ReligionModel ToModel(this Religion entity)
        {
            return entity.MapTo<Religion, ReligionModel>();
        }

        public static Religion ToEntity(this ReligionModel model)
        {
            return model.MapTo<ReligionModel, Religion>();
        }

        public static Religion ToEntity(this ReligionModel model, Religion destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Reply

        public static ReplyModel ToModel(this Reply entity)
        {
            return entity.MapTo<Reply, ReplyModel>();
        }

        public static Reply ToEntity(this ReplyModel model)
        {
            return model.MapTo<ReplyModel, Reply>();
        }

        public static Reply ToEntity(this ReplyModel model, Reply destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region School

        public static SchoolModel ToModel(this School entity)
        {
            return entity.MapTo<School, SchoolModel>();
        }

        public static School ToEntity(this SchoolModel model)
        {
            return model.MapTo<SchoolModel, School>();
        }

        public static School ToEntity(this SchoolModel model, School destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student

        public static StudentModel ToModel(this Student entity)
        {
            return entity.MapTo<Student, StudentModel>();
        }

        public static Student ToEntity(this StudentModel model)
        {
            return model.MapTo<StudentModel, Student>();
        }

        public static Student ToEntity(this StudentModel model, Student destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student Message Group

        public static Student_MessageGroupModel ToModel(this Student_MessageGroup entity)
        {
            return entity.MapTo<Student_MessageGroup, Student_MessageGroupModel>();
        }

        public static Student_MessageGroup ToEntity(this Student_MessageGroupModel model)
        {
            return model.MapTo<Student_MessageGroupModel, Student_MessageGroup>();
        }

        public static Student_MessageGroup ToEntity(this Student_MessageGroupModel model, Student_MessageGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student Attendance

        public static StudentAttendanceModel ToModel(this StudentAttendance entity)
        {
            return entity.MapTo<StudentAttendance, StudentAttendanceModel>();
        }

        public static StudentAttendance ToEntity(this StudentAttendanceModel model)
        {
            return model.MapTo<StudentAttendanceModel, StudentAttendance>();
        }

        public static StudentAttendance ToEntity(this StudentAttendanceModel model, StudentAttendance destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student Exam

        public static StudentExamModel ToModel(this StudentExam entity)
        {
            return entity.MapTo<StudentExam, StudentExamModel>();
        }

        public static StudentExam ToEntity(this StudentExamModel model)
        {
            return model.MapTo<StudentExamModel, StudentExam>();
        }

        public static StudentExam ToEntity(this StudentExamModel model, StudentExam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Student Homework

        public static StudentHomeworkModel ToModel(this StudentHomework entity)
        {
            return entity.MapTo<StudentHomework, StudentHomeworkModel>();
        }

        public static StudentHomework ToEntity(this StudentHomeworkModel model)
        {
            return model.MapTo<StudentHomeworkModel, StudentHomework>();
        }

        public static StudentHomework ToEntity(this StudentHomeworkModel model, StudentHomework destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Subject

        public static SubjectModel ToModel(this Subject entity)
        {
            return entity.MapTo<Subject, SubjectModel>();
        }

        public static Subject ToEntity(this SubjectModel model)
        {
            return model.MapTo<SubjectModel, Subject>();
        }

        public static Subject ToEntity(this SubjectModel model, Subject destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Subject Exam

        public static SubjectExamModel ToModel(this SubjectExam entity)
        {
            return entity.MapTo<SubjectExam, SubjectExamModel>();
        }

        public static SubjectExam ToEntity(this SubjectExamModel model)
        {
            return model.MapTo<SubjectExamModel, SubjectExam>();
        }

        public static SubjectExam ToEntity(this SubjectExamModel model, SubjectExam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Teacher

        public static TeacherModel ToModel(this Teacher entity)
        {
            return entity.MapTo<Teacher, TeacherModel>();
        }

        public static Teacher ToEntity(this TeacherModel model)
        {
            return model.MapTo<TeacherModel, Teacher>();
        }

        public static Teacher ToEntity(this TeacherModel model, Teacher destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Teacher Exam

        public static TeacherExamModel ToModel(this TeacherExam entity)
        {
            return entity.MapTo<TeacherExam, TeacherExamModel>();
        }

        public static TeacherExam ToEntity(this TeacherExamModel model)
        {
            return model.MapTo<TeacherExamModel, TeacherExam>();
        }

        public static TeacherExam ToEntity(this TeacherExamModel model, TeacherExam destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Template

        public static TemplateModel ToModel(this Template entity)
        {
            return entity.MapTo<Template, TemplateModel>();
        }

        public static Template ToEntity(this TemplateModel model)
        {
            return model.MapTo<TemplateModel, Template>();
        }

        public static Template ToEntity(this TemplateModel model, Template destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Time table

        public static TimeTableModel ToModel(this TimeTable entity)
        {
            return entity.MapTo<TimeTable, TimeTableModel>();
        }

        public static TimeTable ToEntity(this TimeTableModel model)
        {
            return model.MapTo<TimeTableModel, TimeTable>();
        }

        public static TimeTable ToEntity(this TimeTableModel model, TimeTable destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Time Table Setting

        public static TimeTableSettingModel ToModel(this TimeTableSetting entity)
        {
            return entity.MapTo<TimeTableSetting, TimeTableSettingModel>();
        }

        public static TimeTableSetting ToEntity(this TimeTableSettingModel model)
        {
            return model.MapTo<TimeTableSettingModel, TimeTableSetting>();
        }

        public static TimeTableSetting ToEntity(this TimeTableSettingModel model, TimeTableSetting destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region User

        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        public static User ToEntity(this UserModel model)
        {
            return model.MapTo<UserModel, User>();
        }

        public static User ToEntity(this UserModel model, User destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region User Role

        public static RoleModel ToModel(this UserRole entity)
        {
            return entity.MapTo<UserRole, RoleModel>();
        }

        public static UserRole ToEntity(this RoleModel model)
        {
            return model.MapTo<RoleModel, UserRole>();
        }

        public static UserRole ToEntity(this RoleModel model, UserRole destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Vendor

        public static VendorModel ToModel(this Vendor entity)
        {
            return entity.MapTo<Vendor, VendorModel>();
        }

        public static Vendor ToEntity(this VendorModel model)
        {
            return model.MapTo<VendorModel, Vendor>();
        }

        public static Vendor ToEntity(this VendorModel model, Vendor destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Video

        public static VideoModel ToModel(this Video entity)
        {
            return entity.MapTo<Video, VideoModel>();
        }

        public static Video ToEntity(this VideoModel model)
        {
            return model.MapTo<VideoModel, Video>();
        }

        public static Video ToEntity(this VideoModel model, Video destination)
        {
            return model.MapTo(destination);
        }

        #endregion

    }
}