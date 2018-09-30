using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Areas.Admin.Models
{
	public class AdminCommentsModel
	{
		public AdminCommentsModel()
		{
			Replies = new List<ReplyModel>();
		}
		public int DisplayOrder { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsApproved { get; set; }
		public string CommentHtml { get; set; }
		public string UserName { get; set; }
		public int UserId { get; set; }
		public DateTime CommentDate { get; set; }
		public IList<ReplyModel> Replies { get; set; }
	}
}