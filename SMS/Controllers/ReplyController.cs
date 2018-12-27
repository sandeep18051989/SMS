using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class ReplyController : PublicHttpController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IProductService _productService;
		private readonly IEventService _eventService;
		private readonly IEmailService _emailService;
		private readonly ITemplateService _templateService;

		#endregion Fileds

		#region Constructor

		public ReplyController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IProductService productService, IEventService eventService, IEmailService emailService, ITemplateService templateService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._productService = productService;
			this._eventService = eventService;
			this._emailService = emailService;
			this._templateService = templateService;
		}

		#endregion

		#region Utilities
		public string GenerateSlug(string ProductId, string Name)
		{
			string phrase = string.Format("{0}-{1}", ProductId, Name);

			string str = RemoveAccent(phrase).ToLower();
			// invalid chars           
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces into one space   
			str = Regex.Replace(str, @"\s+", " ").Trim();
			// cut and trim 
			str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
			str = Regex.Replace(str, @"\s", "-"); // hyphens   
			return str;
		}

		private string RemoveAccent(string text)
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
			return System.Text.Encoding.ASCII.GetString(bytes);
		}

		#endregion


		[HttpPost]
		[ValidateInput(false)]
		public ActionResult PostReply(FormCollection frm)
		{
			TempData.Clear();
			var user = _userContext.CurrentUser;
			var model = new PostReplyModel();
			// Get Email Settings for Use
			var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
			if (frm["CommentId"] != null)
			{
				var _comment = _commentService.GetCommentById(Convert.ToInt32(frm["CommentId"].ToString()));
				if (_comment != null)
				{
					model.ReplyHtml = frm["postReplyModel.ReplyHtml"] != null ? frm["postReplyModel.ReplyHtml"].ToString() : "";
					model.CreatedOn = DateTime.Now;
					model.IsModified = false;
					model.Username = frm["Username"] != null ? frm["Username"].ToString() : "";
					model.CommentId = frm["CommentId"] != null ? Convert.ToInt32(frm["CommentId"].ToString()) : 0;
					model.EntityId = frm["ProductId"] != null ? Convert.ToInt32(frm["ProductId"].ToString()) : 0;
					model.Type = frm["postReplyModel.Type"] != null ? frm["postReplyModel.Type"].ToString() : "";

					var _reply = new Reply()
					{
						ReplyHtml = model.ReplyHtml,
						CreatedOn = DateTime.Now,
						IsActive = true,
						IsDeleted = false,
						DisplayOrder = _replyService.GetAllRepliesByComment(_comment.Id).Count + 1,
						IsModified = false,
						ModifiedOn = DateTime.Now,
						UserId = user != null ? user.Id : 0,
						CommentId = _comment.Id
					};

					_replyService.Insert(_reply);
					_commentService.Update(_comment);

					// Send Notification To The Admin
					if (_settings.Count > 0)
					{
						var template = _settingService.GetSettingByKey("ReplyOnComment");
						var Template = _templateService.GetTemplateByName(template.Value);
						if (Template != null)
						{
							var tokens = new List<DataToken>();
							if (_userContext.CurrentUser != null)
							{
								_templateService.AddUserTokens(tokens, _userContext.CurrentUser);
							}

							// Replace Dynamic Data
							if (model.EntityId > 0)
							{
								var _entityProduct = _productService.GetProductById(model.EntityId);
								Template.BodyHtml = Template.BodyHtml.Replace("[CommentMessage]", _comment.CommentHtml);
								Template.BodyHtml = Template.BodyHtml.Replace("[CommentUsername]", _comment.Username);
								Template.BodyHtml = Template.BodyHtml.Replace("[CommentReplyMessage]", model.ReplyHtml);
								Template.BodyHtml = Template.BodyHtml.Replace("[CommentReplyUsername]", model.Username);
							}

							foreach (var dt in tokens)
							{
								Template.BodyHtml = EF.Services.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.SystemName + "]", dt.Value, StringComparison.InvariantCulture);
							}

							var adminEmail = _settingService.GetSettingByKey("FromEmail");
							if (adminEmail != null)
							{
								if (!String.IsNullOrEmpty(adminEmail.Value))
									_emailService.SendMailUsingTemplate(adminEmail.Value, model.Username + "Posted A Reply", Template);
							}
						}
					}

					SuccessNotification("Your reply is published now!");
					switch (model.Type)
					{
						case "Blog":
							return RedirectToAction("Detail", "Blog", new { id = GenerateSlug(model.EntityId.ToString(), _blogService.GetBlogById(model.EntityId).Name) });
						case "Event":
							return RedirectToAction("Detail", "Event", new { id = GenerateSlug(model.EntityId.ToString(), _eventService.GetEventById(model.EntityId).Title) });
						case "Product":
							return RedirectToAction("Detail", "Product", new { id = GenerateSlug(model.EntityId.ToString(), _productService.GetProductById(model.EntityId).Name) });
						default:
							break;
					}
				}
			}
			else
			{
				return RedirectToAction("Detail", "Product", new { id = GenerateSlug(model.EntityId.ToString(), _productService.GetProductById(model.EntityId).Name) });
			}

			return View(model);
		}

		[ValidateInput(false)]
		public PartialViewResult GetComments(int id, string type)
		{
			var user = _userContext.CurrentUser;
			var model = new List<CommentModel>();
			if (id > 0)
			{
				switch (type)
				{
					case "Blog":
						if (id > 0)
						{
							var _blog = _blogService.GetBlogById(id);
							if (_blog != null)
							{
								if (_blog.Comments.Count > 0)
								{
									foreach (var comment in _blog.Comments)
									{
										var comm = new CommentModel()
										{
											CommentHtml = comment.CommentHtml,
											CreatedOn = comment.CreatedOn,
											CommentId = comment.Id,
											IsActive = comment.IsActive,
											DisplayOrder = comment.DisplayOrder,
											Id = comment.Id,
											ModifiedOn = comment.ModifiedOn,
											Username = comment.Username
										};

										foreach (var rep in _replyService.GetAllRepliesByComment(comment.Id))
										{

											comm.Replies.Add(new ReplyModel()
											{
												CreatedOn = rep.CreatedOn,
												DisplayOrder = rep.DisplayOrder,
												Id = rep.Id,
												IsModified = rep.IsModified,
												ReplyHtml = rep.ReplyHtml,
                                                UserName = rep.UserId.ToString()
											});
										}
										comm.postReplyModel.Type = "Blog";
										model.Add(comm);
									}
								}
							}
						}
						break;
					case "Event":
						if (id > 0)
						{
							var _event = _eventService.GetEventById(id);
							if (_event != null)
							{
								if (_event.Comments.Count > 0)
								{
									foreach (var comment in _event.Comments)
									{
										var comm = new CommentModel()
										{
											CommentHtml = comment.CommentHtml,
											CreatedOn = comment.CreatedOn,
											CommentId = comment.Id,
											IsActive = Convert.ToBoolean(comment.IsActive),
											DisplayOrder = comment.DisplayOrder,
											Id = comment.Id,
											ModifiedOn = comment.ModifiedOn,
											Username = comment.Username
										};

										foreach (var rep in _replyService.GetAllRepliesByComment(comment.Id))
										{

											comm.Replies.Add(new ReplyModel()
											{
												CreatedOn = rep.CreatedOn,
												DisplayOrder = rep.DisplayOrder,
												Id = rep.Id,
												IsModified = rep.IsModified,
												ReplyHtml = rep.ReplyHtml,
												UserName = rep.UserId.ToString()
											});
										}
										comm.postReplyModel.Type = "Event";
										model.Add(comm);
									}
								}
							}
						}
						break;
					default:
						break;
				}

				return PartialView(model.OrderByDescending(x => x.DisplayOrder));
			}

			return PartialView(model);
		}

	}
}
