﻿using AutoMapper;
using EF.Core.Data;
using SMS.Models;
using EF.Services.Http;
using SMS.Areas.Admin.Models;

namespace SMS.Areas.Admin.Mappers
{
    public static class AdminMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        public static void Init()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogModel>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.postCommentModel, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());
                cfg.CreateMap<BlogModel, Blog>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<StudentAssessment, StudentAssessmentModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore());
                cfg.CreateMap<StudentAssessmentModel, StudentAssessment>()
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.Assessment, mo => mo.Ignore());

                cfg.CreateMap<BlogPicture, BlogPictureModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<BlogPictureModel, BlogPicture>()
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Blog, mo => mo.Ignore());

                cfg.CreateMap<BookIssue, BookIssueModel>()
                    .ForMember(dest => dest.Book, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());
                cfg.CreateMap<BookIssueModel, BookIssue>()
                    .ForMember(dest => dest.Book, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());

                cfg.CreateMap<Caste, CasteModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.AvailableCategories, mo => mo.Ignore());
                cfg.CreateMap<CasteModel, Caste>()
                    .ForMember(dest => dest.Categories, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore());

                cfg.CreateMap<Category, CategoryModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")));
                cfg.CreateMap<CategoryModel, Category>()
                    .ForMember(dest => dest.Castes, mo => mo.Ignore());

                cfg.CreateMap<ClassExam, ClassExamModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore());
                cfg.CreateMap<ClassExamModel, ClassExam>()
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore());

                cfg.CreateMap<Class, ClassModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")));
                cfg.CreateMap<ClassModel, Class>();

                cfg.CreateMap<ClassRoom, ClassRoomModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")));
                cfg.CreateMap<ClassRoomModel, ClassRoom>();

                cfg.CreateMap<ClassRoomDivision, ClassRoomDivisionModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")));
                cfg.CreateMap<ClassRoomDivisionModel, ClassRoomDivision>()
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<ClassHomework, ClassHomeworkModel>()
                    .ForMember(dest => dest.AvailableClasses, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.AvailableHomeworks, mo => mo.Ignore());
                cfg.CreateMap<ClassHomeworkModel, ClassHomework>()
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());

                cfg.CreateMap<CustomPage, CustomPageModel>()
                    .ForMember(dest => dest.AvailableTemplates, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.template, mo => mo.Ignore());
                cfg.CreateMap<CustomPageModel, CustomPage>()
                    .ForMember(dest => dest.PermissionRecord, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Template, mo => mo.Ignore());

                cfg.CreateMap<SystemLog, SystemLogModel>()
                    .ForMember(dest => dest.EntityType, mo => mo.Ignore())
                    .ForMember(dest => dest.LogLevel, mo => mo.Ignore());
                cfg.CreateMap<SystemLogModel, SystemLog>()
                    .ForMember(dest => dest.EntityType, mo => mo.Ignore())
                    .ForMember(dest => dest.LogLevel, mo => mo.Ignore());

                cfg.CreateMap<Comment, CommentModel>()
                    .ForMember(dest => dest.postReplyModel, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.Replies, mo => mo.Ignore());
                cfg.CreateMap<CommentModel, Comment>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Exams, mo => mo.Ignore())
                    .ForMember(dest => dest.Homeworks, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Products, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore());

                cfg.CreateMap<Division, DivisionModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")));

                cfg.CreateMap<DivisionExam, DivisionExamModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore());
                cfg.CreateMap<DivisionExamModel, DivisionExam>()
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore());

                cfg.CreateMap<DivisionHomework, DivisionHomeworkModel>()
                    .ForMember(dest => dest.Division, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());
                cfg.CreateMap<DivisionHomeworkModel, DivisionHomework>()
                    .ForMember(dest => dest.Homework, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<DivisionSubject, DivisionSubjectModel>()
                    .ForMember(dest => dest.Division, mo => mo.Ignore())
                    .ForMember(dest => dest.Subject, mo => mo.Ignore());
                cfg.CreateMap<DivisionSubjectModel, DivisionSubject>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<Designation, DesignationModel>();
                cfg.CreateMap<DesignationModel, Designation>();

                cfg.CreateMap<Employee, EmployeeModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCastes, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableContracts, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableContractTypes, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableDesignations, mo => mo.Ignore())
                    .ForMember(dest => dest.Designation, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStartDateString, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractEndDateString, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore());
                cfg.CreateMap<EmployeeModel, Employee>()
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.EmployeePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.Designation, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore());

                cfg.CreateMap<EmployeeAttendance, EmployeeAttendanceModel>()
                    .ForMember(dest => dest.Employee, mo => mo.Ignore())
                    .ForMember(dest => dest.EmployeeAttendanceList, mo => mo.Ignore());
                cfg.CreateMap<EmployeeAttendanceModel, EmployeeAttendance>()
                    .ForMember(dest => dest.AttendanceStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());

                cfg.CreateMap<Event, EventModel>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.InsertPictureModel, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.Url, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                          .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());
                cfg.CreateMap<EventModel, Event>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                          .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<EventPicture, EventPictureModel>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<EventPictureModel, EventPicture>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<News, NewsModel>()
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.Comments, mo => mo.Ignore());
                cfg.CreateMap<NewsModel, News>()
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.NewsStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());

                cfg.CreateMap<NewsPicture, NewsPictureModel>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<NewsPictureModel, NewsPicture>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<FeeCategory, FeeCategoryModel>()
                    .ForMember(dest => dest.Category, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore());
                cfg.CreateMap<FeeCategoryModel, FeeCategory>()
                    .ForMember(dest => dest.Category, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore());

                cfg.CreateMap<Feedback, FeedbackModel>()
                    .ForMember(dest => dest.SentSuccess, mo => mo.Ignore());
                cfg.CreateMap<FeedbackModel, Feedback>()
                    .ForMember(dest => dest.IsDeleted, mo => mo.Ignore());

                cfg.CreateMap<Homework, HomeworkModel>()
                    .ForMember(dest => dest.AvailableStudentApprovals, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                    .ForMember(dest => dest.EndDate, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableTeacherApprovals, mo => mo.Ignore());
                cfg.CreateMap<HomeworkModel, Homework>()
                    .ForMember(dest => dest.StudentHomeWorkStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherApprovalStatus, mo => mo.Ignore());

                cfg.CreateMap<Message, MessageModel>()
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore());
                cfg.CreateMap<MessageModel, Message>()
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore());

                cfg.CreateMap<Option, OptionModel>()
                    .ForMember(dest => dest.Question, mo => mo.Ignore());
                cfg.CreateMap<OptionModel, Option>()
                    .ForMember(dest => dest.Question, mo => mo.Ignore());

                cfg.CreateMap<Payment, PaymentModel>()
                    .ForMember(dest => dest.Allowance, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());
                cfg.CreateMap<PaymentModel, Payment>()
                    .ForMember(dest => dest.Allowance, mo => mo.Ignore());

                cfg.CreateMap<PermissionRecord, PermissionRecordModel>()
                    .ForMember(dest => dest.Roles, mo => mo.Ignore());
                cfg.CreateMap<PermissionRecordModel, PermissionRecord>()
                    .ForMember(dest => dest.PermissionRoles, mo => mo.Ignore());

                cfg.CreateMap<ProductCategory, ProductCategoryModel>()
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.ProductCategory, mo => mo.Ignore());

                cfg.CreateMap<Product, ProductModel>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.postCommentModel, mo => mo.Ignore())
                    .ForMember(dest => dest.ProductCategory, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableVendors, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ProductModel, Product>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.ProductCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.Vendor, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore());

                cfg.CreateMap<ProductPicture, ProductPictureModel>()
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<ProductPictureModel, ProductPicture>()
                    .ForMember(dest => dest.Product, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<Purchase, PurchaseModel>()
                    .ForMember(dest => dest.Vendor, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<PurchaseModel, Purchase>()
                    .ForMember(dest => dest.Product, mo => mo.Ignore());

                cfg.CreateMap<Reaction, ReactionModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<ReactionModel, Reaction>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());

                cfg.CreateMap<Reply, ReplyModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.ToString("U")))
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ReplyModel, Reply>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore());

                cfg.CreateMap<Reply, ReplyModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ReplyModel, Reply>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore());

                cfg.CreateMap<School, SchoolModel>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.ProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.CoverPicture, mo => mo.Ignore());
                cfg.CreateMap<SchoolModel, School>()
                    .ForMember(dest => dest.User, mo => mo.Ignore());

                cfg.CreateMap<Student, StudentModel>()
                    .ForMember(dest => dest.Division_Class_Student, mo => mo.Ignore())
                    .ForMember(dest => dest.FatherPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.AvailableHouses, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCastes, mo => mo.Ignore())
                    .ForMember(dest => dest.MotherPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentPicture, mo => mo.Ignore());

                cfg.CreateMap<StudentModel, Student>()
                    .ForMember(dest => dest.AdmissionStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivision, mo => mo.Ignore())
                    .ForMember(dest => dest.House, mo => mo.Ignore())
                    .ForMember(dest => dest.PersonalityStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentHouse, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroups, mo => mo.Ignore());

                cfg.CreateMap<Student_MessageGroup, Student_MessageGroupModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore());
                cfg.CreateMap<Student_MessageGroupModel, Student_MessageGroup>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore());

                cfg.CreateMap<StudentAttendance, StudentAttendanceModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStatuses, mo => mo.Ignore());
                cfg.CreateMap<StudentAttendanceModel, StudentAttendance>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.AttendanceStatus, mo => mo.Ignore());

                cfg.CreateMap<StudentExam, StudentExamModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore());
                cfg.CreateMap<StudentExamModel, StudentExam>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());

                cfg.CreateMap<StudentHomework, StudentHomeworkModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());
                cfg.CreateMap<StudentHomeworkModel, StudentHomework>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());

                cfg.CreateMap<Subject, SubjectModel>()
                    .ForMember(dest => dest.Division_Class_Subject, mo => mo.Ignore());

                cfg.CreateMap<SubjectExam, SubjectExamModel>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());
                cfg.CreateMap<SubjectExamModel, SubjectExam>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());

                cfg.CreateMap<Teacher, TeacherModel>()
                    .ForMember(dest => dest.AvailableEmployees, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.Divisions, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true, false)))
                    .ForMember(dest => dest.Classes, mo => mo.Ignore());
                cfg.CreateMap<TeacherModel, Teacher>()
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroups, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore());

                cfg.CreateMap<TeacherExam, TeacherExamModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore());
                cfg.CreateMap<TeacherExamModel, TeacherExam>()
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore());

                cfg.CreateMap<Template, TemplateModel>()
                    .ForMember(dest => dest.customPagesModel, mo => mo.Ignore())
                    .ForMember(dest => dest.dataTokens, mo => mo.Ignore());
                cfg.CreateMap<TemplateModel, Template>()
                    .ForMember(dest => dest.Tokens, mo => mo.Ignore());

                cfg.CreateMap<DataToken, DataTokenModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<DataTokenModel, DataToken>()
                    .ForMember(dest => dest.user, mo => mo.Ignore());

                cfg.CreateMap<Religion, ReligionModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<ReligionModel, Religion>();

                cfg.CreateMap<TimeTable, TimeTableModel>()
                    .ForMember(dest => dest.AvailableWeekDays, mo => mo.Ignore())
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());
                cfg.CreateMap<TimeTableModel, TimeTable>()
                    .ForMember(dest => dest.DivisionSubject, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.WeekDay, mo => mo.Ignore());

                cfg.CreateMap<Slider, SliderModel>()
                    .ForMember(dest => dest.AvailableAreas, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.UserId, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<SliderModel, Slider>()
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore());

            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}