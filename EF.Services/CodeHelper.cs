using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using EF.Services.Service;

namespace EF.Services
{
	/// <summary>
	/// Represents a code helper
	/// </summary>
	public partial class CodeHelper
	{

		/// <summary>
		/// Ensures the subscriber email or throw.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public static string EnsureSubscriberEmailOrThrow(string email)
		{
			string output = EnsureNotNull(email);
			output = output.Trim();
			output = EnsureMaximumLength(output, 255);

			if (!IsValidEmail(output))
			{
				throw new Exception("Email is not valid.");
			}

			return output;
		}

		/// <summary>
		/// Verifies that a string is in valid e-mail format
		/// </summary>
		/// <param name="email">Email to verify</param>
		/// <returns>true if the string is a valid e-mail address and false if it's not</returns>
		public static bool IsValidEmail(string email)
		{
			if (String.IsNullOrEmpty(email))
				return false;

			email = email.Trim();
			var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
			return result;
		}

		/// <summary>
		/// Generate random digit code
		/// </summary>
		/// <param name="length">Length</param>
		/// <returns>Result string</returns>
		public static string GenerateRandomDigitCode(int length)
		{
			var random = new Random();
			string str = string.Empty;
			for (int i = 0; i < length; i++)
				str = String.Concat(str, random.Next(10).ToString());
			return str;
		}

		/// <summary>
		/// Returns a random interger number within a specified rage
		/// </summary>
		/// <param name="min">Minimum number</param>
		/// <param name="max">Maximum number</param>
		/// <returns>Result</returns>
		public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
		{
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
		}

		/// <summary>
		/// Returns a random username within a specified rage
		/// </summary>
		/// <returns>Result</returns>
		public static string GenerateRandomStudentUsername()
		{
			int nextRandomNumber = 0;
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			var settingService = EF.Core.ContextHelper.Current.Resolve<ISettingService>();
			var smsService = EF.Core.ContextHelper.Current.Resolve<ISMSService>();

			var uniQueCodeSetting = settingService.GetSettingByKey("StudentPrefix");
			if (uniQueCodeSetting != null)
			{
				var codeStartSetting = settingService.GetSettingByKey("StudentCodeStart");
				int codeStartFrom = Convert.ToInt32(codeStartSetting.Value);
				nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				while (smsService.CheckUsernameExistsForStudent(uniQueCodeSetting.Value.Trim().ToLower() + nextRandomNumber.ToString()))
				{
					nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				}
			}
			return uniQueCodeSetting.Value.Trim() + nextRandomNumber.ToString();
		}

		/// <summary>
		/// Returns a random username within a specified rage
		/// </summary>
		/// <returns>Result</returns>
		public static string GenerateRandomEmployeeUsername()
		{
			int nextRandomNumber = 0;
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			var settingService = EF.Core.ContextHelper.Current.Resolve<ISettingService>();
			var smsService = EF.Core.ContextHelper.Current.Resolve<ISMSService>();

			var uniQueCodeSetting = settingService.GetSettingByKey("EmployeePrefix");
			if (uniQueCodeSetting != null)
			{
				var codeStartSetting = settingService.GetSettingByKey("EmployeeCodeStart");
				int codeStartFrom = Convert.ToInt32(codeStartSetting.Value);
				nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				while (smsService.CheckUsernameExistsForEmployee(uniQueCodeSetting.Value.Trim().ToLower() + nextRandomNumber.ToString()))
				{
					nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				}
			}
			return uniQueCodeSetting.Value.Trim() + nextRandomNumber.ToString();
		}

		/// <summary>
		/// Returns a random username within a specified rage
		/// </summary>
		/// <returns>Result</returns>
		public static string GenerateRandomSubjectCode()
		{
			int nextRandomNumber = 0;
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			var settingService = EF.Core.ContextHelper.Current.Resolve<ISettingService>();
			var smsService = EF.Core.ContextHelper.Current.Resolve<ISMSService>();

			var uniQueCodeSetting = settingService.GetSettingByKey("SubjectPrefix");
			if (uniQueCodeSetting != null)
			{
				var codeStartSetting = settingService.GetSettingByKey("SubjectCodeStart");
				int codeStartFrom = Convert.ToInt32(codeStartSetting.Value);
				nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				while (smsService.CheckCodeExistsForSubject(uniQueCodeSetting.Value.Trim().ToLower() + nextRandomNumber.ToString()))
				{
					nextRandomNumber = new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(codeStartFrom, int.MaxValue);
				}
			}
			return uniQueCodeSetting.Value.Trim() + nextRandomNumber.ToString();
		}

