﻿using System;
using FluentValidation.Attributes;
using SMS.Validations;
using EF.Core.Data;
using System.Collections.Generic;
using EF.Services;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    [Validator(typeof(CommentModelValidator))]
    public partial class CommentModel : BaseEntityModel
    {
        public CommentModel()
        {
            Replies = new List<ReplyModel>();
            User = new UserModel();
            Reactions = new List<ReactionModel>();
        }
        public int CommentId { get; set; }
        public int EventId { get; set; }
        public int ProductId { get; set; }
        public int NewsId { get; set; }
        public int BlogId { get; set; }
        public int HomeworkId { get; set; }
        public int ExamId { get; set; }
        public int DisplayOrder { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        [AllowHtml]
        public string CommentHtml { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public UserModel User { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<ReplyModel> Replies { get; set; }
        public IList<ReactionModel> Reactions { get; set; }

    }
}