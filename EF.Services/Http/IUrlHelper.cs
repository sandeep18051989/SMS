using System.Collections.Generic;
using System.Web;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Http
{
	public partial interface IUrlHelper
	{
		/// <summary>
		/// Get URL referrer
		/// </summary>
		/// <returns>URL referrer</returns>
		string GetUrlReferrer();

		/// <summary>
		/// Get context IP address
		/// </summary>
		/// <returns>URL referrer</returns>
		string GetCurrentIpAddress();

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <returns>Page name</returns>
		string GetThisPageUrl(bool includeQueryString);

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <param name="useSsl">Value indicating whether to get SSL protected page</param>
		/// <returns>Page name</returns>
		string GetThisPageUrl(bool includeQueryString, bool useSsl);

		/// <summary>
		/// Gets a value indicating whether current connection is secured
		/// </summary>
		/// <returns>true - secured, false - not secured</returns>
		bool IsCurrentConnectionSecured();

		/// <summary>
		/// Gets server variable by name
		/// </summary>
		/// <param name="name">Name</param>
		/// <returns>Server variable</returns>
		string ServerVariables(string name);

		/// <summary>
		/// Gets  host location
		/// </summary>
		/// <param name="useSsl">Use SSL</param>
		/// <returns> host location</returns>
		string GetHost(bool useSsl);

		/// <summary>
		/// Gets  location
		/// </summary>
		/// <returns> location</returns>
		string GetLocation();

		/// <summary>
		/// Gets  location
		/// </summary>
		/// <param name="useSsl">Use SSL</param>
		/// <returns> location</returns>
		string GetLocation(bool useSsl);

		bool IsStaticResource(HttpRequest request);

		/// <summary>
		/// Modifies query string
		/// </summary>
		/// <param name="url">Url to modify</param>
		/// <param name="queryStringModification">Query string modification</param>
		/// <param name="anchor">Anchor</param>
		/// <returns>New url</returns>
		string ModifyQueryString(string url, string queryStringModification, string anchor);

		/// <summary>
		/// Remove query string from url
		/// </summary>
		/// <param name="url">Url to modify</param>
		/// <param name="queryString">Query string to remove</param>
		/// <returns>New url</returns>
		string RemoveQueryString(string url, string queryString);

		/// <summary>
		/// Gets a value that indicates whether the client is being redirected to a new location
		/// </summary>
		bool IsRequestBeingRedirected { get; }

		/// <summary>
		/// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
		/// </summary>
		bool IsPostBeingDone { get; set; }

		
	}
}
