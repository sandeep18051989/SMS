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
	public class CommentController : PublicHttpController
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

		public CommentController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IProductService productService, IEventService eventService, IEmailService emailService, ITemplateService templateService)
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

		#region Actions

		//[HttpPost]
		//[ValidateInput(false)]
		//public ActionResult PostComment(FormCollection frm)
		//{
		//	TempData.Clear();
		//	var user = _userContext.CurrentUser;
		//	var model = new PostCommentsModel();
		//	if (!String.IsNullOrEmpty(frm["Username"].ToString()))
		//	{
		//		model.Id = model.EntityId = frm["EntityId"] != null ? Convert.ToInt32(frm["EntityId"].ToString()) : 0;
		//		model.CommentHtml = frm["CommentHtml"]?.ToString() ?? "";
		//		model.CreatedOn = DateTime.Now;
		//		model.Username = frm["Username"]?.ToString() ?? "";
		//		model.Type = frm["Type"]?.ToString() ?? "";

		//		var comment = new Comment()
		//		{
		//			CommentHtml = frm["CommentHtml"]?.ToString() ?? "",
		//			CreatedOn = DateTime.Now,
		//			IsActive = true,
		//			IsDeleted = false,
		//			IsApproved = true,
		//			ModifiedOn = DateTime.Now,
		//			UserId = user != null ? user.Id : 0,
		//			Username = frm["Username"]?.ToString() ?? ""
		//		};

		//		// Get Email Settings for Use
		//		var settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
		//		switch (model.Type)
		//		{
		//			case "Blog":
		//				var blog = _blogService.GetBlogById(model.Id);
		//				if (blog != null)
		//				{
		//					comment.DisplayOrder = blog.Comments.Count + 1;
		//					_commentService.Insert(comment);
		//					blog.Comments.Add(comment);
		//					_blogService.Update(blog);
		//				}

		//				// Send Notification To The Admin
		//				if (settings.Count > 0)
		//				{
		//					var settingTeplate = _settingService.GetSettingByKey("CommentOnBlog");
		//					var template = _templateService.GetTemplateByName(settingTeplate.Value);
		//					if (template != null)
		//					{
		//						// Replace Dynamic Data
		//						var tokens = new List<DataToken>();
		//						_templateService.AddBlogTokens(tokens, blog);
		//						if (_userContext.CurrentUser != null)
		//						{
		//							_templateService.AddUserTokens(tokens, _userContext.CurrentUser);
		//						}

		//						foreach (var dt in tokens)
		//						{
		//							template.BodyHtml = EF.Services.CodeHelper.Replace(template.BodyHtml.ToString(), "[" + dt.SystemName + "]", dt.Value, StringComparison.InvariantCulture);
		//						}

		//						var adminEmail = _settingService.GetSettingByKey("FromEmail");
		//						if (adminEmail != null)
		//						{
		//							if (!String.IsNullOrEmpty(adminEmail.Value))
		//								_emailService.SendMailUsingTemplate(adminEmail.Value, model.Username + "Posted A Comment", template);
		//						}
		//					}
		//				}

		//				SuccessNotification("Comment successfully added on blog.");
		//				return RedirectToAction("Detail", "Blog", new { id = blog.Id });
		//			case "Event":
		//				if (model.Id > 0)
		//				{
		//					var _event = _eventService.GetEventById(model.Id);
		//					if (_event != null)
		//					{
		//						comment.DisplayOrder = _event.Comments.Count + 1;
		//						_commentService.Insert(comment);
		//						_event.Comments.Add(comment);
		//						_eventService.Update(_event);
		//					}

		//					// Send Notification To The Admin
		//					if (settings.Count > 0)
		//					{
		//						var settingTeplate = _settingService.GetSettingByKey("CommentOnEvent");
		//						var template = _templateService.GetTemplateByName(settingTeplate.Value);
		//						if (template != null)
		//						{

		//							// Replace Dynamic Data
		//							if (_event != null)
		//							{
		//								template.BodyHtml = template.BodyHtml.Replace("[EventName]", _event.Title);
		//								template.BodyHtml = template.BodyHtml.Replace("[EventDate]", _event.ModifiedOn.HasValue ? _event.ModifiedOn.Value.ToString("U") : "");
		//								template.BodyHtml = template.BodyHtml.Replace("[EventDescription]", _event.Description);
		//								template.BodyHtml = template.BodyHtml.Replace("[EventComment]", model.CommentHtml);
		//								template.BodyHtml = template.BodyHtml.Replace("[Username]", model.Username);
		//							}

		//							var tokens = new List<DataToken>();
		//							_templateService.AddEventTokens(tokens, _event);
		//							if (_userContext.CurrentUser != null)
		//							{
		//								_templateService.AddUserTokens(tokens, _userContext.CurrentUser);
		//							}

		//							foreach (var dt in tokens)
		//							{
		//								template.BodyHtml = EF.Services.CodeHelper.Replace(template.BodyHtml.ToString(), "[" + dt.SystemName + "]", dt.Value, StringComparison.InvariantCulture);
		//							}

		//							var adminEmail = _settingService.GetSettingByKey("FromEmail");
		//							if (adminEmail != null)
		//							{
		//								if (!String.IsNullOrEmpty(adminEmail.Value))
		//									_emailService.SendMailUsingTemplate(adminEmail.Value, model.Username + "Posted A Comment", template);
		//							}
		//						}
		//					}

		//					SuccessNotification("Comment successfully added on event.");
		//					return RedirectToAction("Detail", "Event", new { id = GenerateSlug(model.EntityId.ToString(), _eventService.GetEventById(model.EntityId).Title) });
		//				}
		//				break;
		//			default:
		//				break;
		//		}

		//	}

		//	return RedirectToAction("Detail", "Product", new { id = GenerateSlug(model.EntityId.ToString(), _productService.GetProductById(model.EntityId).Name) });
		//}

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

		public ActionResult Detail(int id)
		{
			var model = new ProductModel();
			var product = _productService.GetProductById(id);
			if (product != null)
			{
				model = new ProductModel()
				{
					Id = product.Id,
					Pictures = product.Pictures.Select(p => new ProductPictureModel()
					{
						Id = p.Id,
						PictureId = p.PictureId,
						Picture = new PictureModel()
						{
							Id = p.Picture.Id,
							IsActive = p.Picture.IsActive,
							Url = p.Picture.Url,
							AlternateText = p.Picture.AlternateText,
                            PictureSrc = p.Picture.PictureSrc
						},
						DisplayOrder = p.DisplayOrder,
						PicEndDate = p.EndDate,
						IsDefault = p.IsDefault,
						PicStartDate = p.StartDate,
					}).OrderBy(p => p.DisplayOrder).ToList(),
					Name = product.Name,
					UserId = product.UserId,
				};
			}
			return View(model);
		}

		public ActionResult PageNotFound()
		{
			return View();
		}

		#endregion

	}
}
