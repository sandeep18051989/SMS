using System;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(MessageModelValidator))]
	public partial class MessageModel : BaseEntityModel
	{
		public MessageModel()
		{
			Student = new StudentModel();
			MessageGroup = new MessageGroupModel();
		}
		public int StudentId { get; set; }
		public int MessageGroupId { get; set; }
		public DateTime MessageDate { get; set; }
		[AllowHtml]
		public string Text { get; set; }
		public StudentModel Student { get; set; }
		public MessageGroupModel MessageGroup { get; set; }
	}
}
