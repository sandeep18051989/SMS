using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(BookIssueModelValidator))]
	public partial class BookIssueModel : BaseEntityModel
	{
		public BookIssueModel()
		{
			Employee = new EmployeeModel();
			Student = new StudentModel();
			Book = new BookModel();
		}
		public int BookId { get; set; }
		public BookModel Book { get; set; }
		public StudentModel Student { get; set; }
		public int StudentId { get; set; }
		public string Username { get; set; }
		public DateTime IssueDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public double PenaltyAmount { get; set; }
		// EmployeeId
		public int LibrarianId { get; set; }
		public EmployeeModel Employee { get; set; }
	}
}
