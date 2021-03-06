﻿using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
	[Validator(typeof(BookIssueModelValidator))]
	public partial class BookIssueModel : BaseEntityModel
	{
		public BookIssueModel()
		{
            AvailableBooks = new List<SelectListItem>();
            AvailableEmployees = new List<SelectListItem>();
            AvailableStudents = new List<SelectListItem>();
        }
		public int BookId { get; set; }
		public int StudentId { get; set; }
        public string Book { get; set; }
        public string Student { get; set; }
        public string Username { get; set; }
        [UIHint("DateRange")]
		public DateTime? StartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? EndDate { get; set; }
        public string StringStartDate { get; set; }
        public string StringEndDate { get; set; }
        public double PenaltyAmount { get; set; }
        public string Librarian { get; set; }
        // EmployeeId
        public int LibrarianId { get; set; }
        public IList<SelectListItem> AvailableBooks { get; set; }
        public IList<SelectListItem> AvailableEmployees { get; set; }
        public IList<SelectListItem> AvailableStudents { get; set; }

    }
}
