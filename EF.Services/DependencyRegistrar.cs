using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EF.Core;
using EF.Services.Service;
using EF.Services;
using EF.Services.Context;
using EF.Core.Data;
using EF.Data;
using EF.Services.Http;
using EF.Core.Cache;
using EF.Services.Culture;
using EF.Services.Tasks;
using TrackerEnabledDbContext.Common.Interfaces;
using Wibci.CountryReverseGeocode;
using TrackerEnabledDbContext;
using EF.Services.Social;

namespace EF.Services
{
	public class DependencyRegistrar : IDependencyRegistrar
	{
		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, CMSConfig config)
		{
			//HTTP context and other related stuff
			builder.Register(c =>
				 //register FakeHttpContext when HttpContext is not available
				 HttpContext.Current != null ?
				 (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
				 (new AutoHttpContext("~/") as HttpContextBase))
				 .As<HttpContextBase>().InstancePerRequest();

			builder.Register(c => c.Resolve<HttpContextBase>().Request)
				 .As<HttpRequestBase>()
				 .InstancePerLifetimeScope();

			builder.Register(c => c.Resolve<HttpContextBase>().Response)
				 .As<HttpResponseBase>()
				 .InstancePerRequest();

			builder.Register(c => c.Resolve<HttpContextBase>().Server)
				 .As<HttpServerUtilityBase>()
				 .InstancePerRequest();

			//builder.Register(c => c.Resolve<HttpContextBase>().Session)
			//    .As<HttpSessionStateBase>()
			//    .InstancePerRequest();

			builder.RegisterType<UrlHelper>().As<IUrlHelper>().InstancePerLifetimeScope();

			//controllers
			builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

			// Data-Layer
			var dataSettingsManager = new DataSettingsContext();
			var dataProviderSettings = dataSettingsManager.LoadSettings();
			builder.Register(c => dataSettingsManager.LoadSettings()).As<DatabaseSettings>();
			builder.Register(x => new EfDataProviderManager(x.Resolve<DatabaseSettings>())).As<DataManager>().InstancePerDependency();

			builder.Register(x => x.Resolve<DataManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

			if (dataProviderSettings != null && dataProviderSettings.IsValid())
			{
				var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
				var dataProvider = efDataProviderManager.LoadDataProvider();
				dataProvider.InitConnectionFactory();

				builder.Register<EF.Data.IDbContext>(c => new EFDbContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
				builder.Register<ITrackerContext>(c => new EFDbContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
			}
			else
			{
				builder.Register<EF.Data.IDbContext>(c => new EFDbContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
				builder.Register<ITrackerContext>(c => new EFDbContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();

				//builder.Register<IDbContext>(c => new EFDbContext(dataSettingsManager.LoadSettings().DataConnectionString == null ? "..." : dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
			}

			builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
			// Register Services
			builder.RegisterType<WebContext>().As<IWebContext>().InstancePerLifetimeScope();
            builder.RegisterType<SocialSettingService>().As<ISocialSettingService>().InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());

            builder.RegisterType<SystemLogService>().As<ISystemLogService>().InstancePerLifetimeScope();
			builder.RegisterType<AuditService>().As<IAuditService>().InstancePerLifetimeScope();
			builder.RegisterType<BlogService>().As<IBlogService>().InstancePerLifetimeScope();
			builder.RegisterType<CommentService>().As<ICommentService>().InstancePerLifetimeScope();
			builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
			builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();
			builder.RegisterType<ReplyService>().As<IReplyService>().InstancePerLifetimeScope();
			builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
			builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
			builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
			builder.RegisterType<VideoService>().As<IVideoService>().InstancePerLifetimeScope();
			builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
			builder.RegisterType<UserContext>().As<IUserContext>().InstancePerLifetimeScope();
			builder.RegisterType<SliderService>().As<ISliderService>().InstancePerLifetimeScope();
			builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();

            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
			builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
			builder.RegisterType<FeedbackService>().As<IFeedbackService>().InstancePerLifetimeScope();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
			builder.RegisterType<TemplateService>().As<ITemplateService>().InstancePerLifetimeScope();
			builder.RegisterType<CustomPageService>().As<ICustomPageService>().InstancePerLifetimeScope();
			builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
			builder.RegisterType<UrlService>().As<IUrlService>().InstancePerLifetimeScope();
			builder.RegisterType<SMSService>().As<ISMSService>().InstancePerLifetimeScope();
			builder.RegisterType<RouteRegistrar>().As<IRouteRegistrar>().InstancePerLifetimeScope();
            builder.RegisterType<SocialModelFactory>().As<ISocialModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SocialPluginService>().As<ISocialPluginService>().InstancePerLifetimeScope();
            builder.RegisterType<SocialAuthorizer>().As<ISocialAuthorizer>().InstancePerLifetimeScope();
            builder.RegisterType<OpenAuthenticationService>().As<IOpenAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<LicenseService>().As<ILicenseService>().InstancePerLifetimeScope();

            builder.RegisterType<CountryReverseGeocodeService>().As<ICountryReverseGeocodeService>().InstancePerLifetimeScope();
			if (!DatabaseHelper.DatabaseIsInstalled())
			{
				builder.RegisterType<InstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
			}

			builder.RegisterType<CultureHelper>().As<ICultureHelper>().InstancePerLifetimeScope();

			// Scheduled Tasks
			builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
			builder.RegisterType<DefaultMachineNameProvider>().As<IMachineNameProvider>().InstancePerLifetimeScope();

			builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

		}

		/// <summary>
		/// Order of this dependency registrar implementation
		/// </summary>
		public int Order
		{
			get { return 0; }
		}
	}
	public class SettingsSource : IRegistrationSource
	{
		static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
			 "BuildRegistration",
			 BindingFlags.Static | BindingFlags.NonPublic);

		public IEnumerable<IComponentRegistration> RegistrationsFor(
				  Autofac.Core.Service service,
				  Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrations)
		{
			var ts = service as TypedService;
			if (ts != null && typeof(ITempSettings).IsAssignableFrom(ts.ServiceType))
			{
				var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
				yield return (IComponentRegistration)buildMethod.Invoke(null, null);
			}
		}

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    return c.Resolve<ISocialSettingService>().LoadSetting<TSettings>();
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }

	}
}
