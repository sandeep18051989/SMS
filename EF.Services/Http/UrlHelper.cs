using EF.Core;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EF.Services.Context;
using EF.Core.Data;

namespace EF.Services.Http
{
	public partial class UrlHelper : IUrlHelper
	{
		#region Fields 

		private readonly HttpContextBase _httpContext;
		private readonly string[] _staticFileExtensions;
		private static readonly object s_lock = new object();

		#endregion

		#region Constructor

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="httpContext">HTTP context</param>
		public UrlHelper(HttpContextBase httpContext)
		{
			this._httpContext = httpContext;
			this._staticFileExtensions = new[] { ".axd", ".ashx", ".bmp", ".css", ".gif", ".htm", ".html", ".ico", ".jpeg", ".jpg", ".js", ".png", ".rar", ".zip" };
		}

		#endregion

		#region Utilities

		protected virtual Boolean IsRequestAvailable(HttpContextBase httpContext)
		{
			if (httpContext == null)
				return false;

			try
			{
				if (httpContext.Request == null)
					return false;
			}
			catch (HttpException)
			{
				return false;
			}

			return true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get URL referrer
		/// </summary>
		/// <returns>URL referrer</returns>
		public virtual string GetUrlReferrer()
		{
			string referrerUrl = string.Empty;

			//URL referrer is null in some case (for example, in IE 8)
			if (IsRequestAvailable(_httpContext) && _httpContext.Request.UrlReferrer != null)
				referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

			return referrerUrl;
		}

		/// <summary>
		/// Get context IP address
		/// </summary>
		/// <returns>URL referrer</returns>
		public virtual string GetCurrentIpAddress()
		{
			if (!IsRequestAvailable(_httpContext))
				return string.Empty;

			var result = "";
			if (_httpContext.Request.Headers != null)
			{
				//The X-Forwarded-For (XFF) HTTP header field is a de facto standard
				//for identifying the originating IP address of a client
				//connecting to a web server through an HTTP proxy or load balancer.
				var forwardedHttpHeader = "X-FORWARDED-FOR";
				if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ForwardedHTTPheader"]))
				{
					//but in some cases server use other HTTP header
					//in these cases an administrator can specify a custom Forwarded HTTP header
					//e.g. CF-Connecting-IP, X-FORWARDED-PROTO, etc
					forwardedHttpHeader = ConfigurationManager.AppSettings["ForwardedHTTPheader"];
				}

				//it's used for identifying the originating IP address of a client connecting to a web server
				//through an HTTP proxy or load balancer. 
				string xff = _httpContext.Request.Headers.AllKeys
					 .Where(x => forwardedHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
					 .Select(k => _httpContext.Request.Headers[k])
					 .FirstOrDefault();

				if (!String.IsNullOrEmpty(xff))
				{
					string lastIp = xff.Split(new[] { ',' }).FirstOrDefault();
					result = lastIp;
				}
			}

			if (String.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
			{
				result = _httpContext.Request.UserHostAddress;
			}

			//some validation
			if (result == "::1")
				result = "127.0.0.1";
			//remove port
			if (!String.IsNullOrEmpty(result))
			{
				int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
				if (index > 0)
					result = result.Substring(0, index);
			}
			return result;

		}

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <returns>Page name</returns>
		public virtual string GetThisPageUrl(bool includeQueryString)
		{
			bool useSsl = IsCurrentConnectionSecured();
			return GetThisPageUrl(includeQueryString, useSsl);
		}

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <param name="useSsl">Value indicating whether to get SSL protected page</param>
		/// <returns>Page name</returns>
		public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
		{
			string url = string.Empty;
			if (!IsRequestAvailable(_httpContext))
				return url;

			if (includeQueryString)
			{
				string Host = GetHost(useSsl);
				if (Host.EndsWith("/"))
					Host = Host.Substring(0, Host.Length - 1);
				url = Host + _httpContext.Request.RawUrl;
			}
			else
			{
				if (_httpContext.Request.Url != null)
				{
					url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
				}
			}
			url = url.ToLowerInvariant();
			return url;
		}

		/// <summary>
		/// Gets a value indicating whether current connection is secured
		/// </summary>
		/// <returns>true - secured, false - not secured</returns>
		public virtual bool IsCurrentConnectionSecured()
		{
			bool useSsl = false;
			if (IsRequestAvailable(_httpContext))
			{
				//when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true

				//1. use HTTP_CLUSTER_HTTPS?
				if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Use_HTTP_CLUSTER_HTTPS"]) &&
					Convert.ToBoolean(ConfigurationManager.AppSettings["Use_HTTP_CLUSTER_HTTPS"]))
				{
					useSsl = ServerVariables("HTTP_CLUSTER_HTTPS") == "on";
				}
				//2. use HTTP_X_FORWARDED_PROTO?
				else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Use_HTTP_X_FORWARDED_PROTO"]) &&
					Convert.ToBoolean(ConfigurationManager.AppSettings["Use_HTTP_X_FORWARDED_PROTO"]))
				{
					useSsl = string.Equals(ServerVariables("HTTP_X_FORWARDED_PROTO"), "https", StringComparison.OrdinalIgnoreCase);
				}
				else
				{
					useSsl = _httpContext.Request.IsSecureConnection;
				}
			}

			return useSsl;
		}

		/// <summary>
		/// Gets server variable by name
		/// </summary>
		/// <param name="name">Name</param>
		/// <returns>Server variable</returns>
		public virtual string ServerVariables(string name)
		{
			string result = string.Empty;

			try
			{
				if (!IsRequestAvailable(_httpContext))
					return result;

				if (_httpContext.Request.ServerVariables[name] != null)
				{
					result = _httpContext.Request.ServerVariables[name];
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		/// <summary>
		/// Gets  host location
		/// </summary>
		/// <param name="useSsl">Use SSL</param>
		/// <returns> host location</returns>
		public virtual string GetHost(bool useSsl)
		{
			var result = "";
			var httpHost = ServerVariables("HTTP_HOST");
			if (!String.IsNullOrEmpty(httpHost))
			{
				result = "http://" + httpHost;
				if (!result.EndsWith("/"))
					result += "/";
			}

			if (DatabaseHelper.DatabaseIsInstalled())
			{
				#region Database is installed

				//let's resolve IWorkContext  here.
				//Do not inject it via constructor  because it'll cause circular references
				var webContext = ContextHelper.Current.Resolve<IWebContext>();
				var currentStoreUrl = webContext.CurrentStoreUrl;
				if (String.IsNullOrEmpty(currentStoreUrl))
					throw new Exception("Website Host Url Not Found");

				if (String.IsNullOrWhiteSpace(httpHost))
				{
					//HTTP_HOST variable is not available.
					//This scenario is possible only when HttpContext is not available (for example, running in a schedule task)
					//in this case use URL of a store entity configured in admin area
					result = currentStoreUrl;
					if (!result.EndsWith("/"))
						result += "/";
				}
				#endregion
			}
			else
			{
				#region Database is not installed
				if (useSsl)
				{
					//Secure URL is not specified.
					//So a store owner wants it to be detected automatically.
					result = result.Replace("http:/", "https:/");
				}
				#endregion
			}

			if (!result.EndsWith("/"))
				result += "/";
			return result.ToLowerInvariant();
		}

		/// <summary>
		/// Gets  location
		/// </summary>
		/// <returns> location</returns>
		public virtual string GetLocation()
		{
			bool useSsl = IsCurrentConnectionSecured();
			return GetLocation(useSsl);
		}

		/// <summary>
		/// Gets  location
		/// </summary>
		/// <param name="useSsl">Use SSL</param>
		/// <returns> location</returns>
		public virtual string GetLocation(bool useSsl)
		{
			//return HostingEnvironment.ApplicationVirtualPath;

			string result = GetHost(useSsl);
			if (result.EndsWith("/"))
				result = result.Substring(0, result.Length - 1);
			if (IsRequestAvailable(_httpContext))
				result = result + _httpContext.Request.ApplicationPath;
			if (!result.EndsWith("/"))
				result += "/";

			return result.ToLowerInvariant();
		}

		/// <summary>
		/// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
		/// </summary>
		/// <param name="request">HTTP Request</param>
		/// <returns>True if the request targets a static resource file.</returns>
		/// <remarks>
		/// These are the file extensions considered to be static resources:
		/// .css
		///	.gif
		/// .png 
		/// .jpg
		/// .jpeg
		/// .js
		/// .axd
		/// .ashx
		/// </remarks>
		public virtual bool IsStaticResource(HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			string path = request.Path;
			string extension = VirtualPathUtility.GetExtension(path);

			if (extension == null) return false;

			return _staticFileExtensions.Contains(extension);
		}

		public virtual string ModifyQueryString(string url, string queryStringModification, string anchor)
		{
			if (url == null)
				url = string.Empty;
			url = url.ToLowerInvariant();

			if (queryStringModification == null)
				queryStringModification = string.Empty;
			queryStringModification = queryStringModification.ToLowerInvariant();

			if (anchor == null)
				anchor = string.Empty;
			anchor = anchor.ToLowerInvariant();


			string str = string.Empty;
			string str2 = string.Empty;
			if (url.Contains("#"))
			{
				str2 = url.Substring(url.IndexOf("#") + 1);
				url = url.Substring(0, url.IndexOf("#"));
			}
			if (url.Contains("?"))
			{
				str = url.Substring(url.IndexOf("?") + 1);
				url = url.Substring(0, url.IndexOf("?"));
			}
			if (!string.IsNullOrEmpty(queryStringModification))
			{
				if (!string.IsNullOrEmpty(str))
				{
					var dictionary = new Dictionary<string, string>();
					foreach (string str3 in str.Split(new[] { '&' }))
					{
						if (!string.IsNullOrEmpty(str3))
						{
							string[] strArray = str3.Split(new[] { '=' });
							if (strArray.Length == 2)
							{
								if (!dictionary.ContainsKey(strArray[0]))
								{
									dictionary[strArray[0]] = strArray[1];
								}
							}
							else
							{
								dictionary[str3] = null;
							}
						}
					}
					foreach (string str4 in queryStringModification.Split(new[] { '&' }))
					{
						if (!string.IsNullOrEmpty(str4))
						{
							string[] strArray2 = str4.Split(new[] { '=' });
							if (strArray2.Length == 2)
							{
								dictionary[strArray2[0]] = strArray2[1];
							}
							else
							{
								dictionary[str4] = null;
							}
						}
					}
					var builder = new StringBuilder();
					foreach (string str5 in dictionary.Keys)
					{
						if (builder.Length > 0)
						{
							builder.Append("&");
						}
						builder.Append(str5);
						if (dictionary[str5] != null)
						{
							builder.Append("=");
							builder.Append(dictionary[str5]);
						}
					}
					str = builder.ToString();
				}
				else
				{
					str = queryStringModification;
				}
			}
			if (!string.IsNullOrEmpty(anchor))
			{
				str2 = anchor;
			}
			return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2))).ToLowerInvariant();
		}

		/// <summary>
		/// Remove query string from url
		/// </summary>
		/// <param name="url">Url to modify</param>
		/// <param name="queryString">Query string to remove</param>
		/// <returns>New url</returns>
		public virtual string RemoveQueryString(string url, string queryString)
		{
			if (url == null)
				url = string.Empty;
			url = url.ToLowerInvariant();

			if (queryString == null)
				queryString = string.Empty;
			queryString = queryString.ToLowerInvariant();


			string str = string.Empty;
			if (url.Contains("?"))
			{
				str = url.Substring(url.IndexOf("?") + 1);
				url = url.Substring(0, url.IndexOf("?"));
			}
			if (!string.IsNullOrEmpty(queryString))
			{
				if (!string.IsNullOrEmpty(str))
				{
					var dictionary = new Dictionary<string, string>();
					foreach (string str3 in str.Split(new[] { '&' }))
					{
						if (!string.IsNullOrEmpty(str3))
						{
							string[] strArray = str3.Split(new[] { '=' });
							if (strArray.Length == 2)
							{
								dictionary[strArray[0]] = strArray[1];
							}
							else
							{
								dictionary[str3] = null;
							}
						}
					}
					dictionary.Remove(queryString);

					var builder = new StringBuilder();
					foreach (string str5 in dictionary.Keys)
					{
						if (builder.Length > 0)
						{
							builder.Append("&");
						}
						builder.Append(str5);
						if (dictionary[str5] != null)
						{
							builder.Append("=");
							builder.Append(dictionary[str5]);
						}
					}
					str = builder.ToString();
				}
			}
			return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
		}

		/// <summary>
		/// Gets a value that indicates whether the client is being redirected to a new location
		/// </summary>
		public virtual bool IsRequestBeingRedirected
		{
			get
			{
				var response = _httpContext.Response;
				return response.IsRequestBeingRedirected;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
		/// </summary>
		public virtual bool IsPostBeingDone
		{
			get
			{
				if (_httpContext.Items["cms.IsPOSTBeingDone"] == null)
					return false;
				return Convert.ToBoolean(_httpContext.Items["cms.IsPOSTBeingDone"]);
			}
			set
			{
				_httpContext.Items["cms.IsPOSTBeingDone"] = value;
			}
		}

		#endregion

	}
}
