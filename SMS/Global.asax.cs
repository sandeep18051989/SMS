using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EF.Core;
using EF.Core.Enums;
using EF.Data;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using EF.Services.Tasks;
using FluentValidation.Mvc;
using SMS.App_Start;
using SMS.Controllers;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using EF.Services.Social;
using System.Reflection;

namespace SMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Register All Routes
            var routeRegistery = ContextHelper.Current.Resolve<IRouteRegistrar>();
            routeRegistery.RegisterRoutes(routes);

            routes.MapRoute(
                        "Default", // Route name
                        "{controller}/{action}/{id}", // URL with parameters
                        new { controller = "Developer", action = "Index", id = UrlParameter.Optional },
                        new[] { "Nop.Web.Controllers" }
                 );

        }

        protected void Application_Start()
        {
            //disable "X-AspNetMvc-Version" header name
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize CMS Context
            ContextHelper.Initialize(false);
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.DefaultNamespaces.Add("SMS.Controllers");
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new MetadataProvider();

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            FluentValidationModelValidatorProvider.Configure();
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new EFValidatorFactory()));

            bool databaseInstalled = DatabaseHelper.DatabaseIsInstalled();
            //start scheduled tasks
            if (databaseInstalled)
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }

            if (databaseInstalled)
            {
                GlobalFilters.Filters.Add(new ProfilingActionFilter());
            }

            // Run Social Engines
            if (databaseInstalled)
            {
                var authenticationService = ContextHelper.Current.Resolve<ISocialPluginService>();
                var socialFactory = ContextHelper.Current.Resolve<ISocialModelFactory>();
                var activeSocialPlugins = authenticationService.GetSocialPlugins(true);
                foreach (var eam in activeSocialPlugins)
                {
                    var entryClass = socialFactory.GetEntryPoint(eam.AssemblyName, eam.AuthenticationMethodServiceNamespace, eam.AuthenticationMethodService, "Install");
                    if(entryClass != null && !eam.IsInstalled)
                    {
                        entryClass.Install();
                    }
                }
            }

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var urlHelper = ContextHelper.Current.Resolve<IUrlHelper>();
            if (urlHelper.IsStaticResource(Request))
                return;

            string keepAliveUrl = string.Format("{0}keepalive/index", urlHelper.GetLocation());
            if (urlHelper.GetThisPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;

            //ensure database is installed
            if (!DatabaseHelper.DatabaseIsInstalled())
            {
                string installUrl = string.Format("{0}install", urlHelper.GetLocation());
                if (!urlHelper.GetThisPageUrl(false).StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    Response.Redirect(installUrl);
                }
            }

            if (!DatabaseHelper.DatabaseIsInstalled())
                return;

            MiniProfiler.Start();
            //store a value indicating whether profiler was started
            HttpContext.Current.Items["ef.MiniProfilerStarted"] = true;

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //miniprofiler
            var miniProfilerStarted = HttpContext.Current.Items.Contains("ef.MiniProfilerStarted") &&
                    Convert.ToBoolean(HttpContext.Current.Items["ef.MiniProfilerStarted"]);
            if (miniProfilerStarted)
            {
                MiniProfiler.Stop();
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            SetWorkingCulture();
        }

        protected void SetWorkingCulture()
        {
            if (!DatabaseHelper.DatabaseIsInstalled())
                return;

            //ignore static resources
            var urlHelper = ContextHelper.Current.Resolve<IUrlHelper>();
            if (urlHelper.IsStaticResource(Request))
                return;

            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            if (exception == null)
                return;

            if (!DatabaseHelper.DatabaseIsInstalled())
                return;

            try
            {
                // System Log
                var systemLogger = ContextHelper.Current.Resolve<ISystemLogService>();
                var currentUserContext = ContextHelper.Current.Resolve<IUserContext>();

                if (exception is System.Threading.ThreadAbortException)
                    return;

                string message = exception.ToString();
                systemLogger.InsertSystemLog(LogLevel.Error, exception.Message, message, currentUserContext.CurrentUser);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = ContextHelper.Current.Resolve<IUrlHelper>();
                if (!webHelper.IsStaticResource(Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                    Response.ContentType = "text/html";

                    // Call target Controller and pass the routeData.
                    IController errorController = ContextHelper.Current.Resolve<CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "PageNotFound");

                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }

        protected bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //just try to connect
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
