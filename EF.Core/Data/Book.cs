using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Book : BaseEntity
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public double Price { get; set; }
		public int BookStatusId { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		[NotMapped]
		public BookStatus BookStatus
		{
			get
			{
				return (BookStatus)this.BookStatusId;
			}
			set
			{
				this.BookStatusId = (int)value;
			}
		}


	}
}
