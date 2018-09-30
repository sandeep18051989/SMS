using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EF.Core;
using EF.Services.Http;

namespace EF.Services
{
	public partial class Pager : IHtmlString
	{
		protected readonly IPageModel model;
		protected readonly ViewContext viewContext;
		protected string pageQueryName = "page";
		protected bool showTotalSummary;
		protected bool showPagerItems = true;
		protected bool showFirst = true;
		protected bool showPrevious = true;
		protected bool showNext = true;
		protected bool showLast = true;
		protected bool showIndividualPages = true;
		protected bool renderEmptyParameters = true;
		protected int individualPagesDisplayedCount = 5;
		protected Func<int, string> urlBuilder;
		protected IList<string> booleanParameterNames;

		public Pager(IPageModel model, ViewContext context)
		{
			this.model = model;
			this.viewContext = context;
			this.urlBuilder = CreateDefaultUrl;
			this.booleanParameterNames = new List<string>();
		}

		protected ViewContext ViewContext
		{
			get { return viewContext; }
		}

		public Pager QueryParam(string value)
		{
			this.pageQueryName = value;
			return this;
		}
		public Pager ShowTotalSummary(bool value)
		{
			this.showTotalSummary = value;
			return this;
		}
		public Pager ShowPagerItems(bool value)
		{
			this.showPagerItems = value;
			return this;
		}
		public Pager ShowFirst(bool value)
		{
			this.showFirst = value;
			return this;
		}
		public Pager ShowPrevious(bool value)
		{
			this.showPrevious = value;
			return this;
		}
		public Pager ShowNext(bool value)
		{
			this.showNext = value;
			return this;
		}
		public Pager ShowLast(bool value)
		{
			this.showLast = value;
			return this;
		}
		public Pager ShowIndividualPages(bool value)
		{
			this.showIndividualPages = value;
			return this;
		}
		public Pager RenderEmptyParameters(bool value)
		{
			this.renderEmptyParameters = value;
			return this;
		}
		public Pager IndividualPagesDisplayedCount(int value)
		{
			this.individualPagesDisplayedCount = value;
			return this;
		}
		public Pager Link(Func<int, string> value)
		{
			this.urlBuilder = value;
			return this;
		}
		public Pager BooleanParameterName(string paramName)
		{
			booleanParameterNames.Add(paramName);
			return this;
		}

		public override string ToString()
		{
			return ToHtmlString();
		}
		public virtual string ToHtmlString()
		{
			if (model.TotalItems == 0)
				return null;

			var links = new StringBuilder();
			if (showTotalSummary && (model.TotalPages > 0))
			{
				links.Append("<li class=\"total-summary\">");
				//Page {0} of {1} ({2} total)
				links.Append(string.Format("Page {0} of {1} ({2} total)", model.PageIndex + 1, model.TotalPages, model.TotalItems));
				links.Append("</li>");
			}
			if (showPagerItems && (model.TotalPages > 1))
			{
				if (showFirst)
				{
					if ((model.PageIndex >= 3) && (model.TotalPages > individualPagesDisplayedCount))
					{
						links.Append(CreatePageLink(1, "first-page", "first-page"));
					}
				}
				if (showPrevious)
				{
					if (model.PageIndex > 0)
					{
						links.Append(CreatePageLink(model.PageIndex, "Pager.Previous", "previous-page"));
					}
				}
				if (showIndividualPages)
				{
					int firstIndividualPageIndex = GetFirstIndividualPageIndex();
					int lastIndividualPageIndex = GetLastIndividualPageIndex();
					for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
					{
						if (model.PageIndex == i)
						{
							links.AppendFormat("<li class=\"current-page\"><span>{0}</span></li>", (i + 1));
						}
						else
						{
							links.Append(CreatePageLink(i + 1, (i + 1).ToString(), "individual-page"));
						}
					}
				}
				if (showNext)
				{
					if ((model.PageIndex + 1) < model.TotalPages)
					{
						links.Append(CreatePageLink(model.PageIndex + 2, "Pager.Next", "next-page"));
					}
				}
				if (showLast)
				{
					//last page
					if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > individualPagesDisplayedCount))
					{
						links.Append(CreatePageLink(model.TotalPages, "Pager.Last", "last-page"));
					}
				}
			}

			var result = links.ToString();
			if (!String.IsNullOrEmpty(result))
			{
				result = "<ul>" + result + "</ul>";
			}
			return result;
		}
		public virtual bool IsEmpty()
		{
			var html = ToString();
			return string.IsNullOrEmpty(html);
		}

		protected virtual int GetFirstIndividualPageIndex()
		{
			if ((model.TotalPages < individualPagesDisplayedCount) ||
				 ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0))
			{
				return 0;
			}
			if ((model.PageIndex + (individualPagesDisplayedCount / 2)) >= model.TotalPages)
			{
				return (model.TotalPages - individualPagesDisplayedCount);
			}
			return (model.PageIndex - (individualPagesDisplayedCount / 2));
		}
		protected virtual int GetLastIndividualPageIndex()
		{
			int num = individualPagesDisplayedCount / 2;
			if ((individualPagesDisplayedCount % 2) == 0)
			{
				num--;
			}
			if ((model.TotalPages < individualPagesDisplayedCount) ||
				 ((model.PageIndex + num) >= model.TotalPages))
			{
				return (model.TotalPages - 1);
			}
			if ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0)
			{
				return (individualPagesDisplayedCount - 1);
			}
			return (model.PageIndex + num);
		}
		protected virtual string CreatePageLink(int pageNumber, string text, string cssClass)
		{
			var liBuilder = new TagBuilder("li");
			if (!String.IsNullOrWhiteSpace(cssClass))
				liBuilder.AddCssClass(cssClass);

			var aBuilder = new TagBuilder("a");
			aBuilder.SetInnerText(text);
			aBuilder.MergeAttribute("href", urlBuilder(pageNumber));

			liBuilder.InnerHtml += aBuilder;

			return liBuilder.ToString(TagRenderMode.Normal);
		}
		protected virtual string CreateDefaultUrl(int pageNumber)
		{
			var routeValues = new RouteValueDictionary();

			var parametersWithEmptyValues = new List<string>();
			foreach (var key in viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
			{
				var value = viewContext.RequestContext.HttpContext.Request.QueryString[key];
				if (renderEmptyParameters && String.IsNullOrEmpty(value))
				{
					parametersWithEmptyValues.Add(key);
				}
				else
				{
					if (booleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
					{
						if (!String.IsNullOrEmpty(value) && value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
						{
							value = "true";
						}
					}
					routeValues[key] = value;
				}
			}

			if (pageNumber > 1)
			{
				routeValues[pageQueryName] = pageNumber;
			}
			else
			{
				if (routeValues.ContainsKey(pageQueryName))
				{
					routeValues.Remove(pageQueryName);
				}
			}

			var url = System.Web.Mvc.UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes, viewContext.RequestContext, true);
			if (renderEmptyParameters && parametersWithEmptyValues.Any())
			{
				var webHelper = ContextHelper.Current.Resolve<IUrlHelper>();
				foreach (var key in parametersWithEmptyValues)
				{
					url = webHelper.ModifyQueryString(url, key + "=", null);
				}
			}
			return url;
		}
	}
}
