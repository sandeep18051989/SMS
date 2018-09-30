using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Core.Data;
using SMS.Models;

namespace SMS.Areas.Admin.Models
{
	public partial class DashboardModel
	{
		public DashboardModel()
		{
			Settings = new SettingsViewModel();
			adminUserModel = new AdminUserModel();
			Notifications = new NotificationModel();
			Comments = new List<AdminCommentsModel>();

			// Registered Users Chart
			AvailableMonths = new List<SelectListItem>();
			AvailableYears = new List<SelectListItem>();
			RegisteredUsers = new List<User>();
			RegisteredUsersChartModels = new List<RegisteredUsersChartModel>();

			// Registered Feedbacks
			Feedbacks_AvailableMonths = new List<SelectListItem>();
			Feedbacks_AvailableYears = new List<SelectListItem>();
			Feedbacks = new List<Feedback>();
			FeedbacksChartModels = new List<RegisteredFeedbacks>();

			// Registered Events
			Events_AvailableMonths = new List<SelectListItem>();
			Events_AvailableYears = new List<SelectListItem>();
			Events = new List<Event>();
			EventsChartModels = new List<RegisteredEvents>();

			// Active/Inactive users
			ActiveInactiveUsers = new RegisteredActiveInactiveUsers();
			UniqueUsers = new List<Location>();
			ReturnUsers = new List<Location>();
			MergedUsers = new List<Location>();

			ConsolidateUserModel = new List<ConsolidateUserModel>();
		}

		#region Properties
		public SettingsViewModel Settings { get; set; }

		public AdminUserModel adminUserModel { get; set; }

		public NotificationModel Notifications { get; set; }

		public int TotalVisitersTillDate { get; set; }

		public string LastVisiterName { get; set; }

		public DateTime LastVisiterDateTime { get; set; }

		public int VisitsToday { get; set; }

		public int FeedbacksToday { get; set; }

		public int CommentsToday { get; set; }

		public int ProductsToday { get; set; }

		public int EventsToday { get; set; }

		public int BlogsToday { get; set; }

		public IList<AdminCommentsModel> Comments { get; set; }

		public IList<Location> UniqueUsers { get; set; }
		public IList<Location> ReturnUsers { get; set; }
		public IList<Location> MergedUsers { get; set; }
		public IList<ConsolidateUserModel> ConsolidateUserModel { get; set; }
		public string ActiveSettings { get; set; }

		#endregion

		#region User Registrations
		public int SelectedMonth { get; set; }
		public IList<SelectListItem> AvailableMonths { get; set; }

		public int SelectedYear { get; set; }
		public IList<SelectListItem> AvailableYears { get; set; }

		public IList<User> RegisteredUsers { get; set; }

		public IList<RegisteredUsersChartModel> RegisteredUsersChartModels { get; set; }

		public partial class RegisteredUsersChartModel
		{
			public RegisteredUsersChartModel()
			{
				RegisteredUsers = new List<KeyValuePair<int, string>>();
			}
			public IList<KeyValuePair<int, string>> RegisteredUsers { get; set; }

			public DateTime WeekStart { get; set; }

			public DateTime WeekEnd { get; set; }

		}
		#endregion

		#region User Feedbacks

		public int Feedbacks_SelectedMonth { get; set; }
		public IList<SelectListItem> Feedbacks_AvailableMonths { get; set; }

		public int Feedbacks_SelectedYear { get; set; }
		public IList<SelectListItem> Feedbacks_AvailableYears { get; set; }

		public IList<Feedback> Feedbacks { get; set; }

		public IList<RegisteredFeedbacks> FeedbacksChartModels { get; set; }

		public partial class RegisteredFeedbacks
		{
			public RegisteredFeedbacks()
			{
				Feedbacks = new List<KeyValuePair<int, string>>();
			}
			public IList<KeyValuePair<int, string>> Feedbacks { get; set; }

			public DateTime Feedbacks_WeekStart { get; set; }

			public DateTime Feedbacks_WeekEnd { get; set; }

		}

		#endregion

		#region Active/Inactive Users

		// Active/Inactive users
		public RegisteredActiveInactiveUsers ActiveInactiveUsers { get; set; }

		public partial class RegisteredActiveInactiveUsers
		{
			public RegisteredActiveInactiveUsers()
			{
				ActiveUsers = new List<User>();
				InActiveUsers = new List<User>();
			}
			public IList<User> ActiveUsers { get; set; }
			public IList<User> InActiveUsers { get; set; }

		}

		#endregion

		#region Events

		public int Events_SelectedMonth { get; set; }
		public IList<SelectListItem> Events_AvailableMonths { get; set; }

		public int Events_SelectedYear { get; set; }
		public IList<SelectListItem> Events_AvailableYears { get; set; }

		public IList<Event> Events { get; set; }

		public IList<RegisteredEvents> EventsChartModels { get; set; }

		public partial class RegisteredEvents
		{
			public RegisteredEvents()
			{
				Events = new List<KeyValuePair<int, string>>();
			}
			public IList<KeyValuePair<int, string>> Events { get; set; }

			public DateTime Events_WeekStart { get; set; }

			public DateTime Events_WeekEnd { get; set; }

		}

		#endregion
	}
}