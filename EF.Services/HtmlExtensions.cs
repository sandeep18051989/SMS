﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;
//using System.Web.WebPages;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EF.Core.Data;
using EF.Services.Service;

namespace EF.Services
{
    public static class HtmlExtensions
	{
		[ThreadStatic]
		private static StringBuilder m_ReplaceSB;

		#region Admin area extensions
		public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector) where T : BaseEntityModel
		{
			return DeleteConfirmation(helper, "", buttonsSelector);
		}

		public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
			 string buttonsSelector) where T : BaseEntityModel
		{
			if (String.IsNullOrEmpty(actionName))
				actionName = "Delete";

			var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation")
				 .ToHtmlString();

			var deleteConfirmationModel = new ConfirmationForDeleteModel
			{
				Id = helper.ViewData.Model.Id,
				ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
				ActionName = actionName,
				WindowId = modalId
			};

			var window = new StringBuilder();
			window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
			window.AppendLine(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString());
			window.AppendLine("</div>");

			window.AppendLine("<script>");
			window.AppendLine("$(document).ready(function() {");
			window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
			window.AppendLine("});");
			window.AppendLine("</script>");

			return MvcHtmlString.Create(window.ToString());
		}

		public static MvcHtmlString ActionConfirmation(this HtmlHelper helper, string buttonId, string actionName = "")
		{
			if (string.IsNullOrEmpty(actionName))
				actionName = helper.ViewContext.RouteData.GetRequiredString("action");

			var modalId = MvcHtmlString.Create(buttonId + "-action-confirmation").ToHtmlString();

			var actionConfirmationModel = new ActionConfirmationModel()
			{
				ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
				ActionName = actionName,
				WindowId = modalId
			};

			var window = new StringBuilder();
			window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
			window.AppendLine(helper.Partial("Confirm", actionConfirmationModel).ToHtmlString());
			window.AppendLine("</div>");

			window.AppendLine("<script>");
			window.AppendLine("$(document).ready(function() {");
			window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\");", buttonId, modalId));
			window.AppendLine(string.Format("$('#{0}-submit-button').attr(\"name\", $(\"#{1}\").attr(\"name\"));", modalId, buttonId));
			window.AppendLine(string.Format("$(\"#{0}\").attr(\"name\", \"\")", buttonId));
			window.AppendLine(string.Format("if($(\"#{0}\").attr(\"type\") == \"submit\")$(\"#{0}\").attr(\"type\", \"button\")", buttonId));
			window.AppendLine("});");
			window.AppendLine("</script>");

			return MvcHtmlString.Create(window.ToString());
		}

		#region Form fields

		public static MvcHtmlString Hint(this HtmlHelper helper, string value)
		{
			//create tag builder
			var builder = new TagBuilder("div");
			builder.MergeAttribute("title", value);
			builder.MergeAttribute("class", "ico-help");
			var icon = new StringBuilder();
			icon.Append("<i class='fa fa-question-circle'></i>");
			builder.InnerHtml = icon.ToString();
			//render tag
			return MvcHtmlString.Create(builder.ToString());
		}

		public static MvcHtmlString CustomEditorFor<TModel, TValue>(this HtmlHelper<TModel> helper,
			 Expression<Func<TModel, TValue>> expression, bool? renderFormControlClass = null)
		{
			var result = new StringBuilder();

			object htmlAttributes = null;
			var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
			if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
				 (renderFormControlClass.HasValue && renderFormControlClass.Value))
				htmlAttributes = new { @class = "form-control" };

			result.Append(helper.EditorFor(expression, new { htmlAttributes }));

			return MvcHtmlString.Create(result.ToString());
		}

		public static MvcHtmlString CustomDropDownList<TModel>(this HtmlHelper<TModel> helper, string name,
			 IEnumerable<SelectListItem> itemList, object htmlAttributes = null, bool renderFormControlClass = true)
		{
			var result = new StringBuilder();

			var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			if (renderFormControlClass)
				attrs = AddFormControlClassToHtmlAttributes(attrs);

			result.Append(helper.DropDownList(name, itemList, attrs));

			return MvcHtmlString.Create(result.ToString());
		}

		public static MvcHtmlString CustomDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper,
			 Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> itemList,
			 object htmlAttributes = null, bool renderFormControlClass = true)
		{
			var result = new StringBuilder();

			var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			if (renderFormControlClass)
				attrs = AddFormControlClassToHtmlAttributes(attrs);

			result.Append(helper.DropDownListFor(expression, itemList, attrs));

			return MvcHtmlString.Create(result.ToString());
		}

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper,
                                                                                    Expression<Func<TModel, TProperty>> expression,
                                                                                    TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                                        .Cast<TEnum>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem()
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString(),
                                                    Selected = (value.Equals(selectedValue))
                                                };
            return SelectExtensions.DropDownListFor(htmlHelper, expression, items);
        }

        public static MvcHtmlString CustomTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> helper,
			 Expression<Func<TModel, TValue>> expression, object htmlAttributes = null,
			 bool renderFormControlClass = true, int rows = 4, int columns = 20)
		{
			var result = new StringBuilder();

			var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			if (renderFormControlClass)
				attrs = AddFormControlClassToHtmlAttributes(attrs);

			result.Append(helper.TextAreaFor(expression, rows, columns, attrs));

			return MvcHtmlString.Create(result.ToString());
		}

		public static Pager Pager(this HtmlHelper helper, IPageModel pager)
		{
			return new Pager(pager, helper.ViewContext);
		}

		public static MvcHtmlString CustomDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
		{
			var result = new TagBuilder("div");
			result.Attributes.Add("class", "form-text-row");
			result.InnerHtml = helper.DisplayFor(expression).ToString();

			return MvcHtmlString.Create(result.ToString());
		}

		public static MvcHtmlString CustomDisplay<TModel>(this HtmlHelper<TModel> helper, string expression)
		{
			var result = new TagBuilder("div");
			result.Attributes.Add("class", "form-text-row");
			result.InnerHtml = expression;

			return MvcHtmlString.Create(result.ToString());
		}

		public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
				htmlAttributes["class"] = "form-control";
			else
				 if (!htmlAttributes["class"].ToString().Contains("form-control"))
				htmlAttributes["class"] += " form-control";

			return htmlAttributes as RouteValueDictionary;
		}

		#endregion

		#endregion

		#region Common extensions

		private static StringBuilder GetReplaceSB(int capacity)
		{
			var result = m_ReplaceSB;

			if (null == result)
			{
				result = new StringBuilder(capacity);
				m_ReplaceSB = result;
			}
			else
			{
				result.Clear();
				result.EnsureCapacity(capacity);
			}

			return result;
		}

		public static string ReplaceAny(this string s, char replaceWith, params char[] chars)
		{
			if (null == chars)
				return s;

			if (null == s)
				return null;

			StringBuilder sb = null;

			for (int i = 0, count = s.Length; i < count; i++)
			{
				var temp = s[i];
				var replace = false;

				for (int j = 0, cc = chars.Length; j < cc; j++)
					if (temp == chars[j])
					{
						if (null == sb)
						{
							sb = GetReplaceSB(count);
							if (i > 0)
								sb.Append(s, 0, i);
						}

						replace = true;
						break;
					}

				if (replace)
					sb.Append(replaceWith);
				else
					 if (null != sb)
					sb.Append(temp);
			}

			return null == sb ? s : sb.ToString();
		}

		public static MvcHtmlString RequiredHint(this HtmlHelper helper, string additionalText = null)
		{
			// Create tag builder
			var builder = new TagBuilder("span");
			builder.AddCssClass("required");
			var innerText = "*";
			//add additional text if specified
			if (!String.IsNullOrEmpty(additionalText))
				innerText += " " + additionalText;
			builder.SetInnerText(innerText);
			// Render tag
			return MvcHtmlString.Create(builder.ToString());
		}

		public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
		{
			return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
		}
		public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
		{
			var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
			// because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
			return id.Replace('[', '_').Replace(']', '_');
		}

		/// <summary>
		/// Renders the standard label with a specified suffix added to label text
		/// </summary>
		/// <typeparam name="TModel">Model</typeparam>
		/// <typeparam name="TValue">Value</typeparam>
		/// <param name="html">HTML helper</param>
		/// <param name="expression">Expression</param>
		/// <param name="htmlAttributes">HTML attributes</param>
		/// <param name="suffix">Suffix</param>
		/// <returns>Label</returns>
		public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string suffix)
		{
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			string resolvedLabelText = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new[] { '.' }).Last());
			if (string.IsNullOrEmpty(resolvedLabelText))
			{
				return MvcHtmlString.Empty;
			}
			var tag = new TagBuilder("label");
			tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName)));
			if (!String.IsNullOrEmpty(suffix))
			{
				resolvedLabelText = String.Concat(resolvedLabelText, suffix);
			}
			tag.SetInnerText(resolvedLabelText);

			var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			tag.MergeAttributes(dictionary, true);

			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

        #endregion

        #region Breadcrumbs

        public static string BuildBreadcrumbNavigation(this HtmlHelper helper)
        {
            // optional condition: I didn't wanted it to show on home and account controller
            if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Dashboard" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
            {
                return string.Empty;
            }

            StringBuilder breadcrumb = new StringBuilder("<ol class='breadcrumb'>").Append("<li class='breadcrumb-item'>").Append(helper.RouteLink("Home" , new { Area = "" }).ToHtmlString()).Append("</li>").Append("<li class='breadcrumb-item'>").Append(helper.RouteLink("Dashboard", "AdminDashboard", null).ToHtmlString()).Append("</li>");

            if (helper.ViewContext.RouteData.Values["action"].ToString() == "List")
            {
                breadcrumb.Append("<li class='breadcrumb-item'>");

                var pluralWord = helper.ViewContext.RouteData.Values["controller"].ToString().Split(' ');
                string lastWord = pluralWord[pluralWord.Length - 1];

                breadcrumb.Append(helper.ActionLink(((lastWord == "e") ? helper.ViewContext.RouteData.Values["controller"].ToString() : helper.ViewContext.RouteData.Values["controller"].ToString() + "s"),
                                                    helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                                                    helper.ViewContext.RouteData.Values["controller"].ToString()));
                breadcrumb.Append("</li>");
            }
            else if (helper.ViewContext.RouteData.Values["action"].ToString() == "Create")
            {
                breadcrumb.Append("<li class='breadcrumb-item'>");
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                                                    helper.ViewContext.RouteData.Values["controller"].ToString()));
                breadcrumb.Append("</li>");
            }
            else if (helper.ViewContext.RouteData.Values["action"].ToString() == "Edit")
            {
            }
            else
            {
                breadcrumb.Append("<li class='breadcrumb-item'>");
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                                                    helper.ViewContext.RouteData.Values["controller"].ToString()));
                breadcrumb.Append("</li>");
            }

            return breadcrumb.Append("</ol>").ToString();
        }

        #endregion

        #region Fron-End Extensions

        public static MvcHtmlString ReactionModal<T>(this HtmlHelper<T> helper, string buttonsSelector, int id, bool IsAuthenticated, int UserId, bool? IsEvent=null, bool? IsNews = null, bool? IsComment=null, bool? IsReply=null, bool? IsProduct = null, bool? IsBlog = null) where T : BaseEntityModel
        {
            return ReactionModal(helper, "", buttonsSelector, id, IsAuthenticated, UserId, IsEvent, IsNews, IsComment, IsReply, IsProduct, IsBlog);
        }

        public static MvcHtmlString ReactionModal<T>(this HtmlHelper<T> helper, string actionName,
             string buttonsSelector, int id, bool IsAuthenticated, int UserId, bool? IsEvent = null, bool? IsNews = null, bool? IsComment = null, bool? IsReply = null, bool? IsProduct = null, bool? IsBlog = null) where T : BaseEntityModel
        {
            if (String.IsNullOrEmpty(actionName)) {
                actionName = "Reaction";
            }

            var modalId = MvcHtmlString.Create("reaction-confirmation-" + id.ToString()).ToHtmlString();
            var reactionService = EF.Core.ContextHelper.Current.Resolve<ISMSService>();
            var reactionModel = new ConfirmationReactionModel
            {
                Id = id,
                ControllerName = (IsEvent.HasValue ? "Event" : IsNews.HasValue ? "News" : IsComment.HasValue ? "Comment" : IsReply.HasValue ? "Reply" : IsProduct.HasValue ? "Product" : IsBlog.HasValue ? "Blog" : ""),
                ActionName = actionName,
                WindowId = modalId,
                IsAuthenticated = IsAuthenticated,
                UserId = UserId,
                Reactions = IsAuthenticated ? (IsEvent.HasValue ? reactionService.SearchReactions(eventid: id, userid: UserId) : IsNews.HasValue ? reactionService.SearchReactions(newsid: id, userid: UserId) : IsComment.HasValue ? reactionService.SearchReactions(commentid: id, userid: UserId) : IsReply.HasValue ? reactionService.SearchReactions(replyid: id, userid: UserId) : IsProduct.HasValue ? reactionService.SearchReactions(productid: id, userid: UserId) : IsBlog.HasValue ? reactionService.SearchReactions(blogid: id, userid: UserId) : new List<Reaction>()) : new List<Reaction>()
            };

            var window = new StringBuilder();
            window.AppendLine($"<div id='{modalId}' class='modal fade' tabindex='-1' role='dialog' aria-labelledby='{modalId}-title' aria-hidden='true'>");
            window.AppendLine(helper.Partial("Reaction", reactionModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        #endregion
    }
}