		/// <summary>
		/// Ensure that a string doesn't exceed maximum allowed length
		/// </summary>
		/// <param name="str">Input string</param>
		/// <param name="maxLength">Maximum length</param>
		/// <param name="postfix">A string to add to the end if the original string was shorten</param>
		/// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
		public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
		{
			if (String.IsNullOrEmpty(str))
				return str;

			if (str.Length > maxLength)
			{
				var result = str.Substring(0, maxLength);
				if (!String.IsNullOrEmpty(postfix))
				{
					result += postfix;
				}
				return result;
			}
			else
			{
				return str;
			}
		}

		/// <summary>
		/// Ensures that a string only contains numeric values
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
		public static string EnsureNumericOnly(string str)
		{
			if (String.IsNullOrEmpty(str))
				return string.Empty;

			var result = new StringBuilder();
			foreach (char c in str)
			{
				if (Char.IsDigit(c))
					result.Append(c);
			}
			return result.ToString();
		}

		/// <summary>
		/// Ensure that a string is not null
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Result</returns>
		public static string EnsureNotNull(string str)
		{
			if (str == null)
				return string.Empty;

			return str;
		}

		/// <summary>
		/// Indicates whether the specified strings are null or empty strings
		/// </summary>
		/// <param name="stringsToValidate">Array of strings to validate</param>
		/// <returns>Boolean</returns>
		public static bool AreNullOrEmpty(params string[] stringsToValidate)
		{
			bool result = false;
			Array.ForEach(stringsToValidate, str =>
			{
				if (string.IsNullOrEmpty(str)) result = true;
			});
			return result;
		}


		private static AspNetHostingPermissionLevel? _trustLevel = null;
		/// <summary>
		/// </summary>
		/// <returns>The current trust level.</returns>
		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			if (!_trustLevel.HasValue)
			{
				//set minimum
				_trustLevel = AspNetHostingPermissionLevel.None;

				//determine maximum
				foreach (AspNetHostingPermissionLevel trustLevel in
						  new AspNetHostingPermissionLevel[] {
										  AspNetHostingPermissionLevel.Unrestricted,
										  AspNetHostingPermissionLevel.High,
										  AspNetHostingPermissionLevel.Medium,
										  AspNetHostingPermissionLevel.Low,
										  AspNetHostingPermissionLevel.Minimal
								})
				{
					try
					{
						new AspNetHostingPermission(trustLevel).Demand();
						_trustLevel = trustLevel;
						break; //we've set the highest permission we can
					}
					catch (System.Security.SecurityException)
					{
						continue;
					}
				}
			}
			return _trustLevel.Value;
		}

		/// <summary>
		/// Convert enum for front-end
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Converted string</returns>
		public static string ConvertEnum(string str)
		{
			string result = string.Empty;
			char[] letters = str.ToCharArray();
			foreach (char c in letters)
				if (c.ToString() != c.ToString().ToLower())
					result += " " + c.ToString();
				else
					result += c.ToString();
			return result;
		}

		/// <summary>
		/// Get Time Ago
		/// </summary>
		/// <param name="date">Input Date</param>
		/// <returns>Result</returns>
		public static string TimeAgo(DateTime date)
		{

			TimeSpan timeSince = DateTime.Now.Subtract(date);

			if (timeSince.TotalMilliseconds < 1)
				return "not yet";
			if (timeSince.TotalMinutes < 1)
				return "just now";
			if (timeSince.TotalMinutes < 2)
				return "1 minute ago";
			if (timeSince.TotalMinutes < 60)
				return string.Format("{0} minutes ago", timeSince.Minutes);
			if (timeSince.TotalMinutes < 120)
				return "1 hour ago";
			if (timeSince.TotalHours < 24)
				return string.Format("{0} hours ago", timeSince.Hours);
			if (timeSince.TotalDays == 1)
				return "yesterday";
			if (timeSince.TotalDays < 7)
				return string.Format("{0} days ago", timeSince.Days);
			if (timeSince.TotalDays < 14)
				return "last week";
			if (timeSince.TotalDays < 21)
				return "2 weeks ago";
			if (timeSince.TotalDays < 28)
				return "3 weeks ago";
			if (timeSince.TotalDays < 60)
				return "last month";
			if (timeSince.TotalDays < 365)
				return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
			if (timeSince.TotalDays < 730)
				return "last year";

			//last but not least...
			return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));

		}

		public static String Replace(String originalString, String oldValue, String newValue, StringComparison comparisonType)
		{
			Int32 startIndex = 0;

			while (true)
			{
				startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);

				if (startIndex < 0)
				{
					break;
				}

				originalString = String.Concat(originalString.Substring(0, startIndex), newValue, originalString.Substring(startIndex + oldValue.Length));

				startIndex += newValue.Length;
			}

			return (originalString);
		}

	}
}

