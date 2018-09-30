using System;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Data;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class InstallController : PublicHttpController
	{
		private readonly CMSConfig _config;
		#region Ctor

		public InstallController(CMSConfig config)
		{
			this._config = config;
		}

		#endregion

		#region Utilities

		/// <summary>
		/// A value indicating whether we use MARS (Multiple Active Result Sets)
		/// </summary>
		protected bool UseMars
		{
			get { return false; }
		}

		[NonAction]
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

		[NonAction]
		protected string CreateDatabase(string connectionString, string collation, int triesToConnect = 10)
		{
			try
			{
				//parse database name
				var builder = new SqlConnectionStringBuilder(connectionString);
				var databaseName = builder.InitialCatalog;
				//now create connection string to 'master' dabatase. It always exists.
				builder.InitialCatalog = "master";
				var masterCatalogConnectionString = builder.ToString();
				string query = string.Format("CREATE DATABASE [{0}]", databaseName);
				if (!String.IsNullOrWhiteSpace(collation))
					query = string.Format("{0} COLLATE {1}", query, collation);
				using (var conn = new SqlConnection(masterCatalogConnectionString))
				{
					conn.Open();
					using (var command = new SqlCommand(query, conn))
					{
						command.ExecuteNonQuery();
					}
				}

				//try connect
				if (triesToConnect > 0)
				{
					//Sometimes on slow servers (hosting) there could be situations when database requires some time to be created.
					//But we have already started creation of tables and sample data.
					//As a result there is an exception thrown and the installation process cannot continue.
					//That's why we are in a cycle of "triesToConnect" times trying to connect to a database with a delay of one second.
					for (var i = 0; i <= triesToConnect; i++)
					{
						if (i == triesToConnect)
							throw new Exception("Unable to connect to the new database. Please try one more time");

						if (!this.SqlServerDatabaseExists(connectionString))
							Thread.Sleep(1000);
						else
							break;
					}
				}

				return string.Empty;
			}
			catch (Exception ex)
			{
				return string.Format(ex.Message);
			}
		}

		/// <summary>
		/// Create contents of connection strings used by the SqlConnection class
		/// </summary>
		/// <param name="trustedConnection">Avalue that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)</param>
		/// <param name="serverName">The name or network address of the instance of SQL Server to connect to</param>
		/// <param name="databaseName">The name of the database associated with the connection</param>
		/// <param name="userName">The user ID to be used when connecting to SQL Server</param>
		/// <param name="password">The password for the SQL Server account</param>
		/// <param name="timeout">The connection timeout</param>
		/// <returns>Connection string</returns>
		[NonAction]
		protected string CreateConnectionString(bool trustedConnection,
			 string serverName, string databaseName,
			 string userName, string password, int timeout = 0)
		{
			var builder = new SqlConnectionStringBuilder
			{
				IntegratedSecurity = trustedConnection,
				DataSource = serverName,
				InitialCatalog = databaseName
			};
			if (!trustedConnection)
			{
				builder.UserID = userName;
				builder.Password = password;
			}
			builder.PersistSecurityInfo = false;
			if (this.UseMars)
			{
				builder.MultipleActiveResultSets = true;
			}
			if (timeout > 0)
			{
				builder.ConnectTimeout = timeout;
			}
			return builder.ConnectionString;
		}

		#endregion

		#region Install
		public ActionResult Index()
		{
			this.Server.ScriptTimeout = 300;
			var model = new InstallDatabaseModel
			{
				Datasource = ".",
				Database = "sms",
				AdminUsername = "admin",
				Username = "sa",
				Password = "ayush@123",
				AdminPassword = "@password1",
				UserId = 1
			};
			model.School.FullName = "Sandeep School";
			model.School.AcadmicYearName = DateTime.Now.ToString("yyyy") + "-" + Convert.ToInt32(Convert.ToInt32(DateTime.Now.ToString("yy")) + 1).ToString(); 
			return View(model);
		}

		[HttpPost]
		public ActionResult Index(InstallDatabaseModel model)
		{
			if (DatabaseHelper.DatabaseIsInstalled())
				return RedirectToRoute("HomePage");

			//set page timeout to 5 minutes
			this.Server.ScriptTimeout = 300;
			string connectionString;
			if (ModelState.IsValid)
			{
				var webHelper = ContextHelper.Current.Resolve<IUrlHelper>();
				try
				{
					var settingsManager = new DataSettingsContext();
					connectionString = "Data Source=" + model.Datasource + ";Initial Catalog=" + model.Database + ";User id=" + model.Username + ";pwd=" + model.Password + "";
					if (!SqlServerDatabaseExists(connectionString.Trim()))
					{
						//create database
						var collation = "SQL_Latin1_General_CP1_CI_AS";
						var errorCreatingDatabase = CreateDatabase(connectionString, collation);
						if (!String.IsNullOrEmpty(errorCreatingDatabase))
							throw new Exception(errorCreatingDatabase);

						var dataProvider = "sqlserver";
						var settings = new DatabaseSettings
						{
							DataProvider = dataProvider,
							DataConnectionString = connectionString
						};
						settingsManager.SaveSettings(settings);

						//init data provider
						var dataProviderInstance = ContextHelper.Current.Resolve<DataManager>().LoadDataProvider();
						dataProviderInstance.InitDatabase();

						//now resolve installation service
						var installationService = ContextHelper.Current.Resolve<IInstallationService>();
						var school = new School();

						school.AffiliationNumber = !String.IsNullOrEmpty(model.School.AffiliationNumber) ? model.School.AffiliationNumber.Trim() : "";
						school.RegistrationNumber = !String.IsNullOrEmpty(model.School.RegistrationNumber) ? model.School.RegistrationNumber.Trim() : "";

						school.AcadmicYearName = model.School.AcadmicYearName;
						school.City = !String.IsNullOrEmpty(model.School.City) ? model.School.City.Trim() : "";
						school.State = !String.IsNullOrEmpty(model.School.State) ? model.School.State.Trim() : "";
						school.Country = !String.IsNullOrEmpty(model.School.Country) ? model.School.Country.Trim() : "";
						school.Street1 = !String.IsNullOrEmpty(model.School.Street1) ? model.School.Street1.Trim() : "";
						school.Street2 = !String.IsNullOrEmpty(model.School.Street2) ? model.School.Street2.Trim() : "";
						school.Landmark = !String.IsNullOrEmpty(model.School.Landmark) ? model.School.Landmark.Trim() : "";
						school.ZipCode = !String.IsNullOrEmpty(model.School.ZipCode) ? model.School.ZipCode.Trim() : "";
						school.FullName = !String.IsNullOrEmpty(model.School.FullName) ? model.School.FullName.Trim() : "";
						school.Longitude = !String.IsNullOrEmpty(model.School.Longitude) ? model.School.Longitude.Trim() : "";
						school.Latitude = !String.IsNullOrEmpty(model.School.Latitude) ? model.School.Latitude.Trim() : "";
						school.FacebookLink = !String.IsNullOrEmpty(model.School.FacebookLink) ? model.School.FacebookLink.Trim() : "";
						school.TweeterLink = !String.IsNullOrEmpty(model.School.TweeterLink) ? model.School.TweeterLink.Trim() : "";
						school.InstagramLink = !String.IsNullOrEmpty(model.School.InstagramLink) ? model.School.InstagramLink.Trim() : "";
						school.GooglePlusLink = !String.IsNullOrEmpty(model.School.GooglePlusLink) ? model.School.GooglePlusLink.Trim() : "";
						school.PInterestLink = !String.IsNullOrEmpty(model.School.PInterestLink) ? model.School.PInterestLink.Trim() : "";
						school.SchoolGuid = new Guid();
						school.CoverPictureId = model.School.CoverPictureId;
						school.ProfilePictureId = model.School.ProfilePictureId;

						installationService.InstallData(model.AdminUsername, model.AdminPassword, school);

						//reset cache
						DatabaseHelper.ResetCache();

						//Redirect to home page
						return RedirectToRoute("Root");

					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}

			return View(model);
		}

		#endregion
	}
}
