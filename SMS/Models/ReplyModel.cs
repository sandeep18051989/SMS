using SMS.Validations;
using EF.Services;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMS.Models;

namespace SMS.Models
{
	[Validator(typeof(ReplyModelValidator))]
	public class ReplyModel : BaseEntityModel
	{
		public ReplyModel()
		{
			Reactions = new List<ReactionModel>();
            User = new UserModel();
            Student = new StudentModel();
            Teacher = new TeacherModel();
		}
		public string ReplyHtml { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsModified { get; set; }
		public int CommentId { get; set; }
		public string UserName { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public DateTime ReplyDate { get; set; }
		public IList<ReactionModel> Reactions { get; set; }

        public UserModel User { get; set; }
        public StudentModel Student { get; set; }
        public TeacherModel Teacher { get; set; }
    }
}