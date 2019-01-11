using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class BookIssue : BaseEntity
	{
		public int BookId { get; set; }
		public virtual Book Book { get; set; }
		public virtual Student Student { get; set; }
		public int StudentId { get; set; }
		public string Username { get; set; }
		public DateTime? IssueDate { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public double PenaltyAmount { get; set; }
		public int LibrarianId { get; set; }
		public virtual Employee Employee { get; set; }

    }
}
