﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    [Validator(typeof(TeacherExamModelValidator))]
    public partial class TeacherExamModel : BaseEntityModel
    {
        public TeacherExamModel()
        {
            AvailableResultStatuses = new List<SelectListItem>();
            AvailableGradeSystem = new List<SelectListItem>();
            Comments = new List<CommentModel>();
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableExams = new List<SelectListItem>();
            AvailableClassRooms = new List<SelectListItem>();
        }
        public int TeacherId { get; set; }
        public int AcadmicYearId { get; set; }
        public int ExamId { get; set; }
        public int ResultStatusId { get; set; }
        public int GradeSystemId { get; set; }
        public double PassingMarks { get; set; }
        public double MaxMarks { get; set; }
        public double MarksObtained { get; set; }
        public string Exam { get; set; }
        public string Teacher { get; set; }
        public bool BreakAllowed { get; set; }
        public string BreakTime { get; set; }
        public int ClassRoomId { get; set; }
        public string ClassRoom { get; set; }
        public bool Selected { get; set; }
        [UIHint("DateRange")]
        public DateTime? StartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? EndDate { get; set; }
        [UIHint("MDTimePicker")]
        public string StartTime { get; set; }
        [UIHint("MDTimePicker")]
        public string EndTime { get; set; }

        public IList<SelectListItem> AvailableResultStatuses { get; set; }
        public IList<SelectListItem> AvailableGradeSystem { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<SelectListItem> AvailableExams { get; set; }
        public IList<SelectListItem> AvailableClassRooms { get; set; }
        public IList<CommentModel> Comments { get; set; }

    }
}
