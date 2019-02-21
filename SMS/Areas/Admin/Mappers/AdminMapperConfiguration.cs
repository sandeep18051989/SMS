using AutoMapper;
using EF.Core.Data;
using SMS.Models;
using SMS.Models.Widgets;
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
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.Url, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureId, mo => mo.Ignore())
                    .ForMember(dest => dest.VideoId, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CommentsCount, mo => mo.Ignore())
                    .ForMember(dest => dest.PicturesCount, mo => mo.Ignore())
                    .ForMember(dest => dest.ReactionsCount, mo => mo.Ignore())
                    .ForMember(dest => dest.VideosCount, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<BlogModel, Blog>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<Blog, BlogWidgetModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.DefaultPictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.DefaultVideoSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.HasDefaultPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.HasDefaultVideo, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.Username, mo => mo.Ignore())
                    .ForMember(dest => dest.LatestPosts, mo => mo.Ignore())
                    .ForMember(dest => dest.OlderPosts, mo => mo.Ignore())
                    .ForMember(dest => dest.PopularPosts, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore())
                    .ForMember(dest => dest.IsAuthenticated, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<BlogWidgetModel, Blog>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<AssessmentQuestion, AssessmentQuestionModel>()
                    .ForMember(dest => dest.Assessment, mo => mo.Ignore())
                    .ForMember(dest => dest.IsChecked, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Question, mo => mo.Ignore());
                cfg.CreateMap<AssessmentQuestionModel, AssessmentQuestion>()
                    .ForMember(dest => dest.Assessment, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Question, mo => mo.Ignore());

                cfg.CreateMap<AssessmentStudent, AssessmentStudentModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.IsChecked, mo => mo.Ignore())
                    .ForMember(dest => dest.Assessment, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<AssessmentStudentModel, AssessmentStudent>()
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.Assessment, mo => mo.Ignore());

                cfg.CreateMap<Assessment, AssessmentModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.DifficultyLevel, mo => mo.Ignore())
                    .ForMember(dest => dest.StringEndTime, mo => mo.Ignore())
                    .ForMember(dest => dest.StringStartTime, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableSubjects, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableDifficultyLevels, mo => mo.Ignore());
                cfg.CreateMap<AssessmentModel, Assessment>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.DifficultyLevel, mo => mo.Ignore());

                cfg.CreateMap<BlogPicture, BlogPictureModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<BlogPictureModel, BlogPicture>()
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Blog, mo => mo.Ignore());

                cfg.CreateMap<BookIssue, BookIssueModel>()
                    .ForMember(dest => dest.Book, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Librarian, mo => mo.Ignore());
                cfg.CreateMap<BookIssueModel, BookIssue>()
                    .ForMember(dest => dest.Book, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());

                cfg.CreateMap<Caste, CasteModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                    .ForMember(dest => dest.AvailableCategories, mo => mo.Ignore());
                cfg.CreateMap<CasteModel, Caste>()
                    .ForMember(dest => dest.Categories, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore());

                cfg.CreateMap<Category, CategoryModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")));
                cfg.CreateMap<CategoryModel, Category>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Castes, mo => mo.Ignore());

                cfg.CreateMap<Class, ClassModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")));
                cfg.CreateMap<ClassModel, Class>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<ClassRoom, ClassRoomModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")));
                cfg.CreateMap<ClassRoomModel, ClassRoom>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionExams, mo => mo.Ignore());

                cfg.CreateMap<ClassRoomDivision, ClassRoomDivisionModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.AvailableClassRooms, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")));
                cfg.CreateMap<ClassRoomDivisionModel, ClassRoomDivision>()
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<CustomPage, CustomPageModel>()
                    .ForMember(dest => dest.AvailableTemplates, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                    .ForMember(dest => dest.template, mo => mo.Ignore());
                cfg.CreateMap<CustomPageModel, CustomPage>()
                    .ForMember(dest => dest.PermissionRecord, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Template, mo => mo.Ignore());

                cfg.CreateMap<SystemLog, SystemLogModel>()
                    .ForMember(dest => dest.EntityType, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.LogLevel, mo => mo.Ignore());
                cfg.CreateMap<SystemLogModel, SystemLog>()
                    .ForMember(dest => dest.EntityType, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.LogLevel, mo => mo.Ignore());

                cfg.CreateMap<Holiday, HolidayModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.StringDate, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<HolidayModel, Holiday>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<NewsPicture, NewsPictureModel>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<NewsPictureModel, NewsPicture>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<NewsVideo, NewsVideoModel>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<NewsVideoModel, NewsVideo>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<BlogPicture, BlogPictureModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<BlogPictureModel, BlogPicture>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<BlogVideo, BlogVideoModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<BlogVideoModel, BlogVideo>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<EventPicture, EventPictureModel>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<EventPictureModel, EventPicture>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<EventVideo, EventVideoModel>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<EventVideoModel, EventVideo>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<Comment, CommentModel>()
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                    .ForMember(dest => dest.Replies, mo => mo.Ignore());
                cfg.CreateMap<CommentModel, Comment>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionExams, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionHomeworks, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Products, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore());

                cfg.CreateMap<Comment, CommentWidgetModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.ProfilePictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.Replies, mo => mo.Ignore());
                cfg.CreateMap<CommentWidgetModel, Comment>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionExams, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionHomeworks, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Products, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentExams, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentHomeworks, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherExams, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore());

                cfg.CreateMap<Division, DivisionModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableClassRooms, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomId, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore())
                    .ForMember(dest => dest.Homeworks, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")));
                cfg.CreateMap<DivisionModel, Division>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<DivisionExam, DivisionExamModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.Class, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore());
                cfg.CreateMap<DivisionExamModel, DivisionExam>()
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore());

                cfg.CreateMap<Qualification, QualificationModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());
                cfg.CreateMap<QualificationModel, Qualification>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<DivisionHomework, DivisionHomeworkModel>()
                    .ForMember(dest => dest.Division, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStudentApprovals, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableTeacherApprovals, mo => mo.Ignore())
                    .ForMember(dest => dest.StringStartDate, mo => mo.MapFrom(src => src.StartDate.Value.ToString("U")))
                    .ForMember(dest => dest.StringEndDate, mo => mo.MapFrom(src => src.EndDate.Value.ToString("U")))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());
                cfg.CreateMap<DivisionHomeworkModel, DivisionHomework>()
                    .ForMember(dest => dest.Homework, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherApprovalStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentHomeWorkStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<DivisionSubject, DivisionSubjectModel>()
                    .ForMember(dest => dest.ClassName, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassId, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionId, mo => mo.Ignore())
                    .ForMember(dest => dest.DivisionName, mo => mo.Ignore())
                    .ForMember(dest => dest.SubjectName, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.SubjectCode, mo => mo.Ignore());
                cfg.CreateMap<DivisionSubjectModel, DivisionSubject>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());

                cfg.CreateMap<Designation, DesignationModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());
                cfg.CreateMap<DesignationModel, Designation>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<Employee, EmployeeModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCastes, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableContractStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableContractTypes, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableDesignations, mo => mo.Ignore())
                    .ForMember(dest => dest.Designation, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStartDateString, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractEndDateString, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore());
                cfg.CreateMap<EmployeeModel, Employee>()
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ContractType, mo => mo.Ignore())
                    .ForMember(dest => dest.EmployeePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.Designation, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Religion, mo => mo.Ignore());

                cfg.CreateMap<EmployeeAttendance, EmployeeAttendanceModel>()
                    .ForMember(dest => dest.Employee, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.EmployeeAttendanceList, mo => mo.Ignore());
                cfg.CreateMap<EmployeeAttendanceModel, EmployeeAttendance>()
                    .ForMember(dest => dest.AttendanceStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore());

                cfg.CreateMap<Allowance, AllowanceModel>()
                    .ForMember(dest => dest.Designation, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableDesignations, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<AllowanceModel, Allowance>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Designation, mo => mo.Ignore());

                cfg.CreateMap<Event, EventModel>()
                     .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                     .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                     .ForMember(dest => dest.PictureId, mo => mo.Ignore())
                     .ForMember(dest => dest.VideoId, mo => mo.Ignore())
                     .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                     .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                     .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                     .ForMember(dest => dest.Url, mo => mo.Ignore());
                cfg.CreateMap<EventModel, Event>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<Event, EventWidgetModel>()
                     .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                     .ForMember(dest => dest.Comments, mo => mo.Ignore())
                     .ForMember(dest => dest.DefaultPictureSrc, mo => mo.Ignore())
                     .ForMember(dest => dest.DefaultVideoSrc, mo => mo.Ignore())
                     .ForMember(dest => dest.HasDefaultPicture, mo => mo.Ignore())
                     .ForMember(dest => dest.HasDefaultVideo, mo => mo.Ignore())
                     .ForMember(dest => dest.IsAuthenticated, mo => mo.Ignore())
                     .ForMember(dest => dest.IsStudent, mo => mo.Ignore())
                     .ForMember(dest => dest.IsTeacher, mo => mo.Ignore())
                     .ForMember(dest => dest.LatestPosts, mo => mo.Ignore())
                     .ForMember(dest => dest.OlderPosts, mo => mo.Ignore())
                     .ForMember(dest => dest.PopularPosts, mo => mo.Ignore())
                     .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                     .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                     .ForMember(dest => dest.Student, mo => mo.Ignore())
                     .ForMember(dest => dest.Venues, mo => mo.Ignore())
                     .ForMember(dest => dest.Videos, mo => mo.Ignore())
                     .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                     .ForMember(dest => dest.Url, mo => mo.Ignore());
                cfg.CreateMap<EventWidgetModel, Event>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore());

                cfg.CreateMap<EventPicture, EventPictureModel>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<EventPictureModel, EventPicture>()
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<News, NewsModel>()
                     .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                     .ForMember(dest => dest.AvailableStatuses, mo => mo.Ignore())
                     .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                     .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                     .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                     .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                     .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                     .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<NewsModel, News>()
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.NewsStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());

                cfg.CreateMap<News, NewsWidgetModel>()
                     .ForMember(dest => dest.DefaultPictureSrc, mo => mo.Ignore())
                     .ForMember(dest => dest.DefaultVideoSrc, mo => mo.Ignore())
                     .ForMember(dest => dest.Comments, mo => mo.Ignore())
                     .ForMember(dest => dest.HasDefaultPicture, mo => mo.Ignore())
                     .ForMember(dest => dest.HasDefaultVideo, mo => mo.Ignore())
                     .ForMember(dest => dest.Status, mo => mo.Ignore())
                     .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                     .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                     .ForMember(dest => dest.Videos, mo => mo.Ignore())
                     .ForMember(dest => dest.HasDefaultVideo, mo => mo.Ignore())
                     .ForMember(dest => dest.LatestPosts, mo => mo.Ignore())
                     .ForMember(dest => dest.OlderPosts, mo => mo.Ignore())
                     .ForMember(dest => dest.User, mo => mo.Ignore())
                     .ForMember(dest => dest.Student, mo => mo.Ignore())
                     .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                     .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<NewsWidgetModel, News>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.NewsStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());

                cfg.CreateMap<NewsPicture, NewsPictureModel>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());
                cfg.CreateMap<NewsPictureModel, NewsPicture>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<Book, BookModel>()
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.BookStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableBookStatuses, mo => mo.Ignore());
                cfg.CreateMap<BookModel, Book>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.BookStatus, mo => mo.Ignore());

                cfg.CreateMap<AcadmicYear, AcadmicYearModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<AcadmicYearModel, AcadmicYear>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<BookIssue, BookIssueModel>()
                    .ForMember(dest => dest.AvailableEmployees, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStudents, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableBooks, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Librarian, mo => mo.Ignore())
                    .ForMember(dest => dest.StringStartDate, mo => mo.Ignore())
                    .ForMember(dest => dest.StringEndDate, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Book, mo => mo.Ignore());
                cfg.CreateMap<BookIssueModel, BookIssue>()
                    .ForMember(dest => dest.Book, mo => mo.Ignore())
                    .ForMember(dest => dest.Employee, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore());

                cfg.CreateMap<FeeCategory, FeeCategoryModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.StringPeriodFrom, mo => mo.Ignore())
                    .ForMember(dest => dest.StringPeriodTo, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableClassDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.CategoryName, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassDivisionName, mo => mo.Ignore());
                cfg.CreateMap<FeeCategoryModel, FeeCategory>()
                    .ForMember(dest => dest.Category, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassDivision, mo => mo.Ignore());

                cfg.CreateMap<Feedback, FeedbackModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.SentSuccess, mo => mo.Ignore());
                cfg.CreateMap<FeedbackModel, Feedback>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.IsDeleted, mo => mo.Ignore());

                cfg.CreateMap<FeeDetail, FeeDetailModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CashierName, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.StringDate, mo => mo.Ignore())
                    .ForMember(dest => dest.Date, mo => mo.Ignore())
                    .ForMember(dest => dest.Status, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableFeeCategoryStructures, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStudents, mo => mo.Ignore());
                cfg.CreateMap<FeeDetailModel, FeeDetail>()
                     .ForMember(dest => dest.FeeStatus, mo => mo.Ignore())
                     .ForMember(dest => dest.Employee, mo => mo.Ignore())
                     .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                     .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                     .ForMember(dest => dest.Student, mo => mo.Ignore());

                cfg.CreateMap<Homework, HomeworkModel>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.StartDate, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.EndDate, mo => mo.Ignore());
                cfg.CreateMap<HomeworkModel, Homework>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<Message, MessageModel>()
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore());
                cfg.CreateMap<MessageModel, Message>()
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore());

                cfg.CreateMap<Option, OptionModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Question, mo => mo.Ignore());
                cfg.CreateMap<OptionModel, Option>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Question, mo => mo.Ignore());

                cfg.CreateMap<Question, QuestionModel>()
                    .ForMember(dest => dest.AvailableLevels, mo => mo.Ignore())
                    .ForMember(dest => dest.Difficulty, mo => mo.Ignore())
                    .ForMember(dest => dest.MatchFollowingOptions, mo => mo.Ignore())
                    .ForMember(dest => dest.OptionCount, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQuestionTypes, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableSubjects, mo => mo.Ignore());
                cfg.CreateMap<QuestionModel, Question>()
                    .ForMember(dest => dest.QuestionType, mo => mo.Ignore())
                    .ForMember(dest => dest.DifficultyLevel, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Subject, mo => mo.Ignore());

                cfg.CreateMap<Payment, PaymentModel>()
                    .ForMember(dest => dest.Employee, mo => mo.Ignore())
                    .ForMember(dest => dest.StringDate, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableEmployees, mo => mo.Ignore());
                cfg.CreateMap<PaymentModel, Payment>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore());

                cfg.CreateMap<PermissionRecord, PermissionRecordModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Roles, mo => mo.Ignore());
                cfg.CreateMap<PermissionRecordModel, PermissionRecord>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.PermissionRoles, mo => mo.Ignore());

                cfg.CreateMap<UserRole, RoleModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<RoleModel, UserRole>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.PermissionRecords, mo => mo.Ignore());

                cfg.CreateMap<ProductCategory, ProductCategoryModel>()
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableProductCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.Url, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ProductCategory, mo => mo.Ignore());
                cfg.CreateMap<ProductCategoryModel, ProductCategory>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<Product, ProductModel>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.HasDefaultPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.IsAuthenticated, mo => mo.Ignore())
                    .ForMember(dest => dest.HasDefaultVideo, mo => mo.Ignore())
                    .ForMember(dest => dest.DefaultVideoSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.DefaultPictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.ProductCategory, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableVendors, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.FeaturedProducts, mo => mo.Ignore())
                    .ForMember(dest => dest.NewProducts, mo => mo.Ignore())
                    .ForMember(dest => dest.UpcomingProducts, mo => mo.Ignore())
                    .ForMember(dest => dest.RelatedProducts, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ProductModel, Product>()
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.ProductCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.Videos, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore());

                cfg.CreateMap<ProductPicture, ProductPictureModel>()
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<ProductPictureModel, ProductPicture>()
                    .ForMember(dest => dest.Product, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<Picture, PictureModel>()
                    .ForMember(dest => dest.CaptionOff, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedDateString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.UploadedBy, mo => mo.Ignore());
                cfg.CreateMap<PictureModel, Picture>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.Products, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore())
                    .ForMember(dest => dest.Sliders, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());

                cfg.CreateMap<Purchase, PurchaseModel>()
                    .ForMember(dest => dest.AvailableVendors, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<PurchaseModel, Purchase>()
                    .ForMember(dest => dest.Product, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore());

                cfg.CreateMap<Vendor, VendorModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<VendorModel, Vendor>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore());

                cfg.CreateMap<House, HouseModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore());
                cfg.CreateMap<HouseModel, House>()
                    .ForMember(dest => dest.Students, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore());

                cfg.CreateMap<Reaction, ReactionModel>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());
                cfg.CreateMap<ReactionModel, Reaction>()
                    .ForMember(dest => dest.Blog, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore())
                    .ForMember(dest => dest.Event, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Picture, mo => mo.Ignore())
                    .ForMember(dest => dest.Video, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Product, mo => mo.Ignore());

                cfg.CreateMap<Reply, ReplyModel>()
                    .ForMember(dest => dest.CreatedOnString, mo => mo.MapFrom(src => src.CreatedOn.Value.ToString("U")))
                    .ForMember(dest => dest.ModifiedOnString, mo => mo.MapFrom(src => src.ModifiedOn.Value.ToString("U")))
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ReplyModel, Reply>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore());

                cfg.CreateMap<Reply, ReplyWidgetModel>()
                    .ForMember(dest => dest.ProfilePictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.EmployeeName, mo => mo.Ignore());
                cfg.CreateMap<ReplyWidgetModel, Reply>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore());

                cfg.CreateMap<Reply, ReplyModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());
                cfg.CreateMap<ReplyModel, Reply>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Comment, mo => mo.Ignore());

                cfg.CreateMap<School, SchoolModel>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.ProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CoverPicture, mo => mo.Ignore());
                cfg.CreateMap<SchoolModel, School>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore());

                cfg.CreateMap<Student, StudentModel>()
                    .ForMember(dest => dest.FatherPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.FileId, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableReligions, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)))
                    .ForMember(dest => dest.AvailableHouses, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCastes, mo => mo.Ignore())
                    .ForMember(dest => dest.MotherPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAdmissionStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailablePersonalityStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentPicture, mo => mo.Ignore());
                cfg.CreateMap<StudentModel, Student>()
                    .ForMember(dest => dest.AdmissionStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivision, mo => mo.Ignore())
                    .ForMember(dest => dest.House, mo => mo.Ignore())
                    .ForMember(dest => dest.PersonalityStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentHouse, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroups, mo => mo.Ignore());

                cfg.CreateMap<Student_MessageGroup, Student_MessageGroupModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore());
                cfg.CreateMap<Student_MessageGroupModel, Student_MessageGroup>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroup, mo => mo.Ignore());

                cfg.CreateMap<StudentAttendance, StudentAttendanceModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStatuses, mo => mo.Ignore());
                cfg.CreateMap<StudentAttendanceModel, StudentAttendance>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AttendanceStatus, mo => mo.Ignore());

                cfg.CreateMap<Exam, ExamModel>()
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.StringStartDate, mo => mo.Ignore())
                    .ForMember(dest => dest.StringEndDate, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableClassRooms, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore());
                cfg.CreateMap<ExamModel, Exam>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<StudentExam, StudentExamModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableClassRooms, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableExams, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore());
                cfg.CreateMap<StudentExamModel, StudentExam>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());

                cfg.CreateMap<StudentHomework, StudentHomeworkModel>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableStudentApprovals, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableTeacherApprovals, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());
                cfg.CreateMap<StudentHomeworkModel, StudentHomework>()
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentHomeWorkStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherApprovalStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Homework, mo => mo.Ignore());

                cfg.CreateMap<Subject, SubjectModel>()
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore());
                cfg.CreateMap<SubjectModel, Subject>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Teachers, mo => mo.Ignore());

                cfg.CreateMap<SubjectExam, SubjectExamModel>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());
                cfg.CreateMap<SubjectExamModel, SubjectExam>()
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore());

                cfg.CreateMap<Teacher, TeacherModel>()
                    .ForMember(dest => dest.AvailableEmployees, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableQualifications, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailablePersonalityStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.AcadmicYear, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.FileId, mo => mo.Ignore())
                    .ForMember(dest => dest.Url, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<TeacherModel, Teacher>()
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroups, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.PersonalityStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore());

                cfg.CreateMap<Teacher, TeacherWidgetModel>()
                    .ForMember(dest => dest.ProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore())
                    .ForMember(dest => dest.SystemName, mo => mo.MapFrom(src => src.GetSystemName(true)));
                cfg.CreateMap<TeacherWidgetModel, Teacher>()
                    .ForMember(dest => dest.Qualification, mo => mo.Ignore())
                    .ForMember(dest => dest.MessageGroups, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoomDivisions, mo => mo.Ignore())
                    .ForMember(dest => dest.Files, mo => mo.Ignore())
                    .ForMember(dest => dest.PersonalityStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Subjects, mo => mo.Ignore());

                cfg.CreateMap<TeacherExam, TeacherExamModel>()
                    .ForMember(dest => dest.AvailableGradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableAcadmicYears, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableClassRooms, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableExams, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableResultStatuses, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.Selected, mo => mo.Ignore())
                    .ForMember(dest => dest.Comments, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore());
                cfg.CreateMap<TeacherExamModel, TeacherExam>()
                    .ForMember(dest => dest.GradeSystem, mo => mo.Ignore())
                    .ForMember(dest => dest.ResultStatus, mo => mo.Ignore())
                    .ForMember(dest => dest.ClassRoom, mo => mo.Ignore())
                    .ForMember(dest => dest.Exam, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore());

                cfg.CreateMap<Template, TemplateModel>()
                    .ForMember(dest => dest.customPagesModel, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.dataTokens, mo => mo.Ignore());
                cfg.CreateMap<TemplateModel, Template>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Tokens, mo => mo.Ignore());

                cfg.CreateMap<DataToken, DataTokenModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<DataTokenModel, DataToken>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.user, mo => mo.Ignore());

                cfg.CreateMap<Religion, ReligionModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<ReligionModel, Religion>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());

                cfg.CreateMap<TimeTable, TimeTableModel>()
                    .ForMember(dest => dest.AvailableWeekDays, mo => mo.Ignore())
                    .ForMember(dest => dest.Subject, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Division, mo => mo.Ignore());
                cfg.CreateMap<TimeTableModel, TimeTable>()
                    .ForMember(dest => dest.DivisionSubject, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.WeekDay, mo => mo.Ignore());

                cfg.CreateMap<Slider, SliderModel>()
                    .ForMember(dest => dest.AvailableAreas, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore())
                    .ForMember(dest => dest.UserId, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<SliderModel, Slider>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Pictures, mo => mo.Ignore());

                cfg.CreateMap<User, UserModel>()
                    .ForMember(dest => dest.AvailableRoles, mo => mo.Ignore())
                    .ForMember(dest => dest.CoverPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.ProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.Student, mo => mo.Ignore())
                    .ForMember(dest => dest.Teacher, mo => mo.Ignore())
                    .ForMember(dest => dest.IsStudent, mo => mo.Ignore())
                    .ForMember(dest => dest.IsTeacher, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentCoverPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherCoverPicture, mo => mo.Ignore())
                    .ForMember(dest => dest.StudentProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.TeacherProfilePicture, mo => mo.Ignore())
                    .ForMember(dest => dest.ChangePassword, mo => mo.Ignore());
                cfg.CreateMap<UserModel, User>()
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.Roles, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.SocialRecords, mo => mo.Ignore());

                cfg.CreateMap<Video, VideoModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.UploadedBy, mo => mo.Ignore())
                    .ForMember(dest => dest.VideoSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore());
                cfg.CreateMap<VideoModel, Video>()
                    .ForMember(dest => dest.Blogs, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.ModifiedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.News, mo => mo.Ignore())
                    .ForMember(dest => dest.VideoSrc, mo => mo.Ignore())
                    .ForMember(dest => dest.Products, mo => mo.Ignore())
                    .ForMember(dest => dest.Events, mo => mo.Ignore())
                    .ForMember(dest => dest.Reactions, mo => mo.Ignore());

            });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}