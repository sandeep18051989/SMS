using EF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
    public partial class QuestionModel  : BaseEntityModel
    {
        public QuestionModel()
        {
            AvailableSubjects = new List<SelectListItem>();
            AvailableQuestionTypes = new List<SelectListItem>();
            AvailableLevels = new List<SelectListItem>();
            MatchFollowingOptions = new List<OptionModel>();
        }
        public string Name { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string Explanation { get; set; }
        public string Difficulty { get; set; }
        public int QuestionTypeId { get; set; }
        public int? SubjectId { get; set; }
        public Guid QuestionGuid { get; set; }
        public string SolveTime { get; set; }
        public int DifficultyLevelId { get; set; }
        public double RightMarks { get; set; }
        public double NegativeMarks { get; set; }
        public bool IsTimeBound { get; set; }
        public bool IsActive { get; set; }

        public int OptionCount { get; set; }
        public IList<SelectListItem> AvailableSubjects { get; set; }
        public IList<OptionModel> MatchFollowingOptions { get; set; }
        public IList<SelectListItem> AvailableQuestionTypes { get; set; }
        public IList<SelectListItem> AvailableLevels { get; set; }
    }
}